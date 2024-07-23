using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EOToolsWeb.Views.Ships;

public partial class ShipClassEditView : Window
{
    public ShipClassEditView()
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