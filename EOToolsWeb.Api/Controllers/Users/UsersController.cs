using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Users;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class UsersController(EoToolsDbContext db) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;

    [HttpGet]
    public async Task<ActionResult> Get(int userId)
    {
        UserModel? user = await Database.Users.FirstOrDefaultAsync(user => user.Id == userId);

        if (user is null)
        {
            return NotFound();
        }

        user.Password = "";

        return Ok(user);
    }
}
