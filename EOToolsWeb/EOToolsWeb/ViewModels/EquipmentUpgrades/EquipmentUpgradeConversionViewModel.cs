using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Equipments;

namespace EOToolsWeb.ViewModels.EquipmentUpgrades;

public partial class EquipmentUpgradeConversionViewModel(EquipmentManagerViewModel equipmentManager) : ObservableObject
{
    [ObservableProperty]
    private int _equipmentLevelAfter;

    [ObservableProperty]
    private EquipmentModel? _equipment = new();

    public EquipmentUpgradeConversionModel Model { get; set; } = new();

    public EquipmentManagerViewModel EquipmentManager { get; } = equipmentManager;

    public async Task LoadFromModel()
    {
        await EquipmentManager.LoadAllEquipments();

        Equipment = EquipmentManager.EquipmentList.FirstOrDefault(eq => eq.ApiId == Model.IdEquipmentAfter) ?? new();

        EquipmentLevelAfter = Model.EquipmentLevelAfter;
    }

    public void SaveChanges()
    {
        Model.EquipmentLevelAfter = EquipmentLevelAfter;
        Model.IdEquipmentAfter = Equipment?.ApiId ?? 0;
    }
}
