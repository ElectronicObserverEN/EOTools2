using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Extensions.Translations;
using EOToolsWeb.Shared.Translations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Models.Translations;
using EOToolsWeb.Shared.Maps;
using EOToolsWeb.Shared.Ships;
using ReactiveUI;

namespace EOToolsWeb.ViewModels.Translations;

public partial class TranslationManagerViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<TranslationBaseModelRow> _translations = [];

    [ObservableProperty] 
    private TranslationKind _selectedTranslationKind;

    public TranslationKind[] TranslationKinds { get; } = Enum.GetValues<TranslationKind>();

    private HttpClient HttpClient { get; }
    private TranslationViewModel TranslationViewModel { get; }

    public Interaction<TranslationViewModel, bool> ShowEditDialog { get; } = new();

    public TranslationManagerViewModel(HttpClient httpClient, TranslationViewModel viewModel)
    {
        HttpClient = httpClient;
        TranslationViewModel = viewModel;

        PropertyChanged += AfterTranslationKindChanged;
    }

    private async void AfterTranslationKindChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedTranslationKind)) return;

        await LoadTranslations();
    }

    public async Task LoadTranslations()
    {
        Translations = await LoadTranslations(SelectedTranslationKind);
    }

    public async Task<List<TranslationBaseModelRow>> LoadTranslations(TranslationKind kind)
    {
        return await HttpClient.GetFromJsonAsync<List<TranslationBaseModelRow>>(kind.GetApiRoute()) ?? [];
    }

    public TranslationBaseModel CreateModel() => SelectedTranslationKind switch
    {
        TranslationKind.ShipsName => new ShipNameTranslationModel(),
        TranslationKind.ShipsSuffixes => new ShipSuffixTranslationModel(),
        TranslationKind.MapName => new MapNameTranslationModel(),
        TranslationKind.FleetName => new FleetNameTranslationModel(),
        _ => throw new NotSupportedException(),
    };

    [RelayCommand]
    private async Task AddTranslation()
    {
        TranslationBaseModel model = CreateModel();

        TranslationViewModel.Model = model;
        TranslationViewModel.LoadFromModel();

        if (await ShowEditDialog.Handle(TranslationViewModel))
        {
            TranslationViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync(SelectedTranslationKind.GetApiRoute(), model);

            response.EnsureSuccessStatusCode();

            TranslationBaseModelRow? postedModel = await response.Content.ReadFromJsonAsync<TranslationBaseModelRow>();

            if (postedModel is not null)
            {
                Translations.Add(postedModel);

                OnPropertyChanged(nameof(Translations));
            }
        }
    }

    [RelayCommand]
    private async Task EditTranslation(TranslationBaseModelRow vm)
    {
        TranslationViewModel.Model = vm;
        TranslationViewModel.LoadFromModel();

        if (await ShowEditDialog.Handle(TranslationViewModel))
        {
            TranslationViewModel.SaveChanges();

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync(SelectedTranslationKind.GetApiRoute(), vm);

            response.EnsureSuccessStatusCode();

            OnPropertyChanged(nameof(Translations));
        }
    }

    [RelayCommand]
    private async Task DeleteTranslation(TranslationBaseModelRow vm)
    {
        HttpResponseMessage response = await HttpClient.DeleteAsync($"{SelectedTranslationKind.GetApiRoute()}/{vm.Id}");
        
        // TODO : server returns error 500 cause i need to delete translationModel before the parent in the controller
        response.EnsureSuccessStatusCode();

        Translations.Remove(vm);

        OnPropertyChanged(nameof(Translations));
    }

    [RelayCommand]
    private async Task PushTranslations()
    {
        await HttpClient.PutAsync($"{SelectedTranslationKind.GetApiRoute()}/pushTranslations", null);
    }
}