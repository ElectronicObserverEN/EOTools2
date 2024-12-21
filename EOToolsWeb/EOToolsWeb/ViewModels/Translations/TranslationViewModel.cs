using EOToolsWeb.Shared.Translations;
using System.Linq;

namespace EOToolsWeb.ViewModels.Translations;

public class TranslationViewModel : ViewModelBase
{
    public string Japanese { get; set; } = "";
    public string English { get; set; } = "";
    public string Korean { get; set; } = "";
    public string SimplifiedChinese { get; set; } = "";

    public TranslationBaseModel? Model { get; set; }

    public void LoadFromModel()
    {
        Japanese = Model?.Translations.FirstOrDefault(t => t.Language is Language.Japanese)?.Translation ?? "";
        English = Model?.Translations.FirstOrDefault(t => t.Language is Language.English)?.Translation ?? "";
        Korean = Model?.Translations.FirstOrDefault(t => t.Language is Language.Korean)?.Translation ?? "";
        SimplifiedChinese = Model?.Translations.FirstOrDefault(t => t.Language is Language.SimplifiedChinese)?.Translation ?? "";
    }

    public void SaveChanges()
    {
        if (Model is null) return;

        SaveTranslation(Japanese, Language.Japanese);
        SaveTranslation(English, Language.English);
        SaveTranslation(Korean, Language.Korean);
        SaveTranslation(SimplifiedChinese, Language.SimplifiedChinese);
    }

    private void SaveTranslation(string translation, Language language)
    {
        if (Model is null) return;

        TranslationModel? translationModel = Model.Translations.FirstOrDefault(t => t.Language == language);

        if (translationModel is null)
        {
            Model.Translations.Add(new()
            {
                Language = language,
                Translation = translation,
            });
        }
        else
        {
            translationModel.Translation = translation;
        }
    }
}