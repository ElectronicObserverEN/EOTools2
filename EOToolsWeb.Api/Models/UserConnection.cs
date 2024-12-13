using EOToolsWeb.Shared.Users;

namespace EOToolsWeb.Api.Models
{
    public record UserConnection
    {
        public int Id { get; set; }

        public required UserModel User { get; set; }

        public required string Token { get; set; }
    }
}
