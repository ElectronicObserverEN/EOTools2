using EOToolsWeb.Shared.Updates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.Seasons
{
    public class SeasonModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = "";

        [JsonPropertyName("added_update_id")]
        [ForeignKey(nameof(UpdateModel))]
        public int? AddedOnUpdateId { get; set; }

        [JsonPropertyName("removed_update_id")]
        [ForeignKey(nameof(UpdateModel))]
        public int? RemovedOnUpdateId { get; set; }
    }
}
