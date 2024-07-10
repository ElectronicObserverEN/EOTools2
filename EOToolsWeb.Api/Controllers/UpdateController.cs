using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.Updates;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class UpdateController(EoToolsDbContext db) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;

    [HttpGet]
    public List<UpdateModel> GetUpdates()
    {
        return Database.Updates.ToList();
    }
}
