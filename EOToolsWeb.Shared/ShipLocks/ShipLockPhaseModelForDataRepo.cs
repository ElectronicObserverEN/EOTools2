using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.ShipLocks;

public class ShipLockPhaseModelForDataRepo(ShipLockPhaseModel model)
{
    private ShipLockPhaseModel Model { get; } = model;

    public List<int> LockGroups => Model.LockGroups;

    [JsonPropertyName("Name")]
    public string PhaseName => Model.PhaseName;
}