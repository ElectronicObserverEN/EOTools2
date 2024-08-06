using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Ships;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class ShipNameTranslationsController(EoToolsDbContext db, UpdateShipDataService dataUpdateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateShipDataService DataUpdateService { get; } = dataUpdateService;

    [HttpGet]
    public List<ShipNameTranslationModel> Get()
    {
        return Database.ShipTranslations
            .Include(nameof(ShipNameTranslationModel.Translations))
            .ToList();
    }

    [HttpPut]
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

    [HttpPut("pushTranslations")]
    public async Task<IActionResult> UpdateData()
    {
        await DataUpdateService.UpdateShipTranslations();

        return Ok();
    }

    [HttpPost]
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
    public async Task<IActionResult> Delete(int id)
    {
        ShipNameTranslationModel? data = await Database.ShipTranslations.FindAsync(id);

        if (data is null)
        {
            return NotFound();
        }

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
