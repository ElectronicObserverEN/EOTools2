namespace EOToolsWeb.Shared.Translations;

public abstract class TranslationBaseModel
{
    public int Id { get; set; }

    public List<TranslationModel> Translations { get; set; } = [];

    public TranslationModel? GetTranslation(Language lang) => Translations
        .FirstOrDefault(l => l.Language == lang && l.Translation != "");
}