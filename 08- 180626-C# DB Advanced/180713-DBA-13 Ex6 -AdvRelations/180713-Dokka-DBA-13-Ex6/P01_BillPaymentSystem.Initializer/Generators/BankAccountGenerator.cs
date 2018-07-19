using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using System;

namespace P01_BillPaymentSystem.Initializer.Generators
{
    public class BankAccountGenerator
    {
        private static Random rnd = new Random();

        public static BankAccount NewBankAccount(BillsPaymentSystemContext ctx)
        {
            string bankName = BankNameGenerator.GenerateBankName();
            string swiftCode = GetSwiftCode(bankName);

            var bankAccount = new BankAccount()
            {
                Balance = (decimal)(rnd.NextDouble() * rnd.Next(1000, 40_000)),
                BankName = bankName,
                SWIFTCode = swiftCode
            };

            return bankAccount;
        }

        private static string GetSwiftCode(string bankName)
        {
            int endIndex = bankName.LastIndexOf("Bank");

            string swiftCode = bankName.Substring(0, endIndex).ToUpper();

            return swiftCode;
        }
    }
}
