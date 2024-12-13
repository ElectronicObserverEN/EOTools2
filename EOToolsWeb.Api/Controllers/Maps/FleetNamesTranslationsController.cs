using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Maps;
using EOToolsWeb.Shared.Translations;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Maps;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class FleetNamesTranslationsController(EoToolsDbContext db, OperationUpdateService dataUpdateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private OperationUpdateService DataUpdateService { get; } = dataUpdateService;

    [HttpGet]
    public List<FleetNameTranslationModel> Get()
    {
        return Database.Fleets
            .Include(nameof(FleetNameTranslationModel.Translations))
            .ToList();
    }

    [HttpPut]
    public async Task<IActionResult> Put(FleetNameTranslationModel newData)
    {
        FleetNameTranslationModel? savedData = Database.Fleets
            .Include(nameof(FleetNameTranslationModel.Translations))
            .FirstOrDefault(tl => tl.Id == newData.Id);

        if (savedData is null)
        {
            return NotFound();
        }

        foreach (TranslationModel newTranslation in newData.Translations)
        {
            TranslationModel? savedTranslation = savedData.Translations.Find(tl => tl.Language == newTranslation.Language);

            if (savedTranslation is null)
            {
                savedTranslation = new()
                {
                    Translation = newTranslation.Translation,
                    Language = newTranslation.Language,
                };

                savedData.Translations.Add(savedTranslation);
                Database.Add(savedTranslation);
            }
            else
            {
                savedTranslation.Translation = newTranslation.Translation;
            }
        }

        Database.Fleets.Update(savedData);
        await Database.SaveChangesAsync();

        return Ok(savedData);
    }

    [HttpPut("pushTranslations")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> UpdateData()
    {
        await DataUpdateService.PushTranslations();

        return Ok();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> Post(FleetNameTranslationModel newData)
    {
        foreach (TranslationModel newTranslation in newData.Translations)
        {
            Database.Add(newTranslation);
        }

        await Database.Fleets.AddAsync(newData);
        await Database.SaveChangesAsync();

        return Ok(newData);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> Delete(int id)
    {
        FleetNameTranslationModel? data = await Database.Fleets
            .Include(nameof(FleetNameTranslationModel.Translations))
            .FirstOrDefaultAsync(tl => tl.Id == id);

        if (data is null)
        {
            return NotFound();
        }

        Database.RemoveRange(data.Translations);
        Database.Fleets.Remove(data);
        await Database.SaveChangesAsync();

        return Ok();
    }

#if DEBUG

    [HttpPost("init/{tlLanguage}")]
    public async Task<IActionResult> Post(MapsTranslationModel translations, Language tlLanguage)
    {
        foreach (KeyValuePair<string, string> newTranslation in translations.Fleets)
        {
            FleetNameTranslationModel? savedData = Database.Fleets
                .Include(nameof(FleetNameTranslationModel.Translations))
                .FirstOrDefault(tl => tl.Translations.Any(t => t.Translation == newTranslation.Key && t.Language == Language.Japanese));

            if (savedData is null)
            {
                savedData = new();

                TranslationModel jpTl = new()
                {
                    Translation = newTranslation.Key,
                    Language = Language.Japanese,
                };

                savedData.Translations.Add(jpTl);

                Database.Add(jpTl);
                Database.Add(savedData);
            }

            TranslationModel otherTl = new()
            {
                Translation = newTranslation.Value,
                Language = tlLanguage,
            };

            savedData.Translations.Add(otherTl);
            Database.Add(otherTl);
        }

        await Database.SaveChangesAsync();

        return Ok();
    }
#endif
}
