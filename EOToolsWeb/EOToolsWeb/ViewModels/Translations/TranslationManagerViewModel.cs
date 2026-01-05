using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Extensions.Translations;
using EOToolsWeb.Models.Translations;
using EOToolsWeb.Services.AutoTranslation;
using EOToolsWeb.Shared.Maps;
using EOToolsWeb.Shared.Sessions;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Translations;
using EOToolsWeb.ViewModels.Translations.DifferenceChecking;
using EOToolsWeb.Views.Translations;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Translations;

public partial class TranslationManagerViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial ObservableCollection<TranslationBaseModelRow> Translations { get; set; } = [];

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AutoTranslateCommand))]
    public partial TranslationKind SelectedTranslationKind { get; set; }

    [ObservableProperty]
    public partial Language SelectedLanguage { get; set; }

    public TranslationKind[] TranslationKinds { get; } = Enum.GetValues<TranslationKind>();
    public Language[] Languages { get; } = Enum.GetValues<Language>();

    public bool CanAutoTranslate => SelectedTranslationKind is TranslationKind.FleetName;

    [ObservableProperty]
    public partial TranslationBaseModelRow? SelectedRow { get; set; }

    private HttpClient HttpClient { get; }
    private TranslationViewModelOld TranslationViewModelOld { get; }

    public Interaction<TranslationViewModelOld, bool> ShowEditDialog { get; } = new();

    public ICurrentSession Session { get; private set; }

    private IAutoTranslationService AutoTranslationService { get; }

    private GithubTranslationFileProvider GithubTranslationFileProvider { get; }

    public TranslationManagerViewModel(HttpClient httpClient, TranslationViewModelOld viewModelOld, ICurrentSession session, IAutoTranslationService autoTranslation, GithubTranslationFileProvider githubProvider)
    {
        HttpClient = httpClient;
        TranslationViewModelOld = viewModelOld;
        Session = session;
        AutoTranslationService = autoTranslation;
        GithubTranslationFileProvider = githubProvider;

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
            await UpdateOneTranslationOnServer(model);
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

    public async Task UpdateTranslationOnServer(TranslationBaseModel vm)
    {
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync(SelectedTranslationKind.GetApiRoute(), vm);

        response.EnsureSuccessStatusCode();
    }

    public async Task UpdateOneTranslationOnServer(TranslationModel model)
    {
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"{SelectedTranslationKind.GetApiRoute()}/updateTranslation", model);

        response.EnsureSuccessStatusCode();
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

                await UpdateTranslationOnServer(vm);

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

    [RelayCommand(CanExecute = nameof(CanAutoTranslate))]
    private async Task AutoTranslate()
    {
        if (ShowDialogService is null) return;

        try
        {
            List<TranslationBaseModelRow> translations = Translations
                .Where(vm => vm.TranslationEnglish == vm.TranslationJapanese)
                .Reverse()
                .ToList();

            foreach (TranslationBaseModelRow vm in translations)
            {
                bool shouldContinue = await AutoTranslateOne(vm);

                if (!shouldContinue) return;
            }
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    private async Task<bool> AutoTranslateOne(TranslationBaseModelRow vm)
    {
        if (ShowDialogService is null) return false;

        string source = vm.TranslationJapanese;
        string destination = await AutoTranslationService.TranslateText(source, Language.Japanese, Language.English, "We are translating enemy fleet name for the game Kantai collection. The enemies are called Abyssals. Avoid using coma in the fleet name.");

        if (!await ShowDialogService.ShowConfirmPrompt("AI translation", $"Result : {destination}")) return false;

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

        await HttpClient.PutAsJsonAsync(SelectedTranslationKind.GetApiRoute(), vm);

        return true;
    }

    [RelayCommand]
    private async Task OpenDifferenceChecker()
    {
        try
        {
            GithubTranslationFileProvider.ShowDialogService = ShowDialogService;
            var differenceChecker = new DifferenceCheckingViewModel(this, GithubTranslationFileProvider);

            await differenceChecker.LoadDifferences();

            await ShowDialogService.ShowWindow(new DifferenceCheckingView()
            {
                DataContext = differenceChecker,
            });

            await LoadTranslations();
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }
}