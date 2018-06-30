using System;
using System.Data.SqlClient;

namespace _09_IncreaseAgeSP
{
    class Program
    {
        static void Main(string[] args)
        {
           // Creation of Procedure in MMS
           // CREATE PROCEDURE usp_GetOlder(@minionId INT) AS
           // BEGIN
           //     UPDATE Minions
           //     SET Age += 1
           //     WHERE Id = @minionId
           // END

            int minionId = int.Parse(Console.ReadLine());
            string uspName = "usp_GetOlder";

            using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();

                ExecuteStoredProcedure(minionId, uspName, connection);

                PrintAffectedMinion(minionId, connection);

                connection.Close();
            }
        }

        private static void PrintAffectedMinion(int minionId, SqlConnection connection)
        {
            string getMinion = @"SELECT Name,Age FROM Minions WHERE Id = @minionId";

            using (SqlCommand cmd = new SqlCommand(getMinion, connection))
            {
                cmd.Parameters.AddWithValue("@minionId", minionId);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    reader.Read();

                    Console.WriteLine($"{reader[0]} - {reader[1]} years old");
                }
            }
        }

        private static void ExecuteStoredProcedure(int minionId, string uspName, SqlConnection connection)
        {
            using (SqlCommand cmd = new SqlCommand(uspName, connection))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@minionId", minionId);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
