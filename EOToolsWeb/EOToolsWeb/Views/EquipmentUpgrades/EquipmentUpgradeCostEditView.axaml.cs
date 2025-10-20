using Avalonia.Controls;
using EOToolsWeb.ViewModels.EquipmentUpgrades;

namespace EOToolsWeb.Views.EquipmentUpgrades;

public partial class EquipmentUpgradeCostEditView : UserControl
{
    public EquipmentUpgradeImprovmentCostViewModel? ViewModel => DataContext as EquipmentUpgradeImprovmentCostViewModel;

    public EquipmentUpgradeCostEditView()
    {
        InitializeComponent();
    }
}