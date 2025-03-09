using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Maps;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Translations;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Ships;

[ApiController]
[Route("[controller]")]
public class ShipNameTranslationsController(EoToolsDbContext db, UpdateShipDataService dataUpdateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateShipDataService DataUpdateService { get; } = dataUpdateService;

    [HttpGet]
    [Authorize(AuthenticationSchemes = "TokenAuthentication")]
    public List<ShipNameTranslationModel> Get()
    {
        return Database.ShipTranslations
            .Include(nameof(ShipNameTranslationModel.Translations))
            .ToList();
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> Put(ShipNameTranslationModel newData)
    {
        ShipNameTranslationModel? savedData = Database.ShipTranslations
            .Include(nameof(ShipNameTranslationModel.Translations))
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

        Database.ShipTranslations.Update(savedData);
        await Database.SaveChangesAsync();

        return Ok(savedData);
    }

    [HttpPut("updateTranslation")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication")]
    public async Task<IActionResult> Put(TranslationModel newData)
    {
        if (newData.Language is Language.English or Language.Japanese && !HttpContext.User.IsInRole(nameof(UserKind.Admin)))
        {
            return Unauthorized();
        }

        ShipNameTranslationModel? savedData = Database.ShipTranslations
            .Include(nameof(ShipNameTranslationModel.Translations))
            .FirstOrDefault(tl => tl.Id == newData.Id);

        if (savedData is null)
        {
            return NotFound();
        }

        TranslationModel? savedTranslation = savedData.Translations.Find(tl => tl.Language == newData.Language);

        if (savedTranslation is null)
        {
            savedTranslation = new()
            {
                Translation = newData.Translation,
                Language = newData.Language,
            };

            savedData.Translations.Add(savedTranslation);
            Database.Add(savedTranslation);
        }
        else
        {
            savedTranslation.Translation = newData.Translation;
            savedTranslation.IsPendingChange = true;
        }

        Database.ShipTranslations.Update(savedData);
        await Database.TrackAndSaveChanges();

        return Ok(savedData);
    }

    [HttpPut("pushTranslations")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> UpdateData()
    {
        await DataUpdateService.UpdateShipTranslations();

        return Ok();
    }

    [HttpPost]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> Post(ShipNameTranslationModel newData)
    {
        foreach (TranslationModel newTranslation in newData.Translations)
        {
            Database.Add(newTranslation);
        }

        await Database.ShipTranslations.AddAsync(newData);
        await Database.SaveChangesAsync();

        return Ok(newData);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> Delete(int id)
    {
        ShipNameTranslationModel? data = await Database.ShipTranslations
            .Include(nameof(ShipNameTranslationModel.Translations))
            .FirstOrDefaultAsync(tl => tl.Id == id);

        if (data is null)
        {
            return NotFound();
        }

        Database.RemoveRange(data.Translations);
        Database.ShipTranslations.Remove(data);
        await Database.SaveChangesAsync();

        return Ok();
    }

#if DEBUG

    [HttpPost("init/{tlLanguage}")]
    public async Task<IActionResult> Post(ShipTranslationModel translations, Language tlLanguage)
    {
        foreach (KeyValuePair<string, string> newTranslation in translations.Ships)
        {
            ShipNameTranslationModel? savedData = Database.ShipTranslations
                .Include(nameof(ShipNameTranslationModel.Translations))
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
