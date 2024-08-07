using EOToolsWeb.Api.Services.UpdateData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers.FitBonus;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class FitBonusController(FitBonusUpdaterService dataUpdateService) : ControllerBase
{
    private FitBonusUpdaterService DataUpdateService { get; } = dataUpdateService;

    [HttpPut("updateFits")]
    public async Task<IActionResult> UpdateData()
    {
        await DataUpdateService.UpdateThenSaveFileThenPush();

        return Ok();
    }
}
