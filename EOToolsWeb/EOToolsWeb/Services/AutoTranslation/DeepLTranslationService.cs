using DeepL;
using EOToolsWeb.Shared.Translations;
using EOToolsWeb.ViewModels.Settings;
using System;
using System.Threading.Tasks;

namespace EOToolsWeb.Services.AutoTranslation;

public class DeepLTranslationService(SettingsViewModel settings) : IAutoTranslationService
{
    private SettingsViewModel SettingsViewModel { get; } = settings;

    public async Task<string> TranslateText(string text, Language languageSource, Language languageDestination)
    {
        var authKey = SettingsViewModel.DeepLApiKey;
        var client = new DeepLClient(authKey);

        var translatedText = await client.TranslateTextAsync(
            text,
            MapLanguage(languageSource),
            MapLanguage(languageDestination),
            options: new TextTranslateOptions()
            {
                GlossaryId = "6c2a3a23-b619-4211-b331-afca1272bb36",
                Context = "We are translating enemy fleet name for the game Kantai collection. The enemies are called Abyssals. Avoid using coma in the fleet name."
            });

        return translatedText.Text;
    }

    private static string MapLanguage(Language language)
    {
        return language switch
        {
            Language.English => LanguageCode.EnglishBritish,
            Language.SimplifiedChinese => LanguageCode.Chinese,
            Language.Korean => LanguageCode.Korean,
            Language.Spanish => LanguageCode.Spanish,
            Language.Japanese => LanguageCode.Japanese,
            _ => throw new NotSupportedException($"The language {language} is not supported."),
        };
    }
}
