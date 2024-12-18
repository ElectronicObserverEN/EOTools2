using EOToolsWeb.Api.Models;
using EOToolsWeb.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using EOToolsWeb.Shared.Sessions;
using EOToolsWeb.Shared.Users;

public class ApiTokenHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    ISystemClock clock,
    UsersService users,
    ICurrentSession session)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder, clock)
{
    private UsersService Users => users;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("X-TOKEN-EO-TOOLS-WEB-X"))
        {
            return AuthenticateResult.Fail("No authorization header was provided");
        }

        string autorization = Request.Headers["X-TOKEN-EO-TOOLS-WEB-X"].ToString();

        if (string.IsNullOrEmpty(autorization)) return AuthenticateResult.Fail("Authentication denied");
        
        UserConnection? connection = await Users.GetConnectionByToken(autorization);

        if (connection is null)
        {
            return AuthenticateResult.Fail("Authentification has failed");
        }

        session.User = connection.User;

        Claim[] claims = [new(ClaimTypes.Role, connection.User.Kind.ToString())];
        ClaimsIdentity identity = new(claims, Scheme.Name);
        ClaimsPrincipal principal = new(identity);
        AuthenticationTicket ticket = new(principal, Scheme.Name);

        return AuthenticateResult.Success(ticket);
    }
}