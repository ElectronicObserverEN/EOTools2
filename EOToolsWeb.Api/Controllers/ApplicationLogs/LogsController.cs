using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.ApplicationLog;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.ApplicationLogs;

[ApiController]
[Route("[controller]")]
public class LogsController(EoToolsDbContext db) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;

    [HttpGet("{entityName}/{entityId}")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<ActionResult> GetEntityLogs(string entityName, int entityId)
    {
        List<DataChangedLogModel> logs = await Database.DataChangeLogs
            .AsNoTracking()
            .Where(l => l.EntityId == entityId)
            .Where(l => l.EntityName == entityName)
            .Include(nameof(DataChangedLogModel.User))
            .ToListAsync();

        foreach (DataChangedLogModel log in logs)
        {
            log.User.Password = "";
        }

        return Ok(logs);
    }
}
