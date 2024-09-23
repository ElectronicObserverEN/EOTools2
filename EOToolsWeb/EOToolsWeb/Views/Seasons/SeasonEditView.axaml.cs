using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EOToolsWeb.Views.Seasons;

public partial class SeasonEditView : Window
{
    public SeasonEditView()
    {
        InitializeComponent();
    }

    private void OnConfirmClick(object sender, RoutedEventArgs e)
    {
        Close(true);
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        Close(false);
    }
}