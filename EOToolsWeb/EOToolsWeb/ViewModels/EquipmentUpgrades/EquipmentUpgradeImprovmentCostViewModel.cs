using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.UseItem;

namespace EOToolsWeb.ViewModels.EquipmentUpgrades;

public partial class EquipmentUpgradeImprovmentCostViewModel(EquipmentManagerViewModel equipmentManager, UseItemManagerViewModel useItemManager)
    : ObservableObject
{
    [ObservableProperty]
    private int _fuel;

    [ObservableProperty]
    private int _ammo;

    [ObservableProperty]
    private int _steel;

    [ObservableProperty]
    private int _bauxite;

    public EquipmentUpgradeImprovmentCostDetailViewModel Cost0To5ViewModel { get; set; } = new(equipmentManager, useItemManager);
    public EquipmentUpgradeImprovmentCostDetailViewModel Cost6To9ViewModel { get; set; } = new(equipmentManager, useItemManager);
    public EquipmentUpgradeImprovmentCostDetailViewModel CostMaxViewModel { get; set; } = new(equipmentManager, useItemManager);

    public EquipmentUpgradeImprovmentCost Model { get; set; } = new();

    public async Task LoadFromModel()
    {
        Fuel = Model.Fuel;
        Ammo = Model.Ammo;
        Steel = Model.Steel;
        Bauxite = Model.Bauxite;

        Cost0To5ViewModel.Model = Model.Cost0To5;
        await Cost0To5ViewModel.LoadFromModel();

        Cost6To9ViewModel.Model = Model.Cost6To9;
        await Cost6To9ViewModel.LoadFromModel();

        CostMaxViewModel.Model = Model.CostMax ?? new();
        await CostMaxViewModel.LoadFromModel();
    }

    public void SaveChanges()
    {
        Model.Fuel = Fuel;
        Model.Ammo = Ammo;
        Model.Steel = Steel;
        Model.Bauxite = Bauxite;

        Cost0To5ViewModel.SaveChanges();
        Model.Cost0To5 = Cost0To5ViewModel.Model;

        Cost6To9ViewModel.SaveChanges();
        Model.Cost6To9 = Cost6To9ViewModel.Model;

        CostMaxViewModel.SaveChanges();

        if (CostMaxViewModel.DevmatCost == 0 && CostMaxViewModel.SliderDevmatCost == 0 && CostMaxViewModel.ImproveMatCost == 0 && CostMaxViewModel.SliderImproveMatCost == 0)
        {
            Model.CostMax = null;
        }
        else if (Model.CostMax is null)
        {
            Model.CostMax = CostMaxViewModel.Model;
        }
    }
}
