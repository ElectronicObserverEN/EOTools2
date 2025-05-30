using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.UseItem;
using EOToolsWeb.ViewModels.UseItem;
namespace EOToolsWeb.ViewModels.EquipmentUpgrades;

public partial class EquipmentUpgradeImprovmentExtraCostViewModel(UseItemManagerViewModel useItemManager) : ObservableObject
{
    public UpgradeLevel CurrentLevel { get; set; }
    public ObservableCollection<UpgradeLevel> Levels { get; set; } = new();

    public List<UpgradeLevel> UpgradeLevels { get; } = Enum.GetValues<UpgradeLevel>().ToList();

    public ObservableCollection<EquipmentUpgradeImprovmentCostUseItemRequirementViewModel> UseItemsRequired { get; set; } = new();

    public EquipmentUpgradeExtraCost Model { get; set; } = new();

    private UseItemManagerViewModel UseItemManager { get; } = useItemManager;

    public void LoadFromModel()
    {
        UseItemsRequired = [];

        foreach (var consumable in Model.Consumables)
        {
            EquipmentUpgradeImprovmentCostUseItemRequirementViewModel vm = new(UseItemManager);
            vm.Model = consumable;
            vm.LoadFromModel();

            UseItemsRequired.Add(vm);
        }

        Levels = [];

        foreach (var lvl in Model.Levels)
        {
            Levels.Add(lvl.Level);
        }
    }

    public void SaveChanges()
    {
        Model.Consumables.Clear();
        Model.Levels.Clear();

        foreach (UpgradeLevel level in Levels)
        {
            Model.Levels.Add(new EquipmentUpgradeExtraCostLevel()
            {
                Level = level
            });
        }

        foreach (EquipmentUpgradeImprovmentCostUseItemRequirementViewModel vm in UseItemsRequired)
        {
            vm.SaveChanges();

            Model.Consumables.Add(vm.Model);
        }
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
    private void AddLevel()
    {
        Levels.Add(CurrentLevel);
    }

    [RelayCommand]
    private void RemoveLevel(UpgradeLevel lvl)
    {
        Levels.Remove(lvl);
    }

    [RelayCommand]
    public void RemoveUseItemRequirement(EquipmentUpgradeImprovmentCostUseItemRequirementViewModel vm)
    {
        UseItemsRequired.Remove(vm);
    }
}
