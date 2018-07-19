using P01_BillsPaymentSystem.Data.Models.Attributes;
using P01_BillsPaymentSystem.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class PaymentMethod
    {
        [Key]
        public int PaymentMethodId { get; set; }

        [Required]
        public PaymentType Type { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }

        [Xor(nameof(BankAccountId))]
        public int? CreditCardId { get; set; }
        public CreditCard CreditCard { get; set; }

        public int? BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }
    }
}
