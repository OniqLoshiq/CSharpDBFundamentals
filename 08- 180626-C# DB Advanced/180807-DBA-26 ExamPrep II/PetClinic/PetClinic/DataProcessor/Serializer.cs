namespace PetClinic.DataProcessor
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.Dtos.Export;

    public class Serializer
    {
        public static string ExportAnimalsByOwnerPhoneNumber(PetClinicContext context, string phoneNumber)
        {
            var data = context.Passports.Where(p => p.OwnerPhoneNumber == phoneNumber)
                .OrderBy(p => p.Animal.Age)
                .ThenBy(p => p.SerialNumber)
                .Select(p => new
                {
                    OwnerName = p.OwnerName,
                    AnimalName = p.Animal.Name,
                    Age = p.Animal.Age,
                    SerialNumber = p.SerialNumber,
                    RegisteredOn = p.RegistrationDate.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)
                }).ToArray();

            var jsonData = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);

            return jsonData;
        }

        public static string ExportAllProcedures(PetClinicContext context)
        {
            var procedures = context.Procedures
                .OrderBy(p => p.DateTime)
                .ThenBy(p => p.Animal.PassportSerialNumber)
                .Select(p => new ProcedureDto()
                {
                   PassportSerialNumber = p.Animal.PassportSerialNumber,
                   OwnerPhoneNumber = p.Animal.Passport.OwnerPhoneNumber,
                   DateTime = p.DateTime.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                   AnimalAids = p.ProcedureAnimalAids.Select(ai => new AnimalAidDto()
                   {
                       Name = ai.AnimalAid.Name,
                       Price = ai.AnimalAid.Price
                   }).ToArray(),
                   TotalPrice = p.ProcedureAnimalAids.Sum(ai => ai.AnimalAid.Price)
                })
                .ToArray();


            var serializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));

            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), procedures, new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty }));

            return sb.ToString().Trim();
        }
    }
}
