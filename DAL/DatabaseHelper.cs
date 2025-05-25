using System;
using System.Data.SqlClient;

namespace LibraryManagementSystem.DAL
{
    public static class DatabaseHelper
    {
        private static readonly string connectionString =
     "Server=(localdb)\\mssqllocaldb;Database=LibraryManagementSystem;Trusted_Connection=True;MultipleActiveResultSets=true";

        public static SqlConnection GetConnection()
        {
            try
            {
                var connection = new SqlConnection(connectionString);
                connection.Open(); // Ensures connection is valid before returning
                return connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database connection error: {ex.Message}");
                throw; // Rethrow exception to prevent silent failures
            }
        }
    }
}