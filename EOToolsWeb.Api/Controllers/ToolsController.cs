using EOToolsWeb.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ToolsController(ConfigurationService config) : ControllerBase
{
    private ConfigurationService ConfigurationService { get; } = config;

    [HttpGet("GetVersion")]
    public async Task<IActionResult> GetVersion()
    {
        string latestVersionString = await ConfigurationService.GetSoftwareVersion();

        return Ok(latestVersionString);
    }

    [HttpGet("GetUpdate")]
    public IActionResult GetUpdate()
    {
        return File(System.IO.File.OpenRead(Path.Combine("Assets", "EOTools.zip")), "application/zip");
    }
}
