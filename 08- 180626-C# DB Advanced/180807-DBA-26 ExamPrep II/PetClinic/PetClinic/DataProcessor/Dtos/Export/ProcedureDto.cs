using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dtos.Export
{
    [XmlType("Procedure")]
    public class ProcedureDto
    {
        [XmlElement("Passport")]
        public string PassportSerialNumber { get; set; }

        [XmlElement("OwnerNumber")]
        public string OwnerPhoneNumber { get; set; }

        [XmlElement("DateTime")]
        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        public AnimalAidDto[] AnimalAids { get; set; }

        [XmlElement("TotalPrice")]
        public decimal TotalPrice { get; set; }
    }
}
