using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Extensions.Translations;
using EOToolsWeb.Shared.Translations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Models.Translations;
using EOToolsWeb.Shared.Maps;
using EOToolsWeb.Shared.Sessions;
using EOToolsWeb.Shared.Ships;
using ReactiveUI;
using EOToolsWeb.Services.AutoTranslation;
using EOToolsWeb.Services;

namespace EOToolsWeb.ViewModels.Translations;

public partial class TranslationManagerViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial ObservableCollection<TranslationBaseModelRow> Translations { get; set; } = [];

    [ObservableProperty]
    public partial TranslationKind SelectedTranslationKind { get; set; }

    [ObservableProperty] 
    public partial Language SelectedLanguage { get; set; }

    public TranslationKind[] TranslationKinds { get; } = Enum.GetValues<TranslationKind>();
    public Language[] Languages { get; } = Enum.GetValues<Language>();

    [ObservableProperty]
    public partial TranslationBaseModelRow? SelectedRow { get; set; }

    private HttpClient HttpClient { get; }
    private TranslationViewModelOld TranslationViewModelOld { get; }

    public Interaction<TranslationViewModelOld, bool> ShowEditDialog { get; } = new();

    public ICurrentSession Session { get; private set; }

    private IAutoTranslationService AutoTranslationService { get; }

    public TranslationManagerViewModel(HttpClient httpClient, TranslationViewModelOld viewModelOld, ICurrentSession session, IAutoTranslationService autoTranslation)
    {
        HttpClient = httpClient;
        TranslationViewModelOld = viewModelOld;
        Session = session;
        AutoTranslationService = autoTranslation;

        PropertyChanged += AfterTranslationKindChanged;
        PropertyChanged += AfterLanguageChanged;

        PropertyChanging += OnRowChanging;
        PropertyChanged += OnRowChanged;
    }

    private void OnRowChanging(object? sender, PropertyChangingEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedRow)) return;
        if (SelectedRow is null) return;

        SelectedRow.TranslationDestination.PropertyChanged -= OnTranslationChanged;
    }

    private void OnRowChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedRow)) return;
        if (SelectedRow is null) return;

        SelectedRow.TranslationDestination.PropertyChanged += OnTranslationChanged;
    }

    private void OnTranslationChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedRow.TranslationDestination.Translation)) return;
        if (SelectedRow is null) return;

        TranslationModel model = new()
        {
            Id = SelectedRow.Id,
            Language = SelectedLanguage,
            Translation = SelectedRow.TranslationDestination.Translation,
        };

        Task.Run<Task>(async () =>
        {
            HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{SelectedTranslationKind.GetApiRoute()}/updateTranslation", model);

            response.EnsureSuccessStatusCode();
        });
    }

    private async void AfterLanguageChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedLanguage)) return;

        await LoadTranslations();
    }

    private async void AfterTranslationKindChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedTranslationKind)) return;

        await LoadTranslations();
    }

    public async Task LoadTranslations()
    {
        Translations = new(await LoadTranslations(SelectedTranslationKind));
    }

    public async Task<List<TranslationBaseModelRow>> LoadTranslations(TranslationKind kind)
    {
        List<TranslationBaseModelRow> tls = await HttpClient.GetFromJsonAsync<List<TranslationBaseModelRow>>(kind.GetApiRoute()) ?? [];

        foreach (TranslationBaseModelRow tl in tls)
        {
            tl.TranslationDestination.Translation = tl.GetTranslation(SelectedLanguage)?.Translation ?? "";
        }

        return tls;
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
        try
        {
            TranslationBaseModel model = CreateModel();

            TranslationViewModelOld.Model = model;
            TranslationViewModelOld.LoadFromModel();

            if (await ShowEditDialog.Handle(TranslationViewModelOld))
            {
                TranslationViewModelOld.SaveChanges();

                HttpResponseMessage response = await HttpClient.PostAsJsonAsync(SelectedTranslationKind.GetApiRoute(), model);

                response.EnsureSuccessStatusCode();

                TranslationBaseModelRow? postedModel = await response.Content.ReadFromJsonAsync<TranslationBaseModelRow>();

                if (postedModel is not null)
                {
                    Translations.Add(postedModel);
                }
            }
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    private async Task EditTranslation(TranslationBaseModelRow vm)
    {
        try
        {
            TranslationViewModelOld.Model = vm;
            TranslationViewModelOld.LoadFromModel();

            if (await ShowEditDialog.Handle(TranslationViewModelOld))
            {
                TranslationViewModelOld.SaveChanges();

                HttpResponseMessage response = await HttpClient.PutAsJsonAsync(SelectedTranslationKind.GetApiRoute(), vm);

                response.EnsureSuccessStatusCode();

                OnPropertyChanged(nameof(Translations));
            }
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    private async Task DeleteTranslation(TranslationBaseModelRow vm)
    {
        try
        {
            HttpResponseMessage response = await HttpClient.DeleteAsync($"{SelectedTranslationKind.GetApiRoute()}/{vm.Id}");

            response.EnsureSuccessStatusCode();

            Translations.Remove(vm);
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
            await HttpClient.PutAsync($"{SelectedTranslationKind.GetApiRoute()}/pushTranslations", null);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    private async Task AutoTranslate(TranslationBaseModelRow vm)
    {
        if (ShowDialogService is null) return;

        try
        {
            string source = vm.TranslationJapanese;
            string destination = await AutoTranslationService.TranslateText(source, Language.Japanese, Language.English);

            if (!await ShowDialogService.ShowConfirmPrompt("AI translation", $"Result : {destination}")) return;

            foreach (Language lang in LanguageExtensions.AllLanguagesTyped)
            {
                if (vm.GetTranslation(lang) is { } translation)
                { 
                    translation.Translation = destination;
                    translation.IsPendingChange = true;
                }
                else
                {
                    vm.Translations.Add(new TranslationModel
                    {
                        Language = lang,
                        Translation = destination,
                        IsPendingChange = true,
                    });
                }
            }

            HttpResponseMessage response = await HttpClient.PutAsJsonAsync(SelectedTranslationKind.GetApiRoute(), vm);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }
}