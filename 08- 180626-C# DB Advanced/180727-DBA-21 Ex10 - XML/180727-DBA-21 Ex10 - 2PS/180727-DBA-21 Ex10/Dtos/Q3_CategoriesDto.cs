using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10.Dtos
{
    [XmlType("category")]
    public class Q3_CategoriesDto
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("product-count")]
        public int ProductsCount { get; set; }

        [XmlElement("average-price")]
        public decimal AveragePrice { get; set; }

        [XmlElement("total-revenue")]
        public decimal TotalRevenue { get; set; }
    }
}
