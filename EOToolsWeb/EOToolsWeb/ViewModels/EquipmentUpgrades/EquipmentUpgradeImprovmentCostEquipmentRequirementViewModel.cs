using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Equipments;

namespace EOToolsWeb.ViewModels.EquipmentUpgrades;

public partial class EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel(EquipmentManagerViewModel equipmentManager) : ObservableObject
{
    public EquipmentModel Equipment => EquipmentManager.AllEquipments.FirstOrDefault(eq => eq.ApiId == Id) ?? new();

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _count;

    public EquipmentUpgradeImprovmentCostItemDetail Model { get; set; } = new();

    private EquipmentManagerViewModel EquipmentManager { get; } = equipmentManager;
    
    public async Task LoadFromModel()
    {
        await EquipmentManager.LoadAllEquipments();

        Id = Model.ItemId;
        Count = Model.Count;
    }

    public void SaveChanges()
    {
        Model.ItemId = Equipment.ApiId;
        Model.Count = Count;
    }

    [RelayCommand]
    private async Task OpenEquipmentPicker()
    {
        EquipmentModel? model = await EquipmentManager.ShowPicker.Handle(null);

        if (model is null) return;

        Id = model.ApiId;

        OnPropertyChanged(nameof(Equipment));
    }
}
