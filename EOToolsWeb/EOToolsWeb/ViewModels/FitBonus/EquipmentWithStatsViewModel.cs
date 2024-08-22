using System.Linq;
using EOToolsWeb.Models.FitBonus;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.ViewModels.Equipments;

namespace EOToolsWeb.ViewModels.FitBonus;

public class EquipmentWithStatsViewModel
{
    private EquipmentWithStatsModel Model { get; }

    public int Level { get; set; }

    public EquipmentModel Equipment { get; set; } = new();

    private EquipmentManagerViewModel EquipmentManagerViewModel { get; }

    public EquipmentWithStatsViewModel(EquipmentWithStatsModel model, EquipmentManagerViewModel db)
    {
        Model = model;
        EquipmentManagerViewModel = db;

        LoadFromModel();
    }

    private void LoadFromModel()
    {
        Level = Model.Level;
        Equipment = EquipmentManagerViewModel.AllEquipments.FirstOrDefault(eqDb => eqDb.ApiId == Model.EquipmentId) ?? new();
    }
}