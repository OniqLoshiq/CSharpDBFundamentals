using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10.Dtos
{
    [XmlType("sold-products")]
    public class Q4_SoldProductDto
    {
        [XmlAttribute("count")]
        public int ProductsCount { get; set; }

        [XmlElement("product")]
        public Q4_ProductDto[] Products { get; set; }
    }
}
