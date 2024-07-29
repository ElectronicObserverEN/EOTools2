using Avalonia.Controls;
using EOToolsWeb.ViewModels.EquipmentUpgrades;

namespace EOToolsWeb.Views.EquipmentUpgrades;

public partial class EquipmentUpgradeEditHelperView : UserControl
{
    public EquipmentUpgradeHelpersViewModel? ViewModel => DataContext as EquipmentUpgradeHelpersViewModel;

    public EquipmentUpgradeEditHelperView()
    {
        InitializeComponent();
    }
}