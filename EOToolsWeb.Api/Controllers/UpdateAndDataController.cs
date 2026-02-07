using EOToolsWeb.Shared.Translations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers;

[ApiController]
[Route("DataRepo")]
public class UpdateAndDataController() : ControllerBase
{
    private static string[] ValidDataFiles { get; } = [
        "destination.json",
        "Equipments.json",
        "EquipmentUpgrades.json",
        "Events.json",
        "FitBonuses.json",
        "Locks.json",
        "Quests.json",
        "QuestTrackers.json",
        "TimeLimitedQuests.json",
        "Seasons.json",
        "ShipClass.json",
        "Ships.json",
        "Updates.json",
    ];
    
    private static string[] ValidTranslationFiles { get; } = [
        "equipment.json",
        "expedition.json",
        "Locks.json",
        "operation.json",
        "quest.json",
        "ship.json",
        "update.json",
    ];

    private static List<string> ValidLanguages  { get; } = Enum.GetValues<Language>().Select(l => l.GetCulture().ToLower()).ToList();
    
    [AllowAnonymous]
    [HttpGet("update.json")]
    public IActionResult GetUpdateFile()
    {
        return File(System.IO.File.OpenRead(Path.Combine("Data", "DataRepo", "update.json")), "application/json");
    }
    
    [AllowAnonymous]
    [HttpGet("Data/EOUpdater.exe")]
    public IActionResult GetUpdater()
    {
        return File(System.IO.File.OpenRead(Path.Combine("Data", "DataRepo", "Data", "EOUpdater.exe")), "application/exe");
        
    }
    
    [AllowAnonymous]
    [HttpGet("Data/{fileName}")]
    public IActionResult GetDataFile(string fileName)
    {
        if (!ValidDataFiles.Contains(fileName)) return NotFound();
        
        return File(System.IO.File.OpenRead(Path.Combine("Data", "DataRepo", "Data", fileName)), "application/json");
    }
    
    [AllowAnonymous]
    [HttpGet("Translations/{lang}/{fileName}")]
    public IActionResult GetTranslationFile(string lang, string fileName)
    {
        if (!ValidTranslationFiles.Contains(fileName)) return NotFound();
        if (!ValidLanguages.Contains(lang.ToLower())) return NotFound();
        
        return File(System.IO.File.OpenRead(Path.Combine("Data", "DataRepo", "Translations", lang, fileName)), "application/json");
    }
}