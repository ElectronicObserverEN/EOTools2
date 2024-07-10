using EOToolsWeb.Api.Models;
using EOToolsWeb.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

public class ApiTokenHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock,
    UsersService users)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
{
    private UsersService Users => users;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
        {
            return AuthenticateResult.Fail("No authorization header was provided");
        }

        string autorization = Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(autorization)) return AuthenticateResult.Fail("Authentication denied");

        autorization = autorization.Replace("Basic ", "");

        UserConnection? connection = await Users.GetConnectionByToken(autorization);

        if (connection is null)
        {
            return AuthenticateResult.Fail("Authentification has failed");
        }

        Claim[] claims = { new(ClaimTypes.Role, "User") };
        ClaimsIdentity identity = new(claims, Scheme.Name);
        ClaimsPrincipal principal = new(identity);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}