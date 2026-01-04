using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Models.Translations.DifferenceChecking;
using EOToolsWeb.Shared.Translations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Translations.DifferenceChecking;

public partial class DifferenceCheckingViewModel : ViewModelBase
{
    private TranslationManagerViewModel TranslationManager { get; set; }
    private GithubTranslationFileProvider GithubTranslationFileProvider { get; set; }

    public DifferenceCheckingViewModel(TranslationManagerViewModel manager, GithubTranslationFileProvider provider)
    {
        TranslationManager = manager;
        GithubTranslationFileProvider = provider;
    }

    [ObservableProperty]
    public partial List<TranslationDifferencesModel> Differences { get; set; } = new();

    public async Task LoadDifferences()
    {
        List<TranslationDifferencesModel> newList = [];

        // Load the Json depending on the tl kind : 
        Dictionary<string, string> translations = await GithubTranslationFileProvider.GetTranslations(TranslationManager.SelectedTranslationKind, TranslationManager.SelectedLanguage);

        // NOTE : this doesn't check for translations that are on github but not in db
        foreach (var translation in TranslationManager.Translations)
        {
            string key = translation.TranslationJapanese;
            TranslationModel? dbTranslation = translation.GetTranslation(TranslationManager.SelectedLanguage);

            translations.TryGetValue(key, out string? githubTranslation);

            if (dbTranslation is { } && githubTranslation is { })
            {
                if (dbTranslation.Translation != githubTranslation)
                {
                    newList.Add(new TranslationDifferencesModel
                    {
                        Model = translation,
                        TextInDb = dbTranslation.Translation,
                        TextInRepo = githubTranslation
                    });
                }
            }
        }

        Differences = newList;
    }

    [RelayCommand]
    private async Task UpdateDatabaseWithGithubData()
    {
        try
        {
            foreach (var difference in Differences)
            {
                TranslationModel? dbTranslation = difference.Model.GetTranslation(TranslationManager.SelectedLanguage);

                if (dbTranslation is { })
                {
                    dbTranslation.Translation = difference.TextInRepo;
                }
                else
                {
                    // Create new translation
                    difference.Model.Translations.Add(new TranslationModel
                    {
                        Language = TranslationManager.SelectedLanguage,
                        Translation = difference.TextInRepo,
                    });
                }

                await TranslationManager.UpdateTranslationOnServer(difference.Model);
            }
        }
        finally
        {
            await LoadDifferences();
        }
    }
}
