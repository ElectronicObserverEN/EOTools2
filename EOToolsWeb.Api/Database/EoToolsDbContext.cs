using EOToolsWeb.Api.Models;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.Events;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Updates;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Database
{
    // dotnet ef migrations add <name> --context EOToolsDbContext
    public class EoToolsDbContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }
        public DbSet<EventModel> Events { get; set; }
        public DbSet<UpdateModel> Updates { get; set; }
        public DbSet<ShipModel> Ships { get; set; }
        public DbSet<ShipClassModel> ShipClasses { get; set; }
        public DbSet<EquipmentModel> Equipments { get; set; }

        public string DbPath => Path.Combine("Data", "EOTools.db");
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");
    }

}
