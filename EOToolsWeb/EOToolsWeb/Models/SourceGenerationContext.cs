using System.Text.Json.Serialization;

namespace EOToolsWeb.Models;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(ConfigModel))]
public partial class SourceGenerationContext : JsonSerializerContext
{
}
