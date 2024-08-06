namespace EOToolsWeb.Shared.Translations;

public class TranslationModel
{
    public int Id { get; set; }

    public string Translation { get; set; } = "";

    public required Language Language { get; set; }
}