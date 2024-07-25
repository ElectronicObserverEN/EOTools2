using Avalonia.Controls;
using Avalonia.Interactivity;

namespace EOToolsWeb.Views.ShipLocks;

public partial class ShipLockEditView : Window
{
    public ShipLockEditView()
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