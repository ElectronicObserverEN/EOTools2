using EOToolsWeb.Shared.ShipLocks;

namespace EOToolsWeb.Models.ShipLocks;

public class ShipLockPhaseEditRowModel : ShipLockPhaseModel
{
    public string ShipLocks => string.Join(",", LockGroups);
}
