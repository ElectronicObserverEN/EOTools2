using EOToolsWeb.Api.Database;
using EOToolsWeb.Api.Services.UpdateData;
using EOToolsWeb.Shared.Quests;
using EOToolsWeb.Shared.Translations;
using EOToolsWeb.Shared.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EOToolsWeb.Api.Controllers.Quest;

[ApiController]
[Route("[controller]")]
public class QuestTranslationsController(EoToolsDbContext db, UpdateQuestDataService updateService) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;
    private UpdateQuestDataService UpdateQuestDataService { get; } = updateService;

    [HttpGet]
    [Authorize(AuthenticationSchemes = "TokenAuthentication")]
    public async Task<List<QuestTranslationModel>> Get()
    {
        List<QuestModel> quests = await Database.Quests
            .Include(nameof(QuestTranslationModel.Translations))
            .ToListAsync();

        List<QuestTranslationModel> tls = await Database.QuestTranslations
            .Include(nameof(QuestTranslationModel.Translations))
            .ToListAsync();

        foreach (QuestModel eq in quests)
        {
            QuestTranslationModel? questTranslation = tls.Find(e => e.QuestId == eq.Id);

            if (questTranslation is null)
            {
                questTranslation = new()
                {
                    QuestId = eq.Id,
                    Translations = new(),
                };

                Database.QuestTranslations.Add(questTranslation);
            }
        }

        await Database.SaveChangesAsync();

        tls = await Database.QuestTranslations
            .Include(nameof(QuestTranslationModel.Translations))
            .ToListAsync();

        foreach (QuestModel eq in quests)
        {
            QuestTranslationModel? questTranslation = tls.Find(e => e.QuestId == eq.Id);

            if (questTranslation is not null)
            {
                questTranslation.Translations.Add(new()
                {
                    Language = Language.English,
                    Translation = eq.NameEN,
                });

                questTranslation.Translations.Add(new()
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

        QuestTranslationModel? savedData = Database.QuestTranslations
            .Include(nameof(QuestTranslationModel.Translations))
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

        Database.QuestTranslations.Update(savedData);
        await Database.TrackAndSaveChanges();

        return Ok(savedData);
    }

    [HttpPut("pushTranslations")]
    [Authorize(AuthenticationSchemes = "TokenAuthentication", Roles = nameof(UserKind.Admin))]
    public async Task<IActionResult> UpdateData()
    {
        await UpdateQuestDataService.UpdateQuestTranslations();

        return Ok();
    }
}
