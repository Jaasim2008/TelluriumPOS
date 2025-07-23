using TelluriumPOS.ViewModels;


namespace TelluriumPOS;

public partial class DebuggerPage : ContentPage
{
    public DebuggerPage()
    {
        InitializeComponent();
        BindingContext = new DebuggerPageViewModel();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is DebuggerPageViewModel viewModel)
        {
            await viewModel.OnMount();
        }
    }
}