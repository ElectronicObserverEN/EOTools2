using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.FitBonus;

public class FitBonusValueModel
{
    [JsonPropertyName("houg")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Firepower { get; set; }

    [JsonPropertyName("raig")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Torpedo { get; set; }

    [JsonPropertyName("tyku")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? AntiAir { get; set; }

    [JsonPropertyName("souk")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Armor { get; set; }

    [JsonPropertyName("kaih")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Evasion { get; set; }

    [JsonPropertyName("tais")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? ASW { get; set; }

    [JsonPropertyName("saku")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? LOS { get; set; }

    [JsonPropertyName("baku")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Bombing { get; set; }

    /// <summary>
    /// Visible acc fit actually doesn't work according to some studies
    /// </summary>
    [JsonPropertyName("houm")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Accuracy { get; set; }

    [JsonPropertyName("leng")]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Range { get; set; }
}