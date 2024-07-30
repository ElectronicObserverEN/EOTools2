using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.ViewModels.Equipments;
using EOToolsWeb.ViewModels.Ships;

namespace EOToolsWeb.ViewModels.EquipmentUpgrades;

public partial class EquipmentUpgradeImprovmentViewModel(ShipManagerViewModel shipManager, EquipmentManagerViewModel equipmentManager) : ObservableObject
{
    public EquipmentUpgradeImprovmentModel Model { get; set; } = new();

    private EquipmentUpgradeConversionViewModel ConversionViewModel { get; } = new(equipmentManager);

    public EquipmentUpgradeImprovmentCostViewModel CostViewModel { get; set; } = new(equipmentManager);

    private EquipmentManagerViewModel EquipmentManager { get; } = equipmentManager;

    public ObservableCollection<EquipmentUpgradeHelpersViewModel> Helpers { get; set; } = [];

    public string AfterConversionEquipmentName => ConversionViewModel?.Equipment?.Id > 0 ? ConversionViewModel.Equipment.NameEN : "Select an equipment";

    private ShipManagerViewModel ShipManager { get; } = shipManager;

    public async Task LoadFromModel()
    {
        ConversionViewModel.Model = Model.ConversionData ?? new();
        await ConversionViewModel.LoadFromModel();

        CostViewModel.Model = Model.Costs;
        await CostViewModel.LoadFromModel();

        Helpers.Clear();

        foreach (var helper in Model.Helpers)
        {
            EquipmentUpgradeHelpersViewModel vm = new(ShipManager)
            {
                Model = helper,
            };

            await vm.LoadFromModel();

            Helpers.Add(vm);
        }
    }

    public void SaveChanges()
    {
        ConversionViewModel.SaveChanges();
        CostViewModel.SaveChanges();

        foreach (EquipmentUpgradeHelpersViewModel helpers in Helpers)
        {
            helpers.SaveChanges();
        }

        Model.ConversionData = ConversionViewModel.Model.IdEquipmentAfter switch
        {
            0 => null,
            _ => ConversionViewModel.Model,
        };

        Model.Costs = CostViewModel.Model;
        Model.Helpers = Helpers.Select(vm => vm.Model).ToList();

        foreach (var helper in Model.Helpers)
        {
            helper.Improvment = Model;
        }
    }

    [RelayCommand]
    private async Task OpenEquipmentPicker()
    {
        EquipmentModel? model = await EquipmentManager.ShowPicker.Handle(null);

        if (model is null) return;

        ConversionViewModel.Model = new()
        {
            EquipmentLevelAfter = 0,
            IdEquipmentAfter = model.ApiId
        };

        await ConversionViewModel.LoadFromModel();

        OnPropertyChanged(nameof(AfterConversionEquipmentName));
    }


    [RelayCommand]
    private async Task ClearEquipment()
    {
        ConversionViewModel.Model = new();
        await ConversionViewModel.LoadFromModel();

        OnPropertyChanged(nameof(AfterConversionEquipmentName));
    }

    [RelayCommand]
    private void AddHelpers()
    {
        EquipmentUpgradeHelpersModel model = new();
        Helpers.Add(new(ShipManager));
    }

    [RelayCommand]
    private void RemoveHelpers(EquipmentUpgradeHelpersViewModel vm)
    {
        Helpers.Remove(vm);
    }
}
