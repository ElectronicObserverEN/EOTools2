namespace EOToolsWeb.Api.Models.ShipLocks
{
    public class ShipLocksPhasesModel
    {
        public List<ShipLockModelForDataRepo> Locks { get; set; } = [];

        public List<ShipLockPhaseModelForDataRepo> Phases { get; set; } = [];
    }
}
