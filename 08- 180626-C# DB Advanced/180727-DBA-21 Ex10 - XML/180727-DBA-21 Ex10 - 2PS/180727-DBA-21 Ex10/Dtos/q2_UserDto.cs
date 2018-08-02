using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10.Dtos
{
    [XmlType("user")]
    public class Q2_UserDto
    {
        [XmlAttribute("first-name")]
        public string FirstName { get; set; }

        [XmlAttribute("last-name")]
        public string LastName { get; set; }

        [XmlArray("sold-products")]
        public Q2_SoldProductDto[] SoldProducts { get; set; }
    }
}
