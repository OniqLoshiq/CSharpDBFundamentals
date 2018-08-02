using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10.Dtos
{
    [XmlType("product")]
    public class Q4_ProductDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("price")]
        public decimal Price { get; set; }
    }
}
