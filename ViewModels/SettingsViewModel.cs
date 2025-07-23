using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using TelluriumPOS.Catalysts;

namespace TelluriumPOS.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        //Init logger
        private readonly ILoggerCatalyst _logger = new Logger();

        [ObservableProperty]
        private string companynameValue;
        [ObservableProperty]
        private string usernameValue;
        [ObservableProperty]
        private string currencyprefixValue;
        [ObservableProperty]
        private bool showtodayssalesValue = false;
        [ObservableProperty]
        private bool notShowtodayssalesValue = true;
        //This method is called from Settings.xaml.cs
        public void YesShowTodaysSalesRadio() { ShowtodayssalesValue = true; }
        [ObservableProperty]
        private int defaultVATPercentageValue;

        [RelayCommand]
        private async Task ReturnToMainMenu()
        {
            await Shell.Current.GoToAsync("..");
        }

        public async Task OnMount()
        {
            await GetCompanynameSetting();
            await GetUsernameSetting();
            await GetCurrencyprefixSetting();
            await GetShowTodaysSalesSetting();
            await GetDefaultVATPercentageSetting();
        }

        [RelayCommand]
        private async Task SaveSettings()
        {
            await SetCompanynameSetting();
            await SetUsernameSetting();
            await SetCurrencyprefixSetting();
            await SetShowTodaysSalesSetting();
            await SetDefaultVATPercentageSetting();
            await Application.Current.MainPage.DisplayAlert("Saved", "Successfully saved!", "OK");
            _logger.Log("Saved Settings", SignificanceLevels.INFO, UsernameValue ?? "Unknown", "SaveSettings");
        }

        private async Task GetCompanynameSetting()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'CompanyName';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                CompanynameValue = result?.ToString() ?? "";
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "GetCompanynameSetting");
            }
        }
        private async Task SetCompanynameSetting()
        {
            string updateQuery = "UPDATE preferences SET value = @value WHERE option = 'CompanyName';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(updateQuery, conn);
                command.Parameters.AddWithValue("@value", CompanynameValue);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "SetCompanynameSetting");
            }
        }
        private async Task GetUsernameSetting()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'UserName';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                UsernameValue = result?.ToString() ?? "";
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "GetUsernameSetting");
            }
        }
        private async Task SetUsernameSetting()
        {
            string updateQuery = "UPDATE preferences SET value = @value WHERE option = 'UserName';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(updateQuery, conn);
                command.Parameters.AddWithValue("@value", UsernameValue);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "SetUsernameSetting");
            }
        }
        private async Task GetCurrencyprefixSetting()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'CurrencyPrefix';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                CurrencyprefixValue = result?.ToString() ?? "";
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "GetCurrencyprefixSetting");
            }
        }
        private async Task SetCurrencyprefixSetting()
        {
            string updateQuery = "UPDATE preferences SET value = @value WHERE option = 'CurrencyPrefix';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(updateQuery, conn);
                command.Parameters.AddWithValue("@value", CurrencyprefixValue);
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "SetCurrencyprefixSetting");
            }
        }
        private async Task GetShowTodaysSalesSetting()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'ShowTodaysSales';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                ShowtodayssalesValue = Convert.ToBoolean(result?.ToString());
                await conn.CloseAsync();
                //set the `No` option to checked if ShowtodayssalesValue is false
                if (!ShowtodayssalesValue) { NotShowtodayssalesValue = true; }
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "GetShowTodaysSalesSetting");
            }
        }
        private async Task SetShowTodaysSalesSetting()
        {
            string updateQuery = "UPDATE preferences SET value = @value WHERE option = 'ShowTodaysSales';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(updateQuery, conn);
                command.Parameters.AddWithValue("@value", ShowtodayssalesValue.ToString());
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "SetShowTodaysSalesSetting");
            }
        }
        private async Task GetDefaultVATPercentageSetting()
        {
            string selectQuery = "SELECT value FROM preferences WHERE option = 'DefaultVATPercentage';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(selectQuery, conn);
                var result = await command.ExecuteScalarAsync();
                Debug.WriteLine("result?.ToString() -> " + result?.ToString());
                DefaultVATPercentageValue = Convert.ToInt32(result?.ToString());
                await conn.CloseAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "GetDefaultVATPercentageSetting");
            }
        }
        private async Task SetDefaultVATPercentageSetting()
        {
            string updateQuery = "UPDATE preferences SET value = @value WHERE option = 'DefaultVATPercentage';";
            try
            {
                using var conn = new SqliteConnection(@"Data Source = C:\ProgramData\TelluriumPOS\displayPreferences.db");
                await conn.OpenAsync();
                using var command = new SqliteCommand(updateQuery, conn);
                command.Parameters.AddWithValue("@value", DefaultVATPercentageValue.ToString());
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _logger.Log(ex.Message, SignificanceLevels.CRITICAL, UsernameValue ?? "Unknown", "SetDefaultVATPercentageSetting");
            }
        }
    }
}
