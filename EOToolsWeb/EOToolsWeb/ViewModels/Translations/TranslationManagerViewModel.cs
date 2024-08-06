using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Extensions.Translations;
using EOToolsWeb.Shared.Translations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Models.Translations;

namespace EOToolsWeb.ViewModels.Translations;

public partial class TranslationManagerViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<TranslationBaseModelRow> _translations = [];

    [ObservableProperty] 
    private TranslationKind _selectedTranslationKind;

    public TranslationKind[] TranslationKinds { get; } = Enum.GetValues<TranslationKind>();

    private HttpClient HttpClient { get; }

    public TranslationManagerViewModel(HttpClient httpClient)
    {
        HttpClient = httpClient;

        PropertyChanged += AfterTranslationKindChanged;
    }

    private async void AfterTranslationKindChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedTranslationKind)) return;

        await LoadTranslations();
    }

    public async Task LoadTranslations()
    {
        Translations = await HttpClient.GetFromJsonAsync<List<TranslationBaseModelRow>>(SelectedTranslationKind.GetApiRoute()) ?? [];
    }

    [RelayCommand]
    private async Task AddTranslation()
    {

    }

    [RelayCommand]
    private async Task EditTranslation()
    {

    }

    [RelayCommand]
    private async Task DeleteTranslation()
    {

    }
}