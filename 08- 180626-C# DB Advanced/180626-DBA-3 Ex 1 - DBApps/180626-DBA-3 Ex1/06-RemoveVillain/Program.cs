using System;
using System.Data.SqlClient;

namespace _06_RemoveVillain
{
    class Program
    {
        static void Main(string[] args)
        {
            int villainId = int.Parse(Console.ReadLine());

            using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();

                string villainName = GetVillainName(villainId, connection);

                int countReleasedMinions = ReleaseMinions(villainId, connection);

                DeleteVillain(villainId, connection);

                Console.WriteLine($"{villainName} was deleted.");
                Console.WriteLine($"{countReleasedMinions} minions were released.");
                
                connection.Close();
            }
        }

        private static void DeleteVillain(int villainId, SqlConnection connection)
        {
            string deleteVillain = @"DELETE FROM Villains WHERE Id = @villainId";

            using (SqlCommand cmd = new SqlCommand(deleteVillain, connection))
            {
                cmd.Parameters.AddWithValue("@villainId", villainId);

                cmd.ExecuteNonQuery();
            }
        }

        private static int ReleaseMinions(int villainId, SqlConnection connection)
        {
            string releaseMinions = @"DELETE FROM MinionsVillains WHERE VillainId = @villainId";

            using (SqlCommand cmd = new SqlCommand(releaseMinions, connection))
            {
                cmd.Parameters.AddWithValue("@villainId", villainId);

                return cmd.ExecuteNonQuery();
            }
        }

        private static string GetVillainName(int villainId, SqlConnection connection)
        {
            string getVillain = @"SELECT Name FROM Villains WHERE Id = @villainId";

            using (SqlCommand cmd = new SqlCommand(getVillain, connection))
            {
                cmd.Parameters.AddWithValue("@villainId", villainId);

                if(cmd.ExecuteScalar() == null)
                {
                    Console.WriteLine("No such villain was found.");
                    Environment.Exit(0);
                }

                return cmd.ExecuteScalar().ToString();
            }
        }
    }
}
