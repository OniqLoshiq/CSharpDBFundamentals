using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _05_ChangeTownNamesCasing
{
    class Program
    {
        static void Main(string[] args)
        {
            string country = Console.ReadLine();

            using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();

                int countryId = GetCountryId(country, connection);
                ChangeTownNames(countryId, connection);

                connection.Close();
            }
        }

        private static void ChangeTownNames(int countryId, SqlConnection connection)
        {
            string updateTownNames = @"UPDATE Towns SET Name = UPPER(Name) WHERE CountryCode = @countryId";

            using (SqlCommand cmd = new SqlCommand(updateTownNames, connection))
            {
                cmd.Parameters.AddWithValue("@countryId", countryId);

                int affectedRows = cmd.ExecuteNonQuery();

                if(affectedRows == 0)
                {
                    Console.WriteLine("No town names were affected.");
                }
                else
                {
                    Console.WriteLine($"{affectedRows} town names were affected.");

                    PrintChangedTowns(countryId, connection);
                }
            }
        }

        private static void PrintChangedTowns(int countryId, SqlConnection connection)
        {
            string getTownNames = @"SELECT Name FROM Towns WHERE CountryCode = @countryId";
            List<string> towns = new List<string>();

            using (SqlCommand cmd = new SqlCommand(getTownNames, connection))
            {
                cmd.Parameters.AddWithValue("@countryId", countryId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        towns.Add(reader[0].ToString());
                    }
                }
            }

            Console.WriteLine("[{0}]", string.Join(", ", towns));
        }

        private static int GetCountryId(string country, SqlConnection connection)
        {
            string getCountryId = @"SELECT Id FROM Countries WHERE Name = @name";

            using (SqlCommand cmd = new SqlCommand(getCountryId, connection))
            {
                cmd.Parameters.AddWithValue("@name", country);

                if(cmd.ExecuteScalar() == null)
                {
                    Console.WriteLine("No town names were affected.");
                    Environment.Exit(0);
                }

                return (int)cmd.ExecuteScalar();
            }
        }
    }
}
