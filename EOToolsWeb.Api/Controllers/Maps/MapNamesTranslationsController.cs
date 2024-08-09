using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Maps;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Maps;

[ApiController]
[Authorize(AuthenticationSchemes = "TokenAuthentication")]
[Route("[controller]")]
public class MapNamesTranslationsController(EoToolsDbContext db, OperationUpdateService dataUpdateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private OperationUpdateService DataUpdateService { get; } = dataUpdateService;

    [HttpGet]
    public List<MapNameTranslationModel> Get()
    {
        return Database.Maps
            .Include(nameof(MapNameTranslationModel.Translations))
            .ToList();
    }

    [HttpPut]
    public async Task<IActionResult> Put(MapNameTranslationModel newData)
    {
        MapNameTranslationModel? savedData = Database.Maps
            .Include(nameof(MapNameTranslationModel.Translations))
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

        Database.Maps.Update(savedData);
        await Database.SaveChangesAsync();

        return Ok(savedData);
    }

    [HttpPut("pushTranslations")]
    public async Task<IActionResult> UpdateData()
    {
        await DataUpdateService.PushTranslations();

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Post(MapNameTranslationModel newData)
    {
        foreach (TranslationModel newTranslation in newData.Translations)
        {
            Database.Add(newTranslation);
        }

        await Database.Maps.AddAsync(newData);
        await Database.SaveChangesAsync();

        return Ok(newData);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        MapNameTranslationModel? data = await Database.Maps
            .Include(nameof(MapNameTranslationModel.Translations))
            .FirstOrDefaultAsync(tl => tl.Id == id);

        if (data is null)
        {
            return NotFound();
        }

        Database.RemoveRange(data.Translations);
        Database.Maps.Remove(data);
        await Database.SaveChangesAsync();

        return Ok();
    }

#if DEBUG

    [HttpPost("init/{tlLanguage}")]
    public async Task<IActionResult> Post(MapsTranslationModel translations, Language tlLanguage)
    {
        foreach (KeyValuePair<string, string> newTranslation in translations.Maps)
        {
            MapNameTranslationModel? savedData = Database.Maps
                .Include(nameof(MapNameTranslationModel.Translations))
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
