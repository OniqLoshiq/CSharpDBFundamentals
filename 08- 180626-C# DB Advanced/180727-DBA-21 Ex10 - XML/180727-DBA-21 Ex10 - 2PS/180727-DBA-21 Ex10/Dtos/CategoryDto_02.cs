using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10.Dtos
{
    [XmlType("category")]
    public class CategoryDto_02
    {
        [XmlElement("name")]
        [StringLength(15,MinimumLength = 3)]
        public string Name { get; set; }
    }
}
