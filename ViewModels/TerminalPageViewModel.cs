using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;
using TelluriumPOS.Catalysts;

namespace TelluriumPOS.ViewModels
{
    public partial class TerminalViewModel : ObservableObject
    {
        //Init logger
        private readonly ILoggerCatalyst _logger = new Logger();

        [ObservableProperty]
        private string databaseCompanyname;
        [ObservableProperty]
        private string databaseUsername;
        [ObservableProperty]
        private string currentTime;
        [ObservableProperty]
        private string showTodaysSales = "__";
        [ObservableProperty]
        private bool shouldWeShowTodaysSales = false;
        [ObservableProperty]
        private string currencyPrefix;
        [ObservableProperty]
        private string status = "Initializing";
        [ObservableProperty]
        private bool isSalesTabOpen = false;
        [ObservableProperty]
        public bool isEssentialsTabOpen = true;


        private System.Timers.Timer timer;

        public async Task OnMount()
        {
            StartDatetimeCounter();
            await GetCompanynameFromDatabase();
            await GetUsernameFromDatabase();
            await GetCurrencyPrefixFromDatabase();
            await GetShowTodaysSalesFromDatabase();
            Status = "Ready";
        }

        private void StartDatetimeCounter()
        {
            CurrentTime = DateTime.Now.ToString("dd/MM/yy hh:mm tt");

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += (s, e) =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    CurrentTime = DateTime.Now.ToString("dd/MM/yy hh:mm tt");
                });
            };
            timer.Start();
        }
        private async Task GetCompanynameFromDatabase()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'CompanyName';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                DatabaseCompanyname = result?.ToString() ?? "";
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, DatabaseUsername ?? "Unknown", "GetCompanynameFromDatabase");
            }
        }
        private async Task GetUsernameFromDatabase()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'UserName';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                DatabaseUsername = result?.ToString() ?? "";
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, DatabaseUsername ?? "Unknown", "GetUsernameFromDatabase");
            }
        }
        private async Task GetCurrencyPrefixFromDatabase()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'CurrencyPrefix';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                CurrencyPrefix = result?.ToString() ?? "";
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, DatabaseUsername ?? "Unknown", "GetShowTodaysSalesSetting");
            }
        }
        private async Task GetShowTodaysSalesFromDatabase()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'ShowTodaysSales';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                ShouldWeShowTodaysSales = Convert.ToBoolean(result?.ToString());
                await conn.CloseAsync();
                if (ShouldWeShowTodaysSales)
                {
                    ShowTodaysSales = "Today's Sales: " + CurrencyPrefix + "__"; //TODO fix this value later when Analytics page is made
                }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, DatabaseUsername ?? "Unknown", "GetShowTodaysSalesFromDatabase");
            }

        }
        [RelayCommand]
        private void ShowSalesTab()
        {
            IsSalesTabOpen = true;
            IsEssentialsTabOpen = false; //i know this is dumb-est way to switch bools but hey it works
        }
        [RelayCommand]
        private void ShowEssentialsTab()
        {
            IsSalesTabOpen = false;
            IsEssentialsTabOpen = true;
        }
        [RelayCommand]
        public async Task ReturnToMainMenu() //this is called from TerminalPage.xaml.cs
        {
            timer?.Stop(); //to prevent memory leaks
            timer?.Dispose();
            await Shell.Current.GoToAsync("..");
        }
        [RelayCommand]
        private void OpenCalculator() //For Windows Only!
        {
            System.Diagnostics.Process.Start("calc.exe");
        }
    }
}
