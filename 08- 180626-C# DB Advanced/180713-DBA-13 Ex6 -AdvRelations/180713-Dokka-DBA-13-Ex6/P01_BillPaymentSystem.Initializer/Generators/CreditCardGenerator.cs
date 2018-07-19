using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using System;

namespace P01_BillPaymentSystem.Initializer.Generators
{
    public class CreditCardGenerator
    {
        private static Random rnd = new Random();
        private static DateTime StartDate = new DateTime(2019,1,1);
        private static DateTime EndDate = new DateTime(2028, 1, 1);
        
        public static CreditCard NewCreditCard(BillsPaymentSystemContext ctx)
        {
            DateTime expirationDate = RandomDate();
            decimal limit = (decimal)rnd.Next(1000, 9000);
            decimal moneyOwned = (decimal)(rnd.Next(0, (int)limit) * rnd.NextDouble());

            var creditCard = new CreditCard()
            {
                ExpirationDate = expirationDate,
                Limit = limit,
                MoneyOwned = moneyOwned
            };

            return creditCard;
        }

        private static DateTime RandomDate()
        {
            int range = (EndDate - StartDate).Days;

            return StartDate.AddDays(rnd.Next(range));
        }
    }
}
