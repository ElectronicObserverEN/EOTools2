using System.Text.Json.Serialization;

namespace EOToolsWeb.Shared.Users
{
    public record UserModel
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string Username { get; set; } = "";

        [JsonIgnore]
        public string Password { get; set; } = "";
    }
}
