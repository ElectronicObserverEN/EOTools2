namespace EOToolsWeb.Shared.Translations;

public abstract class TranslationBaseModel
{
    public int Id { get; set; }

    public List<TranslationModel> Translations { get; set; } = [];
}