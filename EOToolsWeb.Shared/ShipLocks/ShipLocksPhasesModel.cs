namespace EOToolsWeb.Shared.ShipLocks
{
    public class ShipLocksPhasesModel
    {
        public List<ShipLockModel> Locks { get; set; } = [];

        public List<ShipLockPhaseModel> Phases { get; set; } = [];
    }
}
