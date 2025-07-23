using TelluriumPOS.ViewModels;

namespace TelluriumPOS;

public partial class InventoryPage : ContentPage
{
    public InventoryPage()
    {
        InitializeComponent();
        BindingContext = new InventoryPageViewModel();
    }
    //Basically like svelte's OnMount()
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is InventoryPageViewModel viewModel)
        {
            await viewModel.OnMount();
        }
    }
}