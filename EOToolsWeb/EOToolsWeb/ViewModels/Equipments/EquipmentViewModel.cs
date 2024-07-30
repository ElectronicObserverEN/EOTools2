using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.EquipmentUpgrades;
using ReactiveUI;

namespace EOToolsWeb.ViewModels.Equipments;

public partial class EquipmentViewModel(HttpClient client) : ViewModelBase
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

    [ObservableProperty]
    private List<int> _upgradeIds = [];

    private HttpClient HttpClient { get; } = client;

    public Interaction<EquipmentUpgradeImprovmentModel, bool> ShowUpgradeEditDialog { get; } = new();

    public async Task LoadFromModel()
    {
        NameJP = Model.NameJP;
        NameEN = Model.NameEN;
        ApiId = Model.ApiId;
        CanBeCrafted = Model.CanBeCrafted;

        if (Model.ApiId > 0)
        {
            UpgradeIds = await HttpClient.GetFromJsonAsync<List<int>>($"EquipmentUpgrades/{Model.ApiId}") ?? [];
        }
        else
        {
            UpgradeIds = [];
        }
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
        EquipmentUpgradeImprovmentModel model = new();
        
        if (await ShowUpgradeEditDialog.Handle(model))
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync($"EquipmentUpgradeImprovmentModel/{ApiId}", model);

            response.EnsureSuccessStatusCode();

            UpgradeIds = await HttpClient.GetFromJsonAsync<List<int>>($"EquipmentUpgrades/{Model.ApiId}") ?? [];
        }
    }

    [RelayCommand]
    private async Task EditEquipmentUpgrade(int id)
    {
        EquipmentUpgradeImprovmentModel? model = await HttpClient.GetFromJsonAsync<EquipmentUpgradeImprovmentModel>($"EquipmentUpgradeImprovmentModel/{id}");

        if (model is null) return;
        
        if (await ShowUpgradeEditDialog.Handle(model))
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("EquipmentUpgradeImprovmentModel", model);

            response.EnsureSuccessStatusCode();
        }
    }

    [RelayCommand]
    private async Task RemoveEquipmentUpgrade(int id)
    {

    }

}
