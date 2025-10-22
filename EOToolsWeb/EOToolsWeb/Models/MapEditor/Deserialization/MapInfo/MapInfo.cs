using System.Text.Json.Serialization; 
using System.Collections.Generic; 
namespace EOToolsWeb.Models.MapEditor.Deserialization.MapInfo{ 

    public class MapInfo
    {
        [JsonPropertyName("labels")] 
        public List<Label> Labels { get; set; } = [];
        
        [JsonPropertyName("bg")]
        public List<object> Bg { get; set; }= [];

        [JsonPropertyName("spots")]
        public List<Spot> Spots { get; set; }= [];

        [JsonPropertyName("airbases")]
        public List<AirBase> AirBases { get; set; }= [];

        [JsonPropertyName("airraids")]
        public List<AirRaid> AirRaids { get; set; }= [];

        [JsonPropertyName("enemies")]
        public List<Enemy> Enemies { get; set; }= [];
    }

}