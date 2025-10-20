using EOToolsWeb.Shared.ApplicationLog;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.EquipmentUpgrades;
using EOToolsWeb.Shared.Events;
using EOToolsWeb.Shared.MapData;
using EOToolsWeb.Shared.Maps;
using EOToolsWeb.Shared.Quests;
using EOToolsWeb.Shared.Seasons;
using EOToolsWeb.Shared.Sessions;
using EOToolsWeb.Shared.ShipLocks;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Updates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EOToolsWeb.Api.Database
{
    // dotnet ef migrations add <name> --context EOToolsDbContext
    public class EoToolsDbContext : DbContext
    {
        public DbSet<EventModel> Events { get; set; }
        public DbSet<UpdateModel> Updates { get; set; }
        public DbSet<SeasonModel> Seasons { get; set; }

        public DbSet<QuestModel> Quests { get; set; }
        public DbSet<QuestTitleTranslationModel> QuestTitleTranslations { get; set; }
        public DbSet<QuestDescriptionTranslationModel> QuestDescriptionTranslations { get; set; }

        public DbSet<ShipModel> Ships { get; set; }
        public DbSet<ShipNameTranslationModel> ShipTranslations { get; set; }
        public DbSet<ShipSuffixTranslationModel> ShipSuffixTranslations { get; set; }
        public DbSet<ShipClassModel> ShipClasses { get; set; }

        public DbSet<MapNameTranslationModel> Maps { get; set; }
        public DbSet<FleetNameTranslationModel> Fleets { get; set; }

        public DbSet<EquipmentModel> Equipments { get; set; }
        public DbSet<EquipmentTranslationModel> EquipmentTranslations { get; set; }
        public DbSet<EquipmentUpgradeModel> EquipmentUpgrades { get; set; }
        public DbSet<EquipmentUpgradeImprovmentModel> Improvments { get; set; }

        public DbSet<ShipLockModel> Locks { get; set; }
        public DbSet<ShipLockPhaseModel> LockPhases { get; set; }
        
        public DbSet<NodeModel> Nodes { get; set; }

        private ICurrentSession Session { get; }
        private EoToolsUsersDbContext UserDb { get; }


        /* public EoToolsDbContext()
        {
        }
        */
        

        public EoToolsDbContext(ICurrentSession session, EoToolsUsersDbContext userDb)
        {
            Session = session;
            UserDb = userDb;
        }

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

        public async Task TrackAndSaveChanges()
        {
            foreach (EntityEntry entry in ChangeTracker.Entries().Where(t => t.Entity is not DataChangedLogModel && t.State is EntityState.Modified))
            {
                (Dictionary<string, string> before, Dictionary<string, string> after) = GetChanges(entry);

                string changes = string.Join("\n\r", before
                    .Zip(after)
                    .Select(data => $"{data.First.Key} : {data.First.Value} => {data.Second.Value}"));

                if (!string.IsNullOrEmpty(changes))
                {
                    DataChangedLogModel log = new()
                    {
                        User = Session.User ?? new(),
                        EntityId = entry.Property("Id").CurrentValue is int id ? id : 0,
                        EntityName = entry.Entity.GetType().Name,
                        Changes = changes,
                    };

                    await UserDb.DataChangeLogs.AddAsync(log);
                }
            }

            await SaveChangesAsync();
            await UserDb.SaveChangesAsync();
        }

        private (Dictionary<string, string> before, Dictionary<string, string> after) GetChanges(EntityEntry entry)
        {
            Dictionary<string, string> before = [];
            Dictionary<string, string> after = [];

            foreach (var property in entry.Properties.Where(p => !p.Metadata.IsForeignKey()))
            {
                object? valueBefore = property.OriginalValue;
                object? valueAfter = property.CurrentValue;

                if (valueBefore?.Equals(valueAfter) is not true)
                {
                    before.Add(property.Metadata.Name, valueBefore?.ToString() ?? "null");
                    after.Add(property.Metadata.Name, valueAfter?.ToString() ?? "null");
                }
            }

            return (before, after);
        }
    }

}
