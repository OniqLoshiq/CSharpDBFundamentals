using System;
using System.Data.SqlClient;

namespace _04_AddMinion
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] minionData = Console.ReadLine().Split();
            string minionName = minionData[1];
            int age = int.Parse(minionData[2]);
            string town = minionData[3];

            string villainName = Console.ReadLine().Split()[1];

            using (SqlConnection connection = new SqlConnection(Config.ConnectionString))
            {
                connection.Open();

                int townId = GetTownId(town, connection);

                int villainId = GetVillainId(villainName, connection);

                int minionId = InsertMinionAndGetId(minionName, age, townId, connection);

                AddMinionToVillain(minionId, villainId, connection);

                Console.WriteLine($"Successfully added {minionName} to be minion of {villainName}");

                connection.Close();
            }
        }

        private static void AddMinionToVillain(int minionId, int villainId, SqlConnection connection)
        {
            string addMinionsVillains = @"INSERT INTO MinionsVillains VALUES (@minionId, @villainId)";

            using (SqlCommand cmd = new SqlCommand(addMinionsVillains, connection))
            {
                cmd.Parameters.AddWithValue("@minionId", minionId);
                cmd.Parameters.AddWithValue("@villainId", villainId);

                cmd.ExecuteNonQuery();
            }
        }

        private static int InsertMinionAndGetId(string minionName, int age, int townId, SqlConnection connection)
        {
            string insertMinion = @"INSERT INTO Minions VALUES (@name, @age, @townId)";

            using (SqlCommand cmd = new SqlCommand(insertMinion, connection))
            {
                cmd.Parameters.AddWithValue("@name", minionName);
                cmd.Parameters.AddWithValue("@age", age);
                cmd.Parameters.AddWithValue("@townId", townId);

                cmd.ExecuteNonQuery();
            }
            string getMinionId = @"SELECT Id FROM Minions WHERE Name = @name";
            using (SqlCommand cmd = new SqlCommand(getMinionId, connection))
            {
                cmd.Parameters.AddWithValue("@name", minionName);

                return (int)cmd.ExecuteScalar();
            }
        }

        private static int GetVillainId(string villainName, SqlConnection connection)
        {
            string getVillainId = @"SELECT Id FROM Villains WHERE Name = @name";

            using (SqlCommand cmd = new SqlCommand(getVillainId, connection))
            {
                cmd.Parameters.AddWithValue("@name", villainName);

                if (cmd.ExecuteScalar() == null)
                {
                    InsertVillainToDb(villainName, connection);
                    Console.WriteLine($"Villain {villainName} was added to the database.");
                }

                return (int)cmd.ExecuteScalar();
            }
        }

        private static void InsertVillainToDb(string villainName, SqlConnection connection)
        {
            string insertTown = @"INSERT INTO Villains VALUES (@name, 4)";

            using (SqlCommand cmd = new SqlCommand(insertTown, connection))
            {
                cmd.Parameters.AddWithValue("@name", villainName);
                cmd.ExecuteNonQuery();
            }
        }

        private static int GetTownId(string town, SqlConnection connection)
        {
            string getTownId = @"SELECT Id FROM Towns WHERE Name = @town";

            using (SqlCommand cmd = new SqlCommand(getTownId, connection))
            {
                cmd.Parameters.AddWithValue("@town", town);

                if (cmd.ExecuteScalar() == null)
                {
                    InsertTownToDb(town, connection);
                    Console.WriteLine($"Town {town} was added to the database.");
                }

                return (int)cmd.ExecuteScalar();
            }
        }

        private static void InsertTownToDb(string town, SqlConnection connection)
        {
            string insertTown = @"INSERT INTO Towns (Name) VALUES (@town)";

            using (SqlCommand cmd = new SqlCommand(insertTown, connection))
            {
                cmd.Parameters.AddWithValue("@town", town);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
