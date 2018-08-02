using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10___3CDD.Dtos
{
    [XmlType("sale")]
    public class Q6_SaleDto
    {
        [XmlElement("car")]
        public Q6_CarDto CarData { get; set; }

        [XmlElement("customer-name")]
        public string CustomerName { get; set; }

        [XmlElement("discount")]
        public double Discount { get; set; }

        [XmlElement("price")]
        public decimal CarPrice { get; set; }

        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount
        {
            get
            {
                return (decimal)(1 - this.Discount) * this.CarPrice;
            }
            set
            {
                this.PriceWithDiscount = value;
            }
        }
    }
}
