using System.Linq;
using System.Xml.Serialization;

namespace _180727_DBA_21_Ex10___3CDD.Dtos
{
    [XmlType("customer")]
    public class Q5_CustomerDto
    {
        [XmlAttribute("full-name")]
        public string Name { get; set; }

        [XmlAttribute("bought-cars")]
        public int BoughtCars { get; set; }

        [XmlAttribute("spent-money")]
        public decimal TotalSpentMoney
        {
            get
            {
                return this.DiscountsAndCarPrices.Sum(x => x.TotalCarPrice * (decimal)(1 - x.Discount));
            }
            set
            {
                this.TotalSpentMoney = value;
            }
        }

        [XmlIgnore]
        public Q5_DiscountAndCarPriceDto[] DiscountsAndCarPrices { get; set; }
    }
}
