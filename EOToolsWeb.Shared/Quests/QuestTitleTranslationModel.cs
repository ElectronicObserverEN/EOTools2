using EOToolsWeb.Shared.Translations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EOToolsWeb.Shared.Quests;

public class QuestTitleTranslationModel : TranslationBaseModel
{
    [ForeignKey(nameof(QuestModel))]
    public int QuestId { get; set; }
}
