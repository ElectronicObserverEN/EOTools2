namespace EOToolsWeb.Api.Models
{
    public record UserModel
    {
        public int Id { get; set; }

        public required string Username { get; set; }

        public required string Password { get; set; }
    }
}
