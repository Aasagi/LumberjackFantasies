using System.Data;
using System.Data.SqlClient;

namespace DatabaseConnection
{
    public class DBOperations
    {
        public int UpdateUserHighScore(string userName, int userScore)
        {
            const string ConnectionString = "Server=katiapypfe.database.windows.net;" + "Database=LumberDB;" + "User ID=Gamer" + "Password=BdCanUse6;";
            var rowsAffected = 0;

            using (var dbConnection = new SqlConnection(ConnectionString))
            {
                dbConnection.Open();

                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO HighScore (UserName, Score) VALUES (@userName, @score);";
                    command.Parameters.Add(userName);
                    command.Parameters.Add(userScore);

                    rowsAffected = command.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }

    }
}
