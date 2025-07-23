using TelluriumPOS.ViewModels;

namespace TelluriumPOS;

public partial class Settings : ContentPage
{
    public Settings()
    {
        InitializeComponent();
        BindingContext = new SettingsViewModel();
    }

    //Basically like svelte's OnMount()
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SettingsViewModel viewModel)
        {
            await viewModel.OnMount();
        }
    }

    private void YesRadioCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        bool isChecked = e.Value;
        //When the user chooses/ticks yes, this sends to the view model to update showtodayssalesValue (bool) to true
        if (isChecked)
        {
            if (BindingContext is SettingsViewModel vm)
            {
                vm.YesShowTodaysSalesRadio();
            }
        }
    }

}
