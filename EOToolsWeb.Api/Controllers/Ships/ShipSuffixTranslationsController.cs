using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Translations;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Ships;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class ShipSuffixTranslationsController(EoToolsDbContext db, UpdateShipDataService dataUpdateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateShipDataService DataUpdateService { get; } = dataUpdateService;

    [HttpGet]
    public List<ShipSuffixTranslationModel> Get()
    {
        return Database.ShipSuffixTranslations
            .Include(nameof(ShipSuffixTranslationModel.Translations))
            .ToList();
    }

    [HttpPut]
    public async Task<IActionResult> Put(ShipSuffixTranslationModel newData)
    {
        ShipSuffixTranslationModel? savedData = Database.ShipSuffixTranslations
            .Include(nameof(ShipSuffixTranslationModel.Translations))
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

        Database.ShipSuffixTranslations.Update(savedData);
        await Database.SaveChangesAsync();

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
    public async Task<IActionResult> Post(ShipSuffixTranslationModel newData)
    {
        foreach (TranslationModel newTranslation in newData.Translations)
        {
            Database.Add(newTranslation);
        }

        await Database.ShipSuffixTranslations.AddAsync(newData);
        await Database.SaveChangesAsync();

        return Ok(newData);
    }

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> Delete(int id)
    {
        ShipSuffixTranslationModel? data = await Database.ShipSuffixTranslations
            .Include(nameof(ShipSuffixTranslationModel.Translations))
            .FirstOrDefaultAsync(tl => tl.Id == id);

        if (data is null)
        {
            return NotFound();
        }

        Database.RemoveRange(data.Translations);
        Database.ShipSuffixTranslations.Remove(data);
        await Database.SaveChangesAsync();

        return Ok();
    }

#if DEBUG

    [HttpPost("init/{tlLanguage}")]
    public async Task<IActionResult> Post(ShipTranslationModel translations, Language tlLanguage)
    {
        foreach (KeyValuePair<string, string> newTranslation in translations.Suffixes)
        {
            ShipSuffixTranslationModel? savedData = Database.ShipSuffixTranslations
                .Include(nameof(ShipSuffixTranslationModel.Translations))
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
