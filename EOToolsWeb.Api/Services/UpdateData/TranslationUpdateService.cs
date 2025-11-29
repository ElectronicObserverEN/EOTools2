using EOToolsWeb.Shared.Translations;

namespace EOToolsWeb.Api.Services.UpdateData;

public abstract class TranslationUpdateService
{
    protected List<string> OtherLanguages => LanguageExtensions.OtherLanguagesTyped.Select(l => l.GetCulture()).ToList();
    protected List<string> AllLanguages => LanguageExtensions.AllLanguagesTyped.Select(l => l.GetCulture()).ToList();
}
