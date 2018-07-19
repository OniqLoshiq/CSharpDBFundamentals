using System;

namespace P01_BillPaymentSystem.Initializer.Generators
{
    public class PasswordGenerator
    {
        private static Random rnd1 = new Random();

        internal static int CreateRandomPasswordLength()
        {
            return rnd1.Next(6, 15);
        }

        internal static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789_-";
            char[] chars = new char[passwordLength];
            Random rnd2 = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rnd2.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
