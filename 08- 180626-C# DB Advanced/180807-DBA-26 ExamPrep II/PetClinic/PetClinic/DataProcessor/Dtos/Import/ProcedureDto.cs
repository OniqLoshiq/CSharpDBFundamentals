using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace PetClinic.DataProcessor.Dtos.Import
{
    [XmlType("Procedure")]
    public class ProcedureDto
    {
        [XmlElement("Vet")]
        [Required]
        public string VetName { get; set; }

        [XmlElement("Animal")]
        [Required]
        public string AnimalSerialNumber { get; set; }

        [XmlElement("DateTime")]
        [Required]
        public string DateTime { get; set; }

        [XmlArray("AnimalAids")]
        public AnimalAidDto2[] AnimalAids { get; set; }
    }
}
