using EOToolsWeb.Shared.Translations;

namespace EOToolsWeb.Api.Services.UpdateData;

public abstract class TranslationUpdateService
{
    protected List<string> OtherLanguages => OtherLanguagesTyped.Select(l => l.GetCulture()).ToList();
    protected List<string> AllLanguages => AllLanguagesTyped.Select(l => l.GetCulture()).ToList();

    protected List<Language> OtherLanguagesTyped { get; } = [Language.Korean, Language.SimplifiedChinese, Language.Spanish];
    protected List<Language> AllLanguagesTyped => [.. OtherLanguagesTyped, Language.English];
}
