using System;

namespace P01_BillPaymentSystem.Initializer.Generators
{
    public class NameGenerator
    {
        private static string[] firstNames = { "Doncho", "Petur", "Ivan", "Georgi", "Alexander", "Stefan", "Vladimir", "Svetoslav", "Kaloyan", "Mihail", "Stamat",
                                                "Yana", "Boryana", "Elana", "Gergana", "Dragana", "Stoyana", "Milena", "Amanda", "Daria"};
       
        private static string[] lastNames = { "Ivanov", "Georgiev", "Stefanov", "Alexandrov", "Petrov", "Stamatkov", "Vasilev", "Mirchev", "Kostadinov", "Programatikov"};
       
        public static string FirstName() => GenerateName(firstNames);
        public static string LastName() => GenerateName(lastNames);

        private static string GenerateName(string[] names)
        {
            Random rnd = new Random();

            int index = rnd.Next(0, names.Length);

            string name = names[index];

            return name;
        }
    }
}
