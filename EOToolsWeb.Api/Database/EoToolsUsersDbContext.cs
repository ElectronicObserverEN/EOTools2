using EOToolsWeb.Shared.ApplicationLog;
using EOToolsWeb.Shared.Users;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Database
{
    // dotnet ef migrations add <name> --context EoToolsUsersDbContext
    public class EoToolsUsersDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }

        public DbSet<DataChangedLogModel> DataChangeLogs { get; set; }

        public string DbPath => Path.Combine("Data", "EOToolsUsers.db");

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

}
