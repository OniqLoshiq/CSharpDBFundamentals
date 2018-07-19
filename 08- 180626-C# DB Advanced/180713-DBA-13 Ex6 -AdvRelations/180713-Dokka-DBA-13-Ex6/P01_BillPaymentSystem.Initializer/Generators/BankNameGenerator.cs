using System;

namespace P01_BillPaymentSystem.Initializer.Generators
{
    public class BankNameGenerator
    {
        private static Random rnd = new Random();
        private static string[] bankNames = { "Dubai", "Bulgarian", "National", "First", "Bank", "Monument", "ViP"};

        internal static string GenerateBankName()
        {
            int index = rnd.Next(0, bankNames.Length);

            string name = bankNames[index];

            return name + "Bank";
        }
    }
}
