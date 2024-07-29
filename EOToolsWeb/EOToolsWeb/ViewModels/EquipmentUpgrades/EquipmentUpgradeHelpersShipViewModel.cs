using System.Linq;
using System.Threading.Tasks;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.ViewModels.Ships;

namespace EOToolsWeb.ViewModels.EquipmentUpgrades
{
    public class EquipmentUpgradeHelpersShipViewModel(ShipManagerViewModel shipManager)
    {
        public EquipmentUpgradeHelpersShipModel Model { get; set; } = new();

        public ShipModel ShipModel { get; set; } = new();

        private ShipManagerViewModel ShipManager { get; } = shipManager;

        public async Task LoadFromModel()
        {
            await ShipManager.LoadAllShips();

            ShipModel = ShipManager.Ships.FirstOrDefault(ship => ship.ApiId == Model.ShipId) ?? new();
        }
    }
}
