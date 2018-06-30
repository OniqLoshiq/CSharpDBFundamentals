using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _07_PrintAllMinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> minions = new List<string>();

            using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();

                minions = GetReorderedMinionNames(connection);

                connection.Close();
            }

            if (minions.Count > 1)
            {
                for (int i = 0; i < minions.Count / 2; i++)
                {
                    Console.WriteLine(minions[i]);
                    Console.WriteLine(minions[minions.Count - 1 - i]);
                }

                if (minions.Count % 2 == 1)
                {
                    Console.WriteLine(minions[minions.Count / 2]);
                }
            }
            else
            {
                Console.WriteLine(minions[0]);
            }
        }

        private static List<string> GetReorderedMinionNames( SqlConnection connection)
        {
            List<string> minions = new List<string>();
            string allMinions = @"SELECT Name FROM Minions";

            using (SqlCommand cmd = new SqlCommand(allMinions, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        minions.Add(reader[0].ToString());
                    }
                }
            }

            return minions;
        }
    }
}
