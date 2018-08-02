using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10___3CDD.Dtos
{
    [XmlRoot("customers")]
    public class Q5_CustomerRootDto
    {
        [XmlElement("customer")]
        public Q5_CustomerDto[] Customers { get; set; }
    }
}
