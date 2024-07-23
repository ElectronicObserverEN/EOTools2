using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.Equipments;

public class EquipmentModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("api_id")]
    public int ApiId { get; set; }

    [JsonPropertyName("name_jp")]
    public string NameJP { get; set; } = "";

    [JsonPropertyName("name_en")]
    public string NameEN { get; set; } = "";

    [JsonPropertyName("development")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool CanBeCrafted { get; set; }
}
