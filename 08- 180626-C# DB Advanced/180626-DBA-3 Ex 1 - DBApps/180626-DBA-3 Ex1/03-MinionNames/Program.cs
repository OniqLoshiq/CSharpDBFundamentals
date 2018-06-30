using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace _03_MinionNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();

                int villainId = int.Parse(Console.ReadLine());

                string villainName = GetVillainName(villainId, connection);

                if (villainName == null)
                {
                    Console.WriteLine($"No villain with ID {villainId} exists in the database.");
                }
                else
                {
                    Console.WriteLine($"Villain: {villainName}");

                    List<string> minions = GetMinions(villainId, connection);

                    if (minions.Count > 0)
                    {
                        minions.ForEach(m => Console.WriteLine(m));
                    }
                    else
                    {
                        Console.WriteLine("(no minions)");
                    }
                }

                connection.Close();
            }
        }

        private static List<string> GetMinions(int villainId, SqlConnection connection)
        {
            List<string> minions = new List<string>();

            string cmdGetMinions = @"SELECT m.Name, m.Age FROM Minions AS m JOIN MinionsVillains AS mv ON mv.MinionId = m.Id AND mv.VillainId = @id ORDER BY m.Name";

            using (SqlCommand cmd = new SqlCommand(cmdGetMinions, connection))
            {
                cmd.Parameters.AddWithValue("@id", villainId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    int counter = 1;
                    while (reader.Read())
                    {
                        minions.Add($"{counter}. {reader[0]} {reader[1]}");
                        counter++;
                    }
                }
            }

            return minions;
        }

        private static string GetVillainName(int villainId, SqlConnection connection)
        {
            string cmdGetVillain = @"SELECT v.Name  FROM Villains AS v WHERE v.Id = @id";

            using (SqlCommand cmd = new SqlCommand(cmdGetVillain, connection))
            {
                cmd.Parameters.AddWithValue("@id", villainId);
                return (string)cmd.ExecuteScalar();
            }
        }
    }
}
