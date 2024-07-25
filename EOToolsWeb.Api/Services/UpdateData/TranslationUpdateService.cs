namespace EOToolsWeb.Api.Services.UpdateData;

public abstract class TranslationUpdateService
{
    protected List<string> OtherLanguages { get; } = ["ko-KR"];
    protected List<string> AllLanguages => [.. OtherLanguages, "en-US"];
}
