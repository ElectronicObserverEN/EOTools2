using EOToolsWeb.Shared.Translations;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Translations;

public class GithubTranslationFileProvider : ViewModelBase
{
    private HttpClient Client { get; } = new HttpClient();

    public async Task<Dictionary<string, string>> GetTranslations(TranslationKind kind, Language lang)
    {
        string url = BuildUrl(kind, lang);

        HttpResponseMessage response = await Client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string json = response.Content.ReadAsStringAsync().Result;

        return kind switch
        {
            TranslationKind.ShipsName => await ParseTranslationsFromProperty("ship", json),
            TranslationKind.ShipsSuffixes => await ParseTranslationsFromProperty("suffix", json),
            TranslationKind.MapName => await ParseTranslationsFromProperty("map", json),
            TranslationKind.FleetName => await ParseTranslationsFromProperty("fleet", json),
            TranslationKind.Equipments => await ParseTranslationsFromProperty("equipment", json),
            TranslationKind.QuestsTitle => await ParseQuestTranslations("name", json),
            TranslationKind.QuestsDescription => await ParseQuestTranslations("desc", json),
            // todo locks
            _ => throw new NotSupportedException()
        };
    }

    private async Task<Dictionary<string, string>> ParseQuestTranslations(string propertyName, string json)
    {
        JsonObject? jsonObject = JsonNode.Parse(json)?.AsObject();

        if (jsonObject is null && ShowDialogService is not null)
        {
            await ShowDialogService.ShowMessage("", "Failed to parse Json");
            return [];
        }

        Dictionary<string, string> result = [];

        foreach (var node in jsonObject.AsObject())
        {
            if (node.Key == "version") continue;

            result.Add(node.Key, node.Value[propertyName].ToString());
        }

        return result;
    }

    private async Task<Dictionary<string, string>> ParseTranslationsFromProperty(string propertyName, string json)
    {
        JsonObject? jsonObject = JsonNode.Parse(json)?.AsObject();

        if (jsonObject is null && ShowDialogService is not null)
        {
            await ShowDialogService.ShowMessage("", "Failed to parse Json");
            return [];
        }

        Dictionary<string, string> result = [];

        foreach (var node in jsonObject[propertyName].AsObject())
        {
            result.Add(node.Key, node.Value.ToString());
        }

        return result;
    }

    private string BuildUrl(TranslationKind kind, Language lang)
    {
        string kindName = kind switch
        {
            TranslationKind.ShipsName or TranslationKind.ShipsSuffixes => "ship",
            TranslationKind.MapName or TranslationKind.FleetName => "operation",
            TranslationKind.Equipments => "equipment",
            TranslationKind.QuestsTitle or TranslationKind.QuestsDescription => "quest",
            // todo locks
            _ => throw new NotSupportedException()
        };

        return $"https://raw.githubusercontent.com/ElectronicObserverEN/Data/master/Translations/{lang.GetCulture()}/{kindName}.json";
    }
}
