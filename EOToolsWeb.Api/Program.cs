using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
});

builder.Services.AddScoped<UsersService>();

builder.Services.AddAuthentication("")
    .AddScheme<AuthenticationSchemeOptions, ApiLoginHandler>("ApiAuthentication", null);

using EoToolsDbContext db = new();
db.Database.Migrate();

builder.Services.AddDbContext<EoToolsDbContext>();

builder.Services.AddSingleton<ConfigurationService>();
builder.Services.AddSingleton<GitManagerService>();

var app = builder.Build();

await app.Services.GetRequiredService<ConfigurationService>().Initialize();
await app.Services.GetRequiredService<GitManagerService>().Initialize();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
