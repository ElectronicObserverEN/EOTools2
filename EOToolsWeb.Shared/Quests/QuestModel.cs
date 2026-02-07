using EOToolsWeb.Shared.Seasons;
using EOToolsWeb.Shared.Updates;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Shared.Quests
{
    [Index(nameof(Code), nameof(ApiId), IsUnique = true)]
    public class QuestModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("api_id")]
        public int ApiId { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; } = "";

        [JsonPropertyName("name_jp")]
        public string NameJP { get; set; } = "";

        [JsonPropertyName("name_en")]
        public string NameEN { get; set; } = "";

        [JsonPropertyName("desc_jp")]
        public string DescJP { get; set; } = "";

        [JsonPropertyName("desc_en")]
        public string DescEN { get; set; } = "";

        [JsonPropertyName("added_update_id")]
        [ForeignKey(nameof(UpdateModel))]
        public int? AddedOnUpdateId { get; set; }

        [JsonPropertyName("removed_update_id")]
        [ForeignKey(nameof(UpdateModel))]
        public int? RemovedOnUpdateId { get; set; }

        [JsonPropertyName("season_id")]
        [ForeignKey(nameof(SeasonModel))]
        public int? SeasonId { get; set; }

        [JsonPropertyName("tracker")]
        public string Tracker { get; set; } = "";

        [JsonPropertyName("progressResetType")]
        public QuestProgressResetType? QuestProgressResetType { get; set; }
    }
}
