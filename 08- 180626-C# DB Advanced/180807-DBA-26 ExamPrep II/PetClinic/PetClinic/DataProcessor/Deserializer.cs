namespace PetClinic.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Newtonsoft.Json;
    using PetClinic.Data;
    using PetClinic.DataProcessor.Dtos.Import;
    using PetClinic.Models;

    public class Deserializer
    {
        private const string ErrorMessage = "Error: Invalid data.";
        private const string SuccessMessage = "Record {0} successfully imported.";
        private const string SuccessAnimal = "Record {0} Passport №: {1} successfully imported.";
        private const string SuccessProcedure = "Record successfully imported.";

        public static string ImportAnimalAids(PetClinicContext context, string jsonString)
        {
            var deserializedAnimalAids = JsonConvert.DeserializeObject<AnimalAidDto[]>(jsonString);

            List<AnimalAid> animalAids = new List<AnimalAid>();

            var sb = new StringBuilder();

            foreach (var animalAidDto in deserializedAnimalAids)
            {
                if(!IsValid(animalAidDto) || animalAids.Any(n => n.Name == animalAidDto.Name))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var animalAid = new AnimalAid()
                {
                    Name = animalAidDto.Name,
                    Price = animalAidDto.Price
                };

                animalAids.Add(animalAid);
                sb.AppendLine(string.Format(SuccessMessage, animalAidDto.Name));
            }

            context.AnimalAids.AddRange(animalAids);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportAnimals(PetClinicContext context, string jsonString)
        {
            var deserializedAnimals = JsonConvert.DeserializeObject<AnimalDto[]>(jsonString);

            List<Animal> animals = new List<Animal>();

            var sb = new StringBuilder();

            foreach(var animalDto in deserializedAnimals)
            {
                if(!IsValid(animalDto) || !IsValid(animalDto.Passport) || animals.Any(a => a.Passport.SerialNumber == animalDto.Passport.SerialNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var animal = new Animal()
                {
                    Name = animalDto.Name,
                    Type = animalDto.Type,
                    Age = animalDto.Age,
                    Passport = new Passport()
                    {
                        SerialNumber = animalDto.Passport.SerialNumber,
                        OwnerName = animalDto.Passport.OwnerName,
                        OwnerPhoneNumber = animalDto.Passport.OwnerPhoneNumber,
                        RegistrationDate = DateTime.ParseExact(animalDto.Passport.RegistrationDate, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                    }
                };

                animals.Add(animal);

                sb.AppendLine(string.Format(SuccessAnimal, animalDto.Name, animalDto.Passport.SerialNumber));
            }

            context.Animals.AddRange(animals);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportVets(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(VetDto[]), new XmlRootAttribute("Vets"));
            var deserializedVets = (VetDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Vet> vets = new List<Vet>();

            var sb = new StringBuilder();

            foreach (var vetDto in deserializedVets)
            {
                if (!IsValid(vetDto) || vets.Any(v => v.PhoneNumber == vetDto.PhoneNumber))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var vet = new Vet()
                {
                    Name = vetDto.Name,
                    Profession = vetDto.Profession,
                    Age = vetDto.Age,
                    PhoneNumber = vetDto.PhoneNumber
                };

                vets.Add(vet);
                sb.AppendLine(string.Format(SuccessMessage, vetDto.Name));
            }

            context.Vets.AddRange(vets);
            context.SaveChanges();

            return sb.ToString().Trim();
        }

        public static string ImportProcedures(PetClinicContext context, string xmlString)
        {
            var serializer = new XmlSerializer(typeof(ProcedureDto[]), new XmlRootAttribute("Procedures"));
            var deserializedProcedures = (ProcedureDto[])serializer.Deserialize(new StringReader(xmlString));

            List<Procedure> procedures = new List<Procedure>();
            List<ProcedureAnimalAid> procedureAnimalAids = new List<ProcedureAnimalAid>();

            var sb = new StringBuilder();

            foreach (var proc in deserializedProcedures)
            {
                var vet = context.Vets.SingleOrDefault(v => v.Name == proc.VetName);
                var animal = context.Animals.SingleOrDefault(a => a.PassportSerialNumber == proc.AnimalSerialNumber);

                if (vet == null 
                    || animal == null
                    || proc.AnimalAids.Length != proc.AnimalAids.Distinct().Count()
                    || context.AnimalAids.Where(x => proc.AnimalAids.Any(p => p.Name == x.Name)).Count() != proc.AnimalAids.Count())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var procedure = new Procedure()
                {
                    Vet = vet,
                    Animal = animal,
                    DateTime = DateTime.ParseExact(proc.DateTime, "dd-MM-yyyy", CultureInfo.InvariantCulture)
                };

                List<AnimalAid> animalAids = context.AnimalAids.Where(ai => proc.AnimalAids.Any(x => x.Name == ai.Name)).ToList();

                foreach (var aid in animalAids)
                {
                    var procedureAnimalAid = new ProcedureAnimalAid()
                    {
                        AnimalAid = aid,
                        Procedure = procedure
                    };

                    procedureAnimalAids.Add(procedureAnimalAid);
                }

                procedures.Add(procedure);

                sb.AppendLine(SuccessProcedure);
            }

            context.Procedures.AddRange(procedures);
            context.SaveChanges();

            context.ProceduresAnimalAids.AddRange(procedureAnimalAids);
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
