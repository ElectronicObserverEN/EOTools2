using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.UseItem;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.UseItem;

namespace EOToolsWeb.ViewModels.EquipmentUpgrades;

public partial class EquipmentUpgradeImprovmentCostDetailViewModel(EquipmentManagerViewModel equipmentManager, UseItemManagerViewModel useItemManager) : ObservableObject
{
    [ObservableProperty]
    private int _devmatCost;

    [ObservableProperty]
    private int _sliderDevmatCost;

    [ObservableProperty]
    private int _improveMatCost;

    [ObservableProperty]
    private int _sliderImproveMatCost;
    
    public ObservableCollection<EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel> EquipmentsRequired { get; set; } = new();
    public ObservableCollection<EquipmentUpgradeImprovmentCostUseItemRequirementViewModel> UseItemsRequired { get; set; } = new();

    public EquipmentUpgradeImprovmentCostDetail Model { get; set; } = new();

    private EquipmentManagerViewModel EquipmentManager { get; } = equipmentManager;
    private UseItemManagerViewModel UseItemManager { get; } = useItemManager;

    public async Task LoadFromModel()
    {
        DevmatCost = Model.DevmatCost;
        SliderDevmatCost = Model.SliderDevmatCost;
        ImproveMatCost = Model.ImproveMatCost;
        SliderImproveMatCost = Model.SliderImproveMatCost;

        EquipmentsRequired = [];

        foreach (var equipmentDetail in Model.EquipmentDetail)
        {
            EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel vm = new(EquipmentManager);
            vm.Model = equipmentDetail;
            await vm.LoadFromModel();

            EquipmentsRequired.Add(vm);
        }
        
        UseItemsRequired = [];

        foreach (var consumable in Model.ConsumableDetail)
        {
            EquipmentUpgradeImprovmentCostUseItemRequirementViewModel vm = new(UseItemManager);
            vm.Model = consumable;
            vm.LoadFromModel();

            UseItemsRequired.Add(vm);
        }
    }

    public void SaveChanges()
    {
        Model.DevmatCost = DevmatCost;
        Model.SliderDevmatCost = SliderDevmatCost;
        Model.ImproveMatCost = ImproveMatCost;
        Model.SliderImproveMatCost = SliderImproveMatCost;

        Model.EquipmentDetail.Clear();
        Model.ConsumableDetail.Clear();

        foreach (EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel vm in EquipmentsRequired)
        {
            vm.SaveChanges();

            Model.EquipmentDetail.Add(vm.Model);
        }
        
        foreach (EquipmentUpgradeImprovmentCostUseItemRequirementViewModel vm in UseItemsRequired)
        {
            vm.SaveChanges();

            Model.ConsumableDetail.Add(vm.Model);
        }
    }
    
    [RelayCommand]
    public async Task AddEquipmentRequirement()
    {
        EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel vm = new(EquipmentManager);

        await vm.OpenEquipmentPickerCommand.ExecuteAsync(null);

        if (vm.Id > 0)
        {
            EquipmentsRequired.Add(vm);
        }
    }

    [RelayCommand]
    public void RemoveEquipmentRequirement(EquipmentUpgradeImprovmentCostEquipmentRequirementViewModel vm)
    {
        EquipmentsRequired.Remove(vm);
    }

    [RelayCommand]
    private async Task AddUseItemRequirement()
    {
        UseItemId? pickedId = await UseItemManager.ShowPicker.Handle(null);

        if (pickedId > 0)
        {
            EquipmentUpgradeImprovmentCostUseItemRequirementViewModel vm = new(UseItemManager)
            {
                Model = new()
                {
                    ItemId = (int)pickedId,
                },
            };

            vm.LoadFromModel();

            UseItemsRequired.Add(vm);
        }
    }

    [RelayCommand]
    public void RemoveUseItemRequirement(EquipmentUpgradeImprovmentCostUseItemRequirementViewModel vm)
    {
        UseItemsRequired.Remove(vm);
    }
}
