using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Ships;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Ships;

public partial class ShipManagerViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _filter = "";

    private List<ShipModel> Ships { get; } = [];

    [ObservableProperty]
    private List<ShipModel> _shipsFiltered = new();

    private HttpClient HttpClient { get; }
    private ShipViewModel ShipViewModel { get; }

    public Interaction<ShipViewModel, bool> ShowEditDialog { get; } = new();
    public Interaction<object?, ShipModel?> ShowPicker { get; } = new();

    public ShipManagerViewModel(HttpClient client, ShipViewModel viewModel)
    {
        HttpClient = client;
        ShipViewModel = viewModel;

        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(Filter))
            {
                ReloadShipList();
            }
        };
    }

    public async Task LoadAllShips()
    {
        if (Ships.Count > 0) return;

        List<ShipModel> ships = await HttpClient.GetFromJsonAsync<List<ShipModel>>("Ships") ?? [];
        Ships.AddRange(ships);

        ReloadShipList();
    }


    private void ReloadShipList()
    {
        ShipsFiltered = Ships.Where(ship => string.IsNullOrEmpty(Filter) || ship.NameEN.Contains(Filter)).ToList();
    }

    [RelayCommand]
    private async Task ShowAddShipDialog()
    {
        ShipModel model = new();

        ShipViewModel.Model = model;
        await ShipViewModel.LoadModel();

        if (await ShowEditDialog.Handle(ShipViewModel))
        {
            ShipViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("Ships", model);

            response.EnsureSuccessStatusCode();

            ShipModel? postedModel = await response.Content.ReadFromJsonAsync<ShipModel>();

            if (postedModel is not null)
            {
                Ships.Add(postedModel);

                ReloadShipList();
            }
        }
    }

    [RelayCommand]
    private async Task EditShip(ShipModel vm)
    {
        ShipViewModel.Model = vm;
        await ShipViewModel.LoadModel();

        if (await ShowEditDialog.Handle(ShipViewModel))
        {
            ShipViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("Ships", vm);

            response.EnsureSuccessStatusCode();

            ReloadShipList();
        }
    }

    [RelayCommand]
    private async Task RemoveShip(ShipModel vm)
    {
        await HttpClient.DeleteAsync($"Ships/{vm.Id}");

        Ships.Remove(vm);

        ReloadShipList();
    }
    
    [RelayCommand]
    private async Task PushTranslations()
    {
        await HttpClient.PutAsync("Ships/pushShips", null);
    }
}
