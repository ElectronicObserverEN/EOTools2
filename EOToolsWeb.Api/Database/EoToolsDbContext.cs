using EOToolsWeb.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Database
{
    // dotnet ef migrations add <name> --context EOToolsDbContext
    public class EoToolsDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }

        private string DbPath => Path.Combine("Data", "EOTools.db");
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

}
