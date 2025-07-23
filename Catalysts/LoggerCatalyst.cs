using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace TelluriumPOS.Catalysts
{
    public enum SignificanceLevels
    {
        INFO,
        WARNING,
        CRITICAL
    }
    public interface ILoggerCatalyst
    {
        void Log(string message, SignificanceLevels significance, string username, string context);
    }
    public class Logger : ILoggerCatalyst
    {
        public void Log(string message, SignificanceLevels significance, string username, string context)
        {
            string insertQuery = @"
                INSERT INTO logs (message, time, significance, username, context)
                VALUES (@message, @time, @significance, @username, @context);";

            try
            {
                using var conn = new SqliteConnection(@"Data Source=C:\ProgramData\TelluriumPOS\log.db");
                conn.Open();

                using var command = new SqliteCommand(insertQuery, conn);
                command.Parameters.AddWithValue("@message", message);
                command.Parameters.AddWithValue("@time", DateTime.UtcNow.ToString("o")); // ISO format
                command.Parameters.AddWithValue("@significance", significance.ToString());
                command.Parameters.AddWithValue("@username", username ?? "Unknown");
                command.Parameters.AddWithValue("@context", context ?? "General");

                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Logging failed: {ex.Message}"); //🤣 cannot log an error while the logger is not working
            }
            if (significance == SignificanceLevels.WARNING || significance == SignificanceLevels.CRITICAL)
            {
                Application.Current.MainPage.DisplayAlert("Error!", $"Some error occured! (context: {context})", "OK");
            }
        }
    }
}
