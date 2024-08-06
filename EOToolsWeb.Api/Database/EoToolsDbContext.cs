using EOToolsWeb.Api.Models;
using EOToolsWeb.Api.Models.EquipmentUpgrades;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.Events;
using EOToolsWeb.Shared.ShipLocks;
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
        public DbSet<ShipNameTranslationModel> ShipTranslations { get; set; }
        public DbSet<ShipSuffixTranslationModel> ShipSuffixTranslations { get; set; }
        public DbSet<ShipClassModel> ShipClasses { get; set; }

        public DbSet<EquipmentModel> Equipments { get; set; }
        public DbSet<ShipLockModel> Locks { get; set; }
        public DbSet<ShipLockPhaseModel> LockPhases { get; set; }
        public DbSet<EquipmentUpgradeModel> EquipmentUpgrades { get; set; }
        public DbSet<EquipmentUpgradeImprovmentModel> Improvments { get; set; }

        public string DbPath => Path.Combine("Data", "EOTools.db");
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseSqlite($"Data Source={DbPath}");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<EquipmentUpgradeImprovmentModel>()
                .HasOne(e => e.ConversionData)
                .WithOne(e => e.ImprovmentModel)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasForeignKey(nameof(EquipmentUpgradeConversionModel), nameof(EquipmentUpgradeConversionModel.ImprovmentModelId));

            modelBuilder
                .Entity<EquipmentUpgradeImprovmentModel>()
                .HasMany(e => e.Helpers)
                .WithOne(e => e.Improvment)
                .OnDelete(DeleteBehavior.ClientCascade)
                .HasForeignKey(nameof(EquipmentUpgradeHelpersModel.EquipmentUpgradeImprovmentModelId));
        }
    }

}
