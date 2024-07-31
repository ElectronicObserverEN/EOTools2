using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.UseItem;
using EOToolsWeb.ViewModels.UseItem;

namespace EOToolsWeb.ViewModels.EquipmentUpgrades;

public partial class EquipmentUpgradeImprovmentCostUseItemRequirementViewModel : ObservableObject
{
    public List<UseItemModel> UseItems { get; } 

    public UseItemModel Item => UseItems.FirstOrDefault(eq => eq.ApiId == (int)Id)!;

    [ObservableProperty]
    private UseItemId _id;

    [ObservableProperty]
    private int _count;

    public EquipmentUpgradeImprovmentCostItemDetail Model { get; set; } = new();

    private UseItemManagerViewModel UseItemManagerViewModel { get; }

    public EquipmentUpgradeImprovmentCostUseItemRequirementViewModel(UseItemManagerViewModel useItemManagerViewModel)
    {
        UseItemManagerViewModel = useItemManagerViewModel;

        UseItems = Enum.GetValues<UseItemId>().Select(enu => new UseItemModel()
        {
            ApiId = (int)enu,
            NameEN = enu.ToString()
        }).ToList();
    }

    public void LoadFromModel()
    {
        Id = (UseItemId)Model.ItemId;
        Count = Model.Count;
    }

    public void SaveChanges()
    {
        Model.ItemId = Item.ApiId;
        Model.Count = Count;
    }

    [RelayCommand]
    private async Task OpenEquipmentPicker()
    {
        UseItemId? id = await UseItemManagerViewModel.ShowPicker.Handle(null);

        if (id is not { } pickedId) return;

        Id = pickedId;

        OnPropertyChanged(nameof(Item));
    }
}
