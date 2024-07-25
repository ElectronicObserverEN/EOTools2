using System.Text.Json.Serialization;
using EOToolsWeb.Shared.ShipLocks;

namespace EOToolsWeb.Api.Models.ShipLocks;

public class ShipLockModelForDataRepo(ShipLockModel model)
{
    private ShipLockModel Model { get; } = model;

    public int Id => Model.ApiId;

    [JsonPropertyName("A")]
    public byte ColorA => Model.ColorA;

    [JsonPropertyName("R")]
    public byte ColorR => Model.ColorR;

    [JsonPropertyName("G")]
    public byte ColorG => Model.ColorG;

    [JsonPropertyName("B")]
    public byte ColorB => Model.ColorB;

    [JsonPropertyName("Name")]
    public string NameJapanese => Model.NameJapanese;
}