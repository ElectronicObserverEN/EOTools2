using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Models.UpdateData;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Shared;
using EOToolsWeb.Shared.Events;
using EOToolsWeb.Shared.Updates;
using System.Text.Json;

namespace EOToolsWeb.Api.Services.UpdateData;

public class UpdateMaintenanceDataService(IGitManagerService git, EoToolsDbContext db, DatabaseSyncService dbSync, JsonSerializerOptions options)
{
    private IGitManagerService GitManager => git;
    private DatabaseSyncService DatabaseSync => dbSync;
    private EoToolsDbContext Database => db;
    private JsonSerializerOptions SerializationOptions => options;

    private string UpdateFilePath => Path.Combine(GitManager.FolderPath, "update.json");

    public async Task UpdateMaintenanceData()
    {
        await GitManager.Pull();

        UpdateFileModel? updateData = JsonSerializer.Deserialize<UpdateFileModel>(await File.ReadAllTextAsync(UpdateFilePath));

        if (updateData is null) return;

        UpdateModel? update = SetUpdateTime(updateData);
        SetOldUpdateTime(updateData);

        await DatabaseSync.StageDatabaseChangesToGit();

        await File.WriteAllTextAsync(UpdateFilePath, JsonSerializer.Serialize(updateData, SerializationOptions));

        string commitMessage = update switch
        {
            not null => $"Maintenance information - {update.Name}",
            _ => "Clear maintenance information",
        };

        await GitManager.Push(commitMessage);
    }

    private UpdateModel? SetUpdateTime(UpdateFileModel updateData)
    {
        // get last update : 
        (UpdateModel? update, int updState) = GetMaintenanceState();

        if (update is { UpdateDate: { } updateDate, UpdateStartTime: { } updateStartTime })
        {
            updateData.MaintStart = $"{updateDate.Date.Add(updateStartTime):yyyy/MM/dd HH:mm:ss}";
            updateData.MaintEnd = update.UpdateEndTime switch
            {
                { } endTime => $"{updateDate.Date.Add(endTime):yyyy/MM/dd HH:mm:ss}",
                _ => null,
            };

            updateData.MaintInfoLink = string.IsNullOrEmpty(update.EndTweetLink) switch
            {
                true => string.IsNullOrEmpty(update.StartTweetLink) switch
                {
                    true => "https://twitter.com/KanColle_STAFF",
                    false => update.StartTweetLink,
                },
                false => update.EndTweetLink,
            };
        }

        updateData.MaintEventState = updState;

        return update;
    }

    private void SetOldUpdateTime(UpdateFileModel updateData)
    {
        // get last update : 
        (UpdateModel update, int updState) = GetOldMaintenanceState();

        if (update is { UpdateDate: { } updateDate, UpdateStartTime: { } updateTime })
        {
            updateData.kancolle_mt = $"{updateDate.Date.Add(updateTime):yyyy/MM/dd HH:mm:ss}"; // For backward compatibility
        }

        updateData.event_state = updState;
    }

    private (UpdateModel?, int) GetMaintenanceState()
    {
        // get last update : 
        UpdateModel? update = Database.Updates
            .AsEnumerable()
            .Where(upd => !upd.WasLiveUpdate)
            .Where(upd => upd.UpdateDate is not null)
            .Where(upd => upd.UpdateStartTime is not null)
            .Where(upd => upd.UpdateInProgress() || upd.UpdateIsComing())
            .MinBy(upd => upd.UpdateDate);

        if (update is null)
        {
            return (null, (int)MaintenanceState.None);
        }

        EventModel? eventEnd = db.Events
            .AsEnumerable()
            .FirstOrDefault(ev => ev.EndOnUpdateId == update.Id);

        if (eventEnd != null)
        {
            return new(update, (int)MaintenanceState.EventEnd);
        }

        EventModel? eventStart = db.Events
            .AsEnumerable()
            .FirstOrDefault(ev => ev.StartOnUpdateId == update.Id);

        if (eventStart != null)
        {
            return new(update, (int)MaintenanceState.EventStart);
        }

        return new(update, (int)MaintenanceState.Regular);
    }

    private (UpdateModel?, int) GetOldMaintenanceState()
    {
        // get last update : 
        UpdateModel? update = Database.Updates
            .AsEnumerable()
            .Where(upd => !upd.WasLiveUpdate)
            .Where(upd => upd.UpdateDate is not null)
            .Where(upd => upd.UpdateStartTime is not null)
            .Where(upd => upd.UpdateInProgress() || upd.UpdateIsComing())
            .MinBy(upd => upd.UpdateDate);

        if (update is null)
        {
            return (null, (int)MaintenanceState.None);
        }

        EventModel? eventEnd = db.Events
            .AsEnumerable()
            .FirstOrDefault(ev => ev.EndOnUpdateId == update.Id);

        if (eventEnd != null)
        {
            return new(update, (int)MaintenanceState.EventEnd);
        }

        EventModel? eventStart = db.Events
            .AsEnumerable()
            .FirstOrDefault(ev => ev.StartOnUpdateId == update.Id);

        if (eventStart != null && update.UpdateInProgress() && update.UpdateEndTime is TimeSpan end)
        {
            return new(new()
            {
                Id = update.Id,
                Name = update.Name,
                Description = update.Description,
                WasLiveUpdate = update.WasLiveUpdate,
                EndTweetLink = update.EndTweetLink,
                StartTweetLink = update.StartTweetLink,
                UpdateDate = update.UpdateDate,
                UpdateEndTime = end,
                UpdateStartTime = end,
            }, (int)MaintenanceState.EventStart);
        }

        return new(update, (int)MaintenanceState.Regular);
    }
}
