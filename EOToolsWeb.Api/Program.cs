using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services;
using EOToolsWeb.Api.Services.GitManager;
using EOToolsWeb.Api.Services.UpdateData;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Runtime.InteropServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using EOToolsWeb.Shared.Sessions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition("ApiAuthentication", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Username / Password",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Basic",
    });

    option.AddSecurityDefinition("TokenAuthentication", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Token",
        Name = "X-TOKEN-EO-TOOLS-WEB-X",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "",
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiAuthentication"
                }
            },
            new string[]{}
        }
    });

    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "TokenAuthentication"
                }
            },
            new string[]{}
        }
    });
});

builder.Services.AddScoped<UsersService>();

builder.Services.AddAuthentication("TokenAuthentication")
    .AddScheme<AuthenticationSchemeOptions, ApiLoginHandler>("ApiAuthentication", null)
    .AddScheme<AuthenticationSchemeOptions, ApiTokenHandler>("TokenAuthentication", null);


await using EoToolsDbContext db = new EoToolsDbContext(new CurrentSession());
db.Database.Migrate();

builder.Services.AddDbContext<EoToolsDbContext>();

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
    app.UseSwagger();
    app.UseSwaggerUI(config =>
    {
        config.EnablePersistAuthorization();
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
