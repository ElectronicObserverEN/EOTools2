using System.ComponentModel.DataAnnotations.Schema;

namespace EOToolsWeb.Shared.EquipmentUpgrades;

[Table("EquipmentUpgradeDataModel")]
public class EquipmentUpgradeModel
{
    public int Id { get; set; }

    public int EquipmentId { get; set; }

    /// <summary>
    /// Improvments possibles for this equipment
    /// </summary>
    public List<EquipmentUpgradeImprovmentModel> Improvement { get; set; } = [];
}