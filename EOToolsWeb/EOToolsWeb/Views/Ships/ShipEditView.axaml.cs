using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EOToolsWeb.Views.Ships;

public partial class ShipEditView : Window
{
    public ShipEditView()
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