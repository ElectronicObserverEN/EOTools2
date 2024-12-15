using EOToolsWeb.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController() : ControllerBase
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = "ApiAuthentication")]
    public IActionResult GetToken()
    {
        string? autorization = HttpContext.Request.Headers.Authorization;

        if (string.IsNullOrEmpty(autorization)) return Unauthorized();
        
        return new LoginResponse(autorization);
    }
}
