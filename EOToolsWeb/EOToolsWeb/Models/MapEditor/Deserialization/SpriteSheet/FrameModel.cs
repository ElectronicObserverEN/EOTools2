using System.Text.Json.Serialization;

namespace EOToolsWeb.Models.MapEditor.Deserialization.SpriteSheet;

public class FrameModel
{
    [JsonPropertyName("frame")]
    public required FrameDefinitionModel FrameDefinitionModel { get; set; }

    [JsonPropertyName("rotated")]
    public bool IsRotated { get; set; } 
    
    [JsonPropertyName("trimmed")]
    public bool IsTrimmed  { get; set; }
    
    [JsonPropertyName("spriteSourceSize")]
    public required SpriteSourceSizeModel SpriteSourceSizeModel { get; set; }
    
    [JsonPropertyName("sourceSize")]
    public required SourceSizeModel  SourceSizeModel { get; set; }
}