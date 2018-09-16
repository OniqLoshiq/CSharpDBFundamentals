namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var deserializedData = JsonConvert.DeserializeObject<DepartmentDto[]>(jsonString);

            var departments = new List<Department>();
            var cells = new List<Cell>();

            var sb = new StringBuilder();

            foreach (var dep in deserializedData)
            {
                if (!IsValid(dep) || dep.Cells.Any(c => !IsValid(c)) || cells.Any(c => dep.Cells.Any(dc => dc.CellNumber == c.CellNumber)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                foreach (var cellDto in dep.Cells)
                {
                    var cell = new Cell()
                    {
                        CellNumber = cellDto.CellNumber,
                        HasWindow = cellDto.HasWindow
                    };

                    cells.Add(cell);
                }

                var department = new Department()
                {
                    Name = dep.Name,
                    Cells = cells
                };

                departments.Add(department);

                sb.AppendLine($"Imported {dep.Name} with {dep.Cells.Count} cells");
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var deserializedData = JsonConvert.DeserializeObject<PrisonerDto[]>(jsonString);

            var prisoners = new List<Prisoner>();

            var sb = new StringBuilder();

            foreach (var prisonerDto in deserializedData)
            {
                if (!IsValid(prisonerDto) || prisonerDto.Mails.Any(m => !IsValid(m)))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var mails = new List<Mail>();

                foreach (var mailDto in prisonerDto.Mails)
                {
                    var mail = new Mail()
                    {
                        Description = mailDto.Description,
                        Sender = mailDto.Sender,
                        Address = mailDto.Address
                    };

                    mails.Add(mail);
                }

                DateTime releaseDate;
                DateTime.TryParseExact(prisonerDto.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out releaseDate);

                var prisoner = new Prisoner()
                {
                    FullName = prisonerDto.FullName,
                    Nickname = prisonerDto.Nickname,
                    Age = prisonerDto.Age,
                    IncarcerationDate = DateTime.ParseExact(prisonerDto.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                    ReleaseDate = releaseDate,
                    Bail = prisonerDto.Bail,
                    CellId = prisonerDto.CellId,
                    Mails = mails
                };
                prisoners.Add(prisoner);

                sb.AppendLine($"Imported {prisonerDto.FullName} {prisonerDto.Age} years old");
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(OfficerDto[]), new XmlRootAttribute("Officers"));
            var deserializedData = (OfficerDto[])serializer.Deserialize(new StringReader(xmlString));

            List<OfficerPrisoner> officerPrisoners = new List<OfficerPrisoner>();

            var sb = new StringBuilder();

            foreach (var officerDto in deserializedData)
            {

                if (!IsValid(officerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                object positionType;
                bool validPosition = Enum.TryParse(typeof(Position), officerDto.Position, out positionType);

                object weaponType;
                bool validWeapon = Enum.TryParse(typeof(Weapon), officerDto.Weapon, out weaponType);

                if (positionType == null || weaponType == null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var officer = new Officer()
                {
                    FullName = officerDto.FullName,
                    Salary = officerDto.Salary,
                    Position = (Position)positionType,
                    Weapon = (Weapon)weaponType,
                    DepartmentId = officerDto.DepartmentId
                };

                foreach (var officerPrisonerDto in officerDto.Prisoners)
                {
                    var officerPrisoner = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = officerPrisonerDto.Id
                    };

                    officerPrisoners.Add(officerPrisoner);
                }

                sb.AppendLine($"Imported {officerDto.FullName} ({officerDto.Prisoners.Count()} prisoners)");
            }

            context.OfficersPrisoners.AddRange(officerPrisoners);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new ValidationContext(obj);
            var validationResults = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validationContext, validationResults, true);

            return result;
        }
    }
}