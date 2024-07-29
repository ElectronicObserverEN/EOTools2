using Avalonia.Controls;
using Avalonia.Interactivity;
using EOToolsWeb.ViewModels.EquipmentUpgrades;

namespace EOToolsWeb.Views.EquipmentUpgrades;

public partial class EquipmentUpgradeEditView : Window
{
    public EquipmentUpgradeImprovmentViewModel? ViewModel => DataContext as EquipmentUpgradeImprovmentViewModel;

    public EquipmentUpgradeEditView()
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