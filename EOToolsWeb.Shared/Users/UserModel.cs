using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Shared.Users
{
    [Index(nameof(Username), IsUnique = true)]
    public record UserModel
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; } = "";

        public string Password { get; set; } = "";

        public UserKind Kind { get; set; } 
    }
}
