using EOToolsWeb.Api.Models;
using EOToolsWeb.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

public class ApiLoginHandler(
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
        autorization = Encoding.UTF8.GetString(Convert.FromBase64String(autorization));

        string[] auth = autorization.Split(':');
        string login = auth[0];
        string pass = auth[1];

        try
        {
            UserConnection connection = await Users.Login(login, pass);

            Request.Headers.Authorization = connection.Token;

            Claim[] claims = { new(ClaimTypes.Role, "User") };
            ClaimsIdentity identity = new(claims, Scheme.Name);
            ClaimsPrincipal principal = new(identity);
            AuthenticationTicket ticket = new(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch
        {
            return AuthenticateResult.Fail("Authentification has failed");
        }
    }
}