using System.ComponentModel.DataAnnotations.Schema;
using EOToolsWeb.Shared.Translations;

namespace EOToolsWeb.Shared.Equipments;

public class EquipmentTranslationModel : TranslationBaseModel
{
    [ForeignKey(nameof(EquipmentModel))]
    public int EquipmentId { get; set; }
}
