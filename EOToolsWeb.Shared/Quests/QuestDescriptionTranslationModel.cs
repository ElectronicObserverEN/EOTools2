using EOToolsWeb.Shared.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EOToolsWeb.Shared.Quests;

public class QuestDescriptionTranslationModel : TranslationBaseModel
{
    [ForeignKey(nameof(QuestModel))]
    public int QuestId { get; set; }
}
