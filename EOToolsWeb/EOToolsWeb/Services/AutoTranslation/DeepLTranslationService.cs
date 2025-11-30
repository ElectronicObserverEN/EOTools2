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
        return await TranslateText(text, languageSource, languageDestination, null);
    }

    public async Task<string> TranslateText(string text, Language languageSource, Language languageDestination, string? context)
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
                Context = context,
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
