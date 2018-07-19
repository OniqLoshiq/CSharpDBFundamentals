using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using System;

namespace P01_BillPaymentSystem.Initializer.Generators
{
    public class UserGenerator
    {
        private static Random rnd = new Random();

        public static User NewUser (BillsPaymentSystemContext ctx)
        {
            string firstName = NameGenerator.FirstName();
            string lastName = NameGenerator.LastName();
            int passwordLength = PasswordGenerator.CreateRandomPasswordLength();

            var user = new User()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = EmailGenerator.NewEmail(firstName + lastName),
                Password = PasswordGenerator.CreateRandomPassword(passwordLength)
            };

            return user;
        }
    }
}
