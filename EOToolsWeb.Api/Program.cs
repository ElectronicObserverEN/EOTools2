using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Sessions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.Reflection.Metadata;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddOpenApiDocument(option =>
{
    option.AddSecurity("ApiAuthentication", new OpenApiSecurityScheme()
    {
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Username / Password",
        Name = "Authorization",
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = "Basic",
    });

    option.AddSecurity("TokenAuthentication", new OpenApiSecurityScheme
    {
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Token",
        Name = "X-TOKEN-EO-TOOLS-WEB-X",
        Type = OpenApiSecuritySchemeType.ApiKey,
        Scheme = "",
    });

    option.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("ApiAuthentication"));
    option.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("TokenAuthentication"));
});

builder.Services.AddScoped<UsersService>();

builder.Services.AddAuthentication("TokenAuthentication")
    .AddScheme<AuthenticationSchemeOptions, ApiLoginHandler>("ApiAuthentication", null)
    .AddScheme<AuthenticationSchemeOptions, ApiTokenHandler>("TokenAuthentication", null);


await using EoToolsUsersDbContext userDb = new();
await using EoToolsDbContext db = new EoToolsDbContext(new CurrentSession(), userDb);
db.Database.Migrate();
userDb.Database.Migrate();

builder.Services.AddDbContext<EoToolsDbContext>();
builder.Services.AddDbContext<EoToolsUsersDbContext>();

if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
{
    builder.Services.AddSingleton<IGitManagerService, GitManagerServiceLinux>();
}
else
{
    builder.Services.AddSingleton<IGitManagerService, GitManagerService>();
}

builder.Services.AddSingleton<ConfigurationService>();
builder.Services.AddScoped<DatabaseSyncService>(); 
builder.Services.AddScoped<UpdateMaintenanceDataService>();
builder.Services.AddScoped<UpdateShipDataService>();
builder.Services.AddScoped<UpdateEquipmentDataService>();
builder.Services.AddScoped<UpdateQuestDataService>();
builder.Services.AddScoped<UpdateShipLockDataService>();
builder.Services.AddScoped<UpdateEquipmentUpgradeDataService>(); 
builder.Services.AddScoped<FitBonusUpdaterService>();
builder.Services.AddScoped<OperationUpdateService>();

builder.Services.AddScoped<ICurrentSession, CurrentSession>();

builder.Services.AddSingleton(_ => new JsonSerializerOptions()
{
    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
    WriteIndented = true,
});

var app = builder.Build();

await app.Services.GetRequiredService<ConfigurationService>().Initialize();
await app.Services.GetRequiredService<IGitManagerService>().Initialize();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.PersistAuthorization = true;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
