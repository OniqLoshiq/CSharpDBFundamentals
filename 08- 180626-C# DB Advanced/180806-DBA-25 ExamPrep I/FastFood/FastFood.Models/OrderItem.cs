using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FastFood.Models
{
    public class OrderItem
    {
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        [Required]
        public Order Order { get; set; }

        [ForeignKey("Item")]
        public int ItemId { get; set; }
        [Required]
        public Item Item { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
