using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.UseItem;

namespace EOToolsWeb.ViewModels.EquipmentUpgrades;

public partial class EquipmentUpgradeImprovmentCostUseItemRequirementViewModel : ObservableObject
{
    public List<UseItemModel> UseItems { get; } 

    public UseItemModel Item => UseItems.FirstOrDefault(eq => eq.ApiId == Id)!;

    [ObservableProperty]
    private int _id;

    [ObservableProperty]
    private int _count;

    public EquipmentUpgradeImprovmentCostItemDetail Model { get; set; } = new();

    public EquipmentUpgradeImprovmentCostUseItemRequirementViewModel()
    {
        UseItems = Enum.GetValues<UseItemId>().Select(enu => new UseItemModel()
        {
            ApiId = (int)enu,
            NameEN = enu.ToString()
        }).ToList();
    }

    public void LoadFromModel()
    {
        Id = Model.ItemId;
        Count = Model.Count;
    }

    public void SaveChanges()
    {
        Model.ItemId = Item.ApiId;
        Model.Count = Count;
    }

    [RelayCommand]
    public void OpenEquipmentPicker()
    {
        /*EquipmentPickerViewModel vm = new(UseItems.Select(item => new EquipmentModel()
        {
            ApiId = item.ApiId,
            NameEN = item.NameEN
        }).ToList());

        EquipmentDataPickerView picker = new(vm);

        if (picker.ShowDialog() == true && vm.SelectedEquipment != null)
        {
            Id = vm.SelectedEquipment.ApiId;

            OnPropertyChanged(nameof(Item));
        }*/
    }
}
