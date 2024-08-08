using EOToolsWeb.Shared.Translations;

namespace EOToolsWeb.Extensions.Translations;

public static class TranslationKindExtensions
{
    public static string GetApiRoute(this TranslationKind kind) => kind switch
    {
        TranslationKind.ShipsName => "ShipNameTranslations",
        TranslationKind.ShipsSuffixes => "ShipSuffixTranslations",
        TranslationKind.MapName => "MapNamesTranslations",
        TranslationKind.FleetName => "FleetNamesTranslations",
        _ => "",
    };

    public static string GetName(this TranslationKind kind) => kind switch
    {
        TranslationKind.ShipsName => "Ship names",
        TranslationKind.ShipsSuffixes => "Ship suffixes",
        TranslationKind.MapName => "Map names",
        TranslationKind.FleetName => "Fleet names",
        _ => "",
    };
}
