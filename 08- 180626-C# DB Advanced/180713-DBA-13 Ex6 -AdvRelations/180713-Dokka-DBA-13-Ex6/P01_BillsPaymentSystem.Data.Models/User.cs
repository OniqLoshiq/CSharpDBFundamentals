using P01_BillsPaymentSystem.Data.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace P01_BillsPaymentSystem.Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(80)]
        [NonUnicode]
        public string Email { get; set; }

        [Required]
        [MaxLength(25)]
        [NonUnicode]
        public string Password { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"User: {this.FirstName} {this.LastName}");
            sb.AppendLine("Bank Accounts:");

            var bankAccounts = this.PaymentMethods.Where(x => x.BankAccount != null).Select(x => x.BankAccount).ToArray();
            var creditCards = this.PaymentMethods.Where(x => x.CreditCard != null).Select(x => x.CreditCard).ToArray();
            
            foreach (var ba in bankAccounts)
            {
                sb.AppendLine($"-- ID: {ba.BankAccountId}")
                  .AppendLine($"--- Balance: {ba.Balance:f2}")
                  .AppendLine($"--- Bank: {ba.BankName}")
                  .AppendLine($"--- SWIFT: {ba.SWIFTCode}");
            }

            sb.AppendLine("Credit Cards:");
            foreach (var cc in creditCards)
            {
                sb.AppendLine($"-- ID: {cc.CreditCardId}")
                  .AppendLine($"--- Limit: {cc.Limit:f2}")
                  .AppendLine($"--- Limit: {cc.MoneyOwned:f2}")
                  .AppendLine($"--- Limit Left: {cc.LimitLeft:f2}")
                  .AppendLine($"--- Expiration Date: {cc.ExpirationDate.Year}/{cc.ExpirationDate.Month}");
            }

            return sb.ToString().Trim();
        }
    }
}
