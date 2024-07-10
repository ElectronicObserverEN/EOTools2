using EOToolsWeb.Api.Models;
using EOToolsWeb.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController(UsersService users) : ControllerBase
{
    [HttpGet]
    [Authorize(AuthenticationSchemes = "ApiAuthentication")]
    public IActionResult GetToken()
    {
        string? autorization = HttpContext.Request.Headers.Authorization;

        if (string.IsNullOrEmpty(autorization)) return Unauthorized();
        
        return new LoginResponse(autorization);
    }

#if DEBUG
    [HttpGet("make")]
    public string MakePassword(string password)
    {
        return users.GetPasswordHashed(password);
    }
#endif
}
