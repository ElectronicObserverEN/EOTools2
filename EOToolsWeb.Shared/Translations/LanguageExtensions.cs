namespace EOToolsWeb.Shared.Translations;

public static class LanguageExtensions
{
    public static string GetCulture(this Language lang) => lang switch
    {
        Language.Japanese => "ja-JP",
        Language.English => "en-US",
        Language.Korean => "ko-KR",
        Language.SimplifiedChinese => "zh-CN",
        Language.Spanish => "es-ES",
        _ => "",
    };

    public static List<Language> OtherLanguagesTyped => [Language.Korean, Language.SimplifiedChinese, Language.Spanish];
    public static List<Language> AllLanguagesTyped => [.. OtherLanguagesTyped, Language.English];
}
