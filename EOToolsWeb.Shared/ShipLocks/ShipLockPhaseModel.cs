using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Shared.Events;

namespace EOToolsWeb.Shared.ShipLocks
{
    public class ShipLockPhaseModel : ObservableObject
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonIgnore]
        public int SortId { get; set; }

        [ForeignKey(nameof(EventModel))]
        public int EventId { get; set; } = new();

        public List<int> LockGroups { get; set; } = [];

        [JsonPropertyName("Name")]
        public string PhaseName { get; set; } = "";
    }
}
