namespace SoftJail.DataProcessor
{
    using AutoMapper;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ExportDto;
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var prisoners = context.Prisoners
                .Where(p => ids.Any(i => i == p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers
                                        .Select(o => new
                                        {
                                            OfficerName = o.Officer.FullName,
                                            Department = o.Officer.Department.Name
                                        })
                                        .OrderBy(o => o.OfficerName)
                                        .ToArray(),
                    TotalOfficerSalary = p.PrisonerOfficers.Sum(po => po.Officer.Salary)
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToArray();

            var jsonString = JsonConvert.SerializeObject(prisoners, new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            });

            return jsonString;
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {
            var prisonersRequested = prisonersNames.Split(',');
            
            var prisoners = context.Prisoners.Where(p => prisonersRequested.Any(pr => pr == p.FullName)).Include(p => p.Mails).ToArray();
            var newPrisoners = Mapper.Map<PrisonerDto[]>(prisoners)
                .OrderBy(p => p.FullName)
                .ThenBy(p => p.Id)
                .ToArray();

            var serializer = new XmlSerializer(typeof(PrisonerDto[]), new XmlRootAttribute("Prisoners"));

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), newPrisoners, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            return sb.ToString().Trim();
        }
    }
}