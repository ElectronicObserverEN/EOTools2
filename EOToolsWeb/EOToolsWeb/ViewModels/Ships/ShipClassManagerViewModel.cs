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

public partial class ShipClassManagerViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _filter = "";

    private List<ShipClassModel> ShipClass { get; } = [];

    [ObservableProperty]
    private List<ShipClassModel> _shipClassListFiltered = new();

    private HttpClient HttpClient { get; }

    public Interaction<ShipClassViewModel, bool> ShowEditDialog { get; } = new();

    private ShipClassViewModel ShipClassViewModel { get; }

    public ShipClassManagerViewModel(HttpClient client, ShipClassViewModel viewModel)
    {
        HttpClient = client;
        ShipClassViewModel = viewModel;

        PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(Filter))
            {
                ReloadList();
            }
        };
    }

    public async Task LoadAllClasses()
    {
        if (ShipClass.Count > 0) return;

        List<ShipClassModel> classes = await HttpClient.GetFromJsonAsync<List<ShipClassModel>>("ShipClasses") ?? [];
        ShipClass.AddRange(classes);

        ReloadList();
    }


    private void ReloadList()
    {
        ShipClassListFiltered = ShipClass.Where(ship => string.IsNullOrEmpty(Filter) || ship.NameEnglish.Contains(Filter)).ToList();
    }
    
    [RelayCommand]
    private async Task ShowAddShipClassDialog()
    {
        ShipClassModel model = new();

        ShipClassViewModel.Model = model;
        ShipClassViewModel.LoadModel();

        if (await ShowEditDialog.Handle(ShipClassViewModel))
        {
            ShipClassViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("ShipClasses", model);

            response.EnsureSuccessStatusCode();

            ShipClassModel? postedModel = await response.Content.ReadFromJsonAsync<ShipClassModel>();

            if (postedModel is not null)
            {
                ShipClass.Add(postedModel);

                ReloadList();
            }
        }
    }

    [RelayCommand]
    private async Task EditClass(ShipClassModel vm)
    {
        ShipClassViewModel.Model = vm;
        ShipClassViewModel.LoadModel();

        if (await ShowEditDialog.Handle(ShipClassViewModel))
        {
            ShipClassViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync("ShipClasses", vm);

            response.EnsureSuccessStatusCode();

            ReloadList();
        }
    }

    [RelayCommand]
    private async Task RemoveClass(ShipClassModel vm)
    {
        await HttpClient.DeleteAsync($"ShipClasses/{vm.Id}");

        ShipClass.Remove(vm);

        ReloadList();
    }

    [RelayCommand]
    private async Task PushTranslations()
    {
        await HttpClient.PutAsync("Ships/pushShips", null);
    }
}
