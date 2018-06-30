using System;
using System.Data.SqlClient;
using System.Linq;

namespace _08_IncreaseMinionAge
{
    class Program
    {
        static void Main(string[] args)
        {
            int[] minionIndeuces = Console.ReadLine().Split().Select(int.Parse).ToArray();

            using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();

                for (int i = 0; i < minionIndeuces.Length; i++)
                {
                    UpdateMinionAge(minionIndeuces[i], connection);
                }

                PrintAllMinions(connection);
                
                connection.Close();
            }
        }

        private static void PrintAllMinions(SqlConnection connection)
        {
            string minionsInfo = @"SELECT Name, Age FROM Minions";

            using (SqlCommand cmd = new SqlCommand(minionsInfo, connection))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while(reader.Read())
                    {
                        Console.WriteLine($"{reader[0]} {reader[1]}");
                    }
                }
            }
        }

        private static void UpdateMinionAge(int index, SqlConnection connection)
        {
            string minionToUpdate = @"UPDATE Minions SET Age += 1 WHERE Id = @index";

            using (SqlCommand cmd = new SqlCommand(minionToUpdate, connection))
            {
                cmd.Parameters.AddWithValue("@index", index);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
