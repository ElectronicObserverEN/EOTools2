using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Equipments;
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
        return await UpdateEquipmentDataService.GetAllEquipmentTranslations();
    }

    [HttpPut]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> Put(EquipmentTranslationModel newData)
    {
        EquipmentTranslationModel? savedData = Database.EquipmentTranslations
            .Include(nameof(EquipmentTranslationModel.Translations))
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
                    IsPendingChange = true,
                };

                savedData.Translations.Add(savedTranslation);
                Database.Add(savedTranslation);
            }
            else
            {
                savedTranslation.Translation = newTranslation.Translation;
            }
        }

        Database.EquipmentTranslations.Update(savedData);
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
                IsPendingChange = true,
            };

            savedData.Translations.Add(savedTranslation);
            Database.Add(savedTranslation);
        }
        else
        {
            savedTranslation.Translation = newData.Translation;
            savedTranslation.IsPendingChange = true;
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
