using Avalonia.Controls;
using Avalonia.Interactivity;
using EOToolsWeb.ViewModels.ShipLocks;

namespace EOToolsWeb.Views.ShipLocks;

public partial class ShipLockPhaseEditView : Window
{
    public ShipLockPhaseViewModel? ViewModel => DataContext as ShipLockPhaseViewModel;

    public ShipLockPhaseEditView()
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