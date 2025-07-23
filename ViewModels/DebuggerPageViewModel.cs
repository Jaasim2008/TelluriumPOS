using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;
using System.Diagnostics;

namespace TelluriumPOS.ViewModels
{
    public partial class DebuggerPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string displaypreferencesFromDatabase;

        [RelayCommand]
        private async Task ReturnToMainMenu()
        {
            await Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        private void OpenCmd() //For windows only!
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = @"C:\ProgramData\TelluriumPOS",
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                {
                    Debug.WriteLine(ex);
                }
            }
        }

        [RelayCommand]
        public static void RunLogViewer() //For windows only!
        {
            var process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.WorkingDirectory = @"C:\ProgramData\TelluriumPOS"; // where log.db is
            process.StartInfo.Arguments = "/k sqlite3 log.db \"SELECT * FROM logs;\"";
            process.StartInfo.UseShellExecute = true; // needed to show the CMD window

            process.Start();
        }

        public async Task OnMount()
        {
            await GetDisplaypreferences();
        }

        private async Task GetDisplaypreferences()
        {
            string selectQuery = "SELECT id, option, value FROM preferences;";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                using var reader = command.ExecuteReader();

                string result = "";
                while (await reader.ReadAsync())
                {
                    int id = reader.GetInt32(0);
                    string option = reader.GetString(1);
                    string value = reader.GetString(2);
                    result += $"{id} | {option} | {value}\n";
                }
                DisplaypreferencesFromDatabase = result;
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);

            }
        }
    }
}
