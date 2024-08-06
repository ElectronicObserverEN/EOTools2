using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Ships;
using ReactiveUI;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;
using EOToolsWeb.Models.Settings;

namespace EOToolsWeb.ViewModels.Ships;

public partial class ShipManagerViewModel : ViewModelBase
{
    [ObservableProperty]
    private string _filter = "";

    public List<ShipModel> Ships { get; } = [];

    [ObservableProperty]
    private List<ShipModel> _shipsFiltered = new();

    private HttpClient HttpClient { get; }
    private ShipViewModel ShipViewModel { get; }
    private ShipClassManagerViewModel ShipClassManagerViewModel { get; }
    private SettingsModel Settings { get; }

    public Interaction<ShipViewModel, bool> ShowEditDialog { get; } = new();
    public Interaction<object?, ShipModel?> ShowPicker { get; } = new();

    public ShipManagerViewModel(HttpClient client, ShipViewModel viewModel, ShipClassManagerViewModel classManager, SettingsModel settings)
    {
        HttpClient = client;
        ShipViewModel = viewModel;
        ShipClassManagerViewModel = classManager;
        Settings = settings;

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

    [RelayCommand]
    public async Task ImportFromAPI()
    {
        /*if (string.IsNullOrEmpty(AppSettings.KancolleEOAPIFolder)) return;

        string importPath = Path.Combine(AppSettings.KancolleEOAPIFolder, "kcsapi", "api_start2", "getData");

        JObject data = JsonHelper.ReadKCJson(importPath);

        using EOToolsDbContext db = new();
        ShipTranslationService translationService = Ioc.Default.GetRequiredService<ShipTranslationService>();

        foreach (JObject shipJson in data["api_data"]["api_mst_ship"].Children<JObject>())
        {
            int apiId = int.Parse(shipJson["api_id"].ToString());
            string nameJp = shipJson["api_name"].ToString();
            ShipClassModel? shipClass = null;

            if (shipJson.ContainsKey("api_ctype") && apiId < 1500)
            {
                int classId = int.Parse(shipJson["api_ctype"].ToString());

                string nameJapanese = GetShipClassUntranslated(classId, apiId);
                shipClass = db.ShipClass.FirstOrDefault(sc => sc.ApiId == classId);

                if (shipClass is null)
                {
                    shipClass = new()
                    {
                        ApiId = classId,
                        NameJapanese = nameJapanese,
                        NameEnglish = translationService.Class(nameJapanese),
                    };

                    await db.ShipClass.AddAsync(shipClass);
                }
            }

            ShipViewModel? vm = Ships.Find(sh => sh.Model.ApiId == apiId);

            if (vm is null)
            {
                ShipModel model = new()
                {
                    ApiId = apiId,
                    NameJP = nameJp
                };

                model.NameEN = model.GetNameEN();

                db.Add(model);

                Ships.Add(new(model));
            }
        }

        await db.SaveChangesAsync();

        ReloadShipList();*/
    }
}
