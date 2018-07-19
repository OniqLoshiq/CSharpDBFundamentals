using System;

namespace P01_BillPaymentSystem.Initializer.Generators
{
    public class EmailGenerator
    {
        private static Random rnd = new Random();

        private static string[] domains = { "mail.bg", "abv.bg", "gmail.com", "hotmail.com", "yahoo.com", "gyuvetch.bg", "engineering.bg" };
        
        internal static string NewEmail(string name)
        {
            string domain = domains[rnd.Next(domains.Length)];
            int number = rnd.Next(1, 3333);

            return $"{name.ToLower()}{number}@{domain}";
        }
    }
}
