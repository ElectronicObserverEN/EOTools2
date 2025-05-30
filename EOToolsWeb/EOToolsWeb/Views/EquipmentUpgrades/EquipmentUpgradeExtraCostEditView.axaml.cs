using Avalonia.Controls;
using EOToolsWeb.ViewModels.EquipmentUpgrades;

namespace EOToolsWeb.Views.EquipmentUpgrades;

public partial class EquipmentUpgradeExtraCostEditView : UserControl
{
    public EquipmentUpgradeImprovmentExtraCostViewModel? ViewModel => DataContext as EquipmentUpgradeImprovmentExtraCostViewModel;

    public EquipmentUpgradeExtraCostEditView()
    {
        InitializeComponent();
    }
}