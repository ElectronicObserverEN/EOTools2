using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Equipments;
using EOToolsWeb.Shared.Maps;
using EOToolsWeb.Shared.Ships;
using EOToolsWeb.Shared.Translations;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Equipments;

[ApiController]
[Route("[controller]")]
public class EquipmentTranslationsController(EoToolsDbContext db, UpdateEquipmentDataService updateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateEquipmentDataService UpdateEquipmentDataService { get; } = updateService;

    [HttpGet]
    [Authorize(AuthenticationSchemes = "TokenAuthentication")]
    public async Task<List<EquipmentTranslationModel>> Get()
    {
        List<EquipmentModel> equipments = await Database.Equipments
            .Include(nameof(EquipmentTranslationModel.Translations))
            .ToListAsync();

        List<EquipmentTranslationModel> tls = await Database.EquipmentTranslations
            .Include(nameof(EquipmentTranslationModel.Translations))
            .ToListAsync();

        foreach (EquipmentModel eq in equipments)
        {
            EquipmentTranslationModel? equipmentTranslation = tls.Find(e => e.EquipmentId == eq.Id);

            if (equipmentTranslation is null)
            {
                equipmentTranslation = new()
                {
                    EquipmentId = eq.Id,
                    Translations = new(),
                };

                Database.EquipmentTranslations.Add(equipmentTranslation);
            }
        }

        await Database.SaveChangesAsync();

        tls = await Database.EquipmentTranslations
            .Include(nameof(EquipmentTranslationModel.Translations))
            .ToListAsync();

        foreach (EquipmentModel eq in equipments)
        {
            EquipmentTranslationModel? equipmentTranslation = tls.Find(e => e.EquipmentId == eq.Id);

            if (equipmentTranslation is not null)
            {
                equipmentTranslation.Translations.Add(new()
                {
                    Language = Language.English,
                    Translation = eq.NameEN,
                });

                equipmentTranslation.Translations.Add(new()
                {
                    Language = Language.Japanese,
                    Translation = eq.NameJP,
                });
            }
        }

        return tls;
    }

    [HttpPut("updateTranslation")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication")]
    public async Task<IActionResult> Put(TranslationModel newData)
    {
        if (newData.Language is Language.English or Language.Japanese)
        {
            return Unauthorized();
        }

        EquipmentTranslationModel? savedData = Database.EquipmentTranslations
            .Include(nameof(EquipmentTranslationModel.Translations))
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
        }

        Database.EquipmentTranslations.Update(savedData);
        await Database.TrackAndSaveChanges();

        return Ok(savedData);
    }

    [HttpPut("pushTranslations")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> UpdateData()
    {
        await UpdateEquipmentDataService.UpdateEquipmentTranslations();

        return Ok();
    }
}
