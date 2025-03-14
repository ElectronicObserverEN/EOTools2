﻿using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Models.Settings;
using EOToolsWeb.Shared.Ships;
using ReactiveUI;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using EOToolsWeb.Extensions.Translations;
using EOToolsWeb.ViewModels.Translations;
using EOToolsWeb.Models.Translations;
using EOToolsWeb.Shared.Translations;

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
    private ShipTranslationManager Translation { get; }

    public Interaction<ShipViewModel, bool> ShowEditDialog { get; } = new();
    public Interaction<object?, ShipModel?> ShowPicker { get; } = new();

    public ShipManagerViewModel(HttpClient client, ShipViewModel viewModel, ShipClassManagerViewModel classManager, SettingsModel settings, ShipTranslationManager translation)
    {
        HttpClient = client;
        ShipViewModel = viewModel;
        ShipClassManagerViewModel = classManager;
        Settings = settings;
        Translation = translation;

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
        try
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
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    private async Task EditShip(ShipModel vm)
    {
        try
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
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    private async Task RemoveShip(ShipModel vm)
    {
        try
        {
            await HttpClient.DeleteAsync($"Ships/{vm.Id}");

            Ships.Remove(vm);

            ReloadShipList();
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }
    
    [RelayCommand]
    private async Task PushTranslations()
    {
        try
        {
            await HttpClient.PutAsync("Ships/pushShips", null);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    public async Task ImportFromAPI()
    {
        if (string.IsNullOrEmpty(Settings.KancolleEoApiFolder)) return;

        await Translation.Initialize();

        string importPath = Path.Combine(Settings.KancolleEoApiFolder, "kcsapi", "api_start2", "getData");

        string text = await File.ReadAllTextAsync(importPath);
        // --- revome svdata=
        text = text[7..];

        JsonObject? data = JsonSerializer.Deserialize<JsonObject>(text);

        if (data is null) return;

        foreach (JsonObject shipJson in data["api_data"]["api_mst_ship"].AsArray())
        {
            int apiId = int.Parse(shipJson["api_id"].ToString());
            string nameJp = shipJson["api_name"].ToString();
            ShipClassModel? shipClass = null;

            ShipModel? vm = Ships.Find(sh => sh.ApiId == apiId);

            if (vm is null)
            {
                ShipModel model = new()
                {
                    ApiId = apiId,
                    NameJP = nameJp,
                };

                model.NameEN = Translation.TranslateName(nameJp);

                HttpResponseMessage response = await HttpClient.PostAsJsonAsync("Ships", model);

                response.EnsureSuccessStatusCode();

                ShipModel? postedModel = await response.Content.ReadFromJsonAsync<ShipModel>();

                if (postedModel is not null)
                {
                    Ships.Add(postedModel);
                }
            }
        }

        ReloadShipList();
    }

    public bool IsDebug =>
#if DEBUG
        true;
#else
        false;
#endif

    [RelayCommand(CanExecute = nameof(IsDebug))]
    public async Task ImportTranslationsFromFile()
    {
        string _path = "";

        await Translation.Initialize();

        string text = await File.ReadAllTextAsync(_path);
        JsonObject? data = JsonSerializer.Deserialize<JsonObject>(text);

        if (data is null) return;

        List<TranslationBaseModelRow> tls = await HttpClient.GetFromJsonAsync<List<TranslationBaseModelRow>>(TranslationKind.ShipsName.GetApiRoute()) ?? [];
        
        foreach (KeyValuePair<string, JsonNode> shipJson in data["ship"].AsObject())
        {
            string nameJp = shipJson.Key;
            string nameTl = shipJson.Value.GetValue<string>();

            TranslationBaseModelRow? model = tls.Find(tl => tl.TranslationJapanese == nameJp);

            if (model != null)
            {
                model.Translations.Add(new TranslationModel()
                {
                    Language = Language.Spanish,
                    Translation = nameTl,
                });

                HttpResponseMessage response = await HttpClient.PutAsJsonAsync("ShipNameTranslations", model);

                response.EnsureSuccessStatusCode();
            }
        }
    }
}
