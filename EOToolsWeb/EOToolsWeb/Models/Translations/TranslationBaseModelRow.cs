using System.Linq;
using EOToolsWeb.Shared.Translations;

namespace EOToolsWeb.Models.Translations;

public class TranslationBaseModelRow : TranslationBaseModel
{
    public string TranslationsDisplay => (GetTranslation(Language.English) ?? GetTranslation(Language.Japanese))?.Translation ?? "";

    public TranslationModel? GetTranslation(Language lang) => Translations
        .FirstOrDefault(l => l.Language == lang && l.Translation != "");
}
