using System.Linq;
using EOToolsWeb.Shared.Translations;
using EOToolsWeb.ViewModels.Translations;

namespace EOToolsWeb.Models.Translations;

public class TranslationBaseModelRow : TranslationBaseModel
{
    public string TranslationJapanese => GetTranslation(Language.Japanese)?.Translation ?? "";
    public string TranslationEnglish => GetTranslation(Language.English)?.Translation ?? "";

    public TranslationViewModel TranslationDestination { get; set; } = new();
}
