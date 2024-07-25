using System.Text.Json.Serialization;
using EOToolsWeb.Shared.ShipLocks;

namespace EOToolsWeb.Api.Models.ShipLocks;

public class ShipLockPhaseModelForDataRepo(ShipLockPhaseModel model)
{
    private ShipLockPhaseModel Model { get; } = model;

    public List<int> LockGroups => Model.LockGroups;

    [JsonPropertyName("Name")]
    public string PhaseName => Model.PhaseName;
}