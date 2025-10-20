using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EOToolsWeb.Models.MapEditor.Deserialization.SpriteSheet;

public class SpriteSheetModel
{
    [JsonPropertyName("frames")]
    public Dictionary<string, FrameModel> Frames { get; set; } = [];
}