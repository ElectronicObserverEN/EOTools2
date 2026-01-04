using EOToolsWeb.Shared.Translations;

namespace EOToolsWeb.Models.Translations.DifferenceChecking;

public record TranslationDifferencesModel
{
    public required TranslationBaseModel Model { get; set; }

    public required string TextInDb { get; set; }

    public string TextInRepo { get; set; } = "";
}
