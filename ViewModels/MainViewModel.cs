using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TelluriumPOS.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [RelayCommand]
        private async Task goToTerminalAsync()
        {
            await Shell.Current.GoToAsync(nameof(TerminalPage));
        }
        [RelayCommand]
        private async Task goToSettingsAsync()
        {
            await Shell.Current.GoToAsync(nameof(Settings));
        }
        [RelayCommand]
        private async Task goToDebuggerAsync()
        {
            await Shell.Current.GoToAsync(nameof(DebuggerPage));

        }
        [RelayCommand]
        private async Task goToInventoryAsync()
        {
            await Shell.Current.GoToAsync(nameof(InventoryPage));

        }
    }
}
