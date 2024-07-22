using EOToolsWeb.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ToolsController(ConfigurationService config) : ControllerBase
{
    private ConfigurationService ConfigurationService { get; } = config;

    [HttpGet("GetUpdate")]
    public async Task<IActionResult> GetUpdate(string userVersion)
    {
        string latestVersionString = await ConfigurationService.GetSoftwareVersion();

        if (latestVersionString != userVersion)
        {
            return File(System.IO.File.OpenRead(Path.Combine("Assets", "EOTools.zip")), "application/zip");
        }

        return Ok();
    }
}
