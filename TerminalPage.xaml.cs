using CommunityToolkit.Maui.Views;
using TelluriumPOS.ViewModels;
using GridLength = Microsoft.Maui.GridLength;

namespace TelluriumPOS;
public partial class TerminalPage : ContentPage
{
    private readonly TerminalViewModel viewModel;
    public TerminalPage()
    {
        InitializeComponent();
        viewModel = new TerminalViewModel();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await viewModel.OnMount();
    }
    public async void ShowReturnToHomePopup(object sender, EventArgs e) //putting this in the view model would take my soul away
    {
        var popup = new Popup();

        var grid = new Grid
        {
            Padding = 20,
            BackgroundColor = Colors.AntiqueWhite,
            WidthRequest = 300,
            RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
            ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                },
            RowSpacing = 10,
            ColumnSpacing = 10
        };

        var label = new Label
        {
            Text = "Return to Main Menu?\n(Unsaved Data will be Lost)",
            FontSize = 18,
            HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center,
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center
        };

        Grid.SetRow(label, 0);
        Grid.SetColumnSpan(label, 2);


        var cancelButton = new Button
        {
            Text = "Cancel",
            Command = new Command(() => popup.Close()),
            Background = Colors.DarkRed
        };
        Grid.SetRow(cancelButton, 1);
        Grid.SetColumn(cancelButton, 0);

        var okButton = new Button
        {
            Text = "Yes",
            Background = Colors.ForestGreen,
            Command = new Command(async () =>
            {
                await viewModel.ReturnToMainMenu();
                popup.Close();
            })
        };
        Grid.SetRow(okButton, 1);
        Grid.SetColumn(okButton, 1);

        grid.Children.Add(label);
        grid.Children.Add(cancelButton);
        grid.Children.Add(okButton);

        popup.Content = grid;

        await this.ShowPopupAsync(popup);
    }
    public async void ShowAddItemPopup(object sender, EventArgs e)
    {
        var popup = new Popup();
        var grid = new Grid
        {
            Padding = 20,
            BackgroundColor = Colors.AntiqueWhite,
            WidthRequest = 300,
            RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto }
                },
            ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Star },
                    new ColumnDefinition { Width = GridLength.Star }
                },
            RowSpacing = 10,
            ColumnSpacing = 10
        };
        var header = new Label
        {
            Text = "Add New Item by:",
            FontSize = 18,
            HorizontalTextAlignment = Microsoft.Maui.TextAlignment.Center,
            TextColor = Colors.Black,
            HorizontalOptions = LayoutOptions.Center
        };
        var byScanninglayout = new HorizontalStackLayout
        {
            Spacing = 4,
            Style = (Microsoft.Maui.Controls.Style)Microsoft.Maui.Controls.Application.Current.Resources["HorizontalStackLayoutLikeButtonStyle"],
            Padding = 14
        };
        var byTypingLayout = new HorizontalStackLayout
        {
            Spacing = 4,
            Style = (Microsoft.Maui.Controls.Style)Microsoft.Maui.Controls.Application.Current.Resources["HorizontalStackLayoutLikeButtonStyle"],
            Padding = 14
        };
        var byScanningImage = new Image
        {
            Source = "scan.png",
            WidthRequest = 24,

        };
        var byTypingImage = new Image
        {
            Source = "keyboard.png",
            WidthRequest = 24,
        };
        var byScanningLabel = new Label
        {
            Text = "Scan",
            FontSize = 18,
            VerticalTextAlignment = Microsoft.Maui.TextAlignment.Center,
            TextColor = Colors.White,
        };
        var byTypingLabel = new Label
        {
            Text = "Type",
            FontSize = 18,
            VerticalTextAlignment = Microsoft.Maui.TextAlignment.Center,
            TextColor = Colors.White
        };

        byScanninglayout.Children.Add(byScanningImage);
        byScanninglayout.Children.Add(byScanningLabel);
        byTypingLayout.Children.Add(byTypingImage);
        byTypingLayout.Children.Add(byTypingLabel);

        Grid.SetRow(header, 0);
        Grid.SetColumnSpan(header, 2);
        Grid.SetRow(byScanninglayout, 1);
        Grid.SetColumn(byScanninglayout, 0);
        Grid.SetRow(byTypingLayout, 1);
        Grid.SetColumn(byTypingLayout, 1);


        grid.Children.Add(header);
        grid.Children.Add(byScanninglayout);
        grid.Children.Add(byTypingLayout);

        popup.Content = grid;
        await this.ShowPopupAsync(popup);
    }
}