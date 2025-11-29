using EOToolsWeb.Shared.Translations;
using System.Threading.Tasks;

namespace EOToolsWeb.Services.AutoTranslation;

public interface IAutoTranslationService
{
    public Task<string> TranslateText(string text, Language languageSource, Language languageDestination);
}
