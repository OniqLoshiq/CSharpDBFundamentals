using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10___3CDD.Dtos
{
    [XmlType("supplier")]
    public class Q3_SupplierDto
    {
        [XmlAttribute("id")]
        public int Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("parts-count")]
        public int PartsCount { get; set; }
    }
}
