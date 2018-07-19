using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class CreditCard
    {
        [Key]
        public int CreditCardId { get; set; }

        [Required]
        public decimal Limit { get; set; }

        [Required]
        public decimal MoneyOwned { get; set; }

        [NotMapped]
        public decimal LimitLeft => this.Limit - this.MoneyOwned;

        [Required]
        public DateTime ExpirationDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                this.MoneyOwned -= amount;
            }
        }

        public void Withdraw(decimal amount)
        {
            if (this.LimitLeft - amount >= 0)
            {
                this.MoneyOwned += amount;
            }
        }
    }
}
