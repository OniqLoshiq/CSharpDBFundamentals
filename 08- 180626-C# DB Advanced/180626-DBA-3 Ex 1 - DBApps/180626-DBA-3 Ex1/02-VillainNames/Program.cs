using System;
using System.Data.SqlClient;

namespace _02_VillainNames
{
    class Program
    {
        static void Main(string[] args)
        {
            using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();

                string minionsInfo = "SELECT v.Name, COUNT(*) AS [NumOfMinions] FROM Villains AS v JOIN MinionsVillains AS mv ON mv.VillainId = v.Id GROUP BY v.Name HAVING COUNT(*) >= 3 ORDER BY NumOfMinions DESC";

                using (SqlCommand cmd = new SqlCommand(minionsInfo, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            Console.WriteLine($"{reader[0]} -> {reader[1]}");
                        }
                    }

                }
                    connection.Close();
            }
        }
    }
}
