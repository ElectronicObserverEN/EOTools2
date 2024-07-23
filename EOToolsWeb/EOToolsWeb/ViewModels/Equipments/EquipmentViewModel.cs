using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.EquipmentUpgrades;

namespace EOToolsWeb.ViewModels.Equipments;

public partial class EquipmentViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _nameEN = "";

    [ObservableProperty]
    private string _nameJP = "";

    [ObservableProperty]
    private int _apiId;

    [ObservableProperty] 
    private bool _canBeCrafted;

    public EquipmentModel Model { get; set; } = new();

    public ObservableCollection<EquipmentUpgradeImprovmentModel> Upgrades { get; set; } = [];

    public void LoadFromModel()
    {
        NameJP = Model.NameJP;
        NameEN = Model.NameEN;
        ApiId = Model.ApiId;
        CanBeCrafted = Model.CanBeCrafted;
    }

    public void SaveChanges()
    {
        Model.NameJP = NameJP;
        Model.NameEN = NameEN;
        Model.ApiId = ApiId;
        Model.CanBeCrafted = CanBeCrafted;
    }


    [RelayCommand]
    private async Task ShowAddEquipmentUpgradeDialog()
    {

    }

    [RelayCommand]
    private async Task EditEquipmentUpgrade(EquipmentUpgradeImprovmentModel vm)
    {

    }

    [RelayCommand]
    private async Task RemoveEquipmentUpgrade(EquipmentUpgradeImprovmentModel vm)
    {

    }

}
