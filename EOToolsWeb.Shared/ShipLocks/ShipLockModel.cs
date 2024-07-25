using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using EOToolsWeb.Shared.Events;

namespace EOToolsWeb.Shared.ShipLocks
{
    public class ShipLockModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonPropertyName("Id")]
        public int SortId { get; set; }

        [ForeignKey(nameof(EventModel))]
        public int EventId { get; set; } = new();
        
        [JsonPropertyName("A")]
        public byte ColorA { get; set; }

        [JsonPropertyName("R")]
        public byte ColorR { get; set; }

        [JsonPropertyName("G")]
        public byte ColorG { get; set; }

        [JsonPropertyName("B")]
        public byte ColorB { get; set; }

        [JsonPropertyName("Name")]
        public string NameJapanese { get; set; } = "";

        [JsonIgnore]
        public string NameEnglish { get; set; } = "";
    }
}
