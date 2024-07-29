using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Ships;

namespace EOToolsWeb.ViewModels.EquipmentUpgrades;

public partial class EquipmentUpgradeHelpersViewModel(ShipManagerViewModel shipManager)
{
    public int ShipId { get; set; }

    public DayOfWeek Day { get; set; }

    public ObservableCollection<EquipmentUpgradeHelpersShipViewModel> Ships { get; set; } = new();
    public ObservableCollection<EquipmentUpgradeHelpersDayModel> CanHelpOnDays { get; set; } = new();

    public EquipmentUpgradeHelpersModel Model { get; set; } = new();

    public List<DayOfWeek> Days => Enum.GetValues<DayOfWeek>().ToList();

    private ShipManagerViewModel ShipManager { get; } = shipManager;

    public async Task LoadFromModel()
    {
        Ships = [];

        foreach (var ship in Model.ShipIds)
        {
            EquipmentUpgradeHelpersShipViewModel vm = new EquipmentUpgradeHelpersShipViewModel(ShipManager)
            {
                Model = ship,
            };

            await vm.LoadFromModel();

            Ships.Add(vm);
        }

        CanHelpOnDays = new(Model.CanHelpOnDays);
    }

    public void SaveChanges()
    {
        Model.CanHelpOnDays = CanHelpOnDays.ToList();
        Model.ShipIds = Ships.Select(vm => vm.Model).ToList();
    }


    [RelayCommand]
    public void AddShipId()
    {
        EquipmentUpgradeHelpersShipModel model = new()
        {
            ShipId = ShipId
        };

        Ships.Add(new EquipmentUpgradeHelpersShipViewModel(ShipManager));
    }

    [RelayCommand]
    public void AddDay()
    {
        EquipmentUpgradeHelpersDayModel model = new()
        {
            Day = Day
        };

        CanHelpOnDays.Add(model);
    }

    [RelayCommand]
    public void RemoveShipId(EquipmentUpgradeHelpersShipViewModel id)
    {
        Ships.Remove(id);
    }

    [RelayCommand]
    public void RemoveDay(EquipmentUpgradeHelpersDayModel day)
    {
        CanHelpOnDays.Remove(day);
    }
}
