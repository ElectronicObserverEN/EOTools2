using EOToolsWeb.Shared.ShipLocks;

namespace EOToolsWeb.Models;

public class ShipLockPhaseEditRowModel : ShipLockPhaseModel
{
    public string ShipLocks => string.Join(",", LockGroups);
}
