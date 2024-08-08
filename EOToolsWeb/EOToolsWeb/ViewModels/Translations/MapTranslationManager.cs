using System;
using EOToolsWeb.Shared.Maps;
using EOToolsWeb.Shared.Translations;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using EOToolsWeb.Models.Settings;
using EOToolsWeb.Models.Translations;
using System.Net.Http.Json;
using EOToolsWeb.Extensions.Translations;

namespace EOToolsWeb.ViewModels.Translations
{
    public class MapTranslationManager(TranslationManagerViewModel translationManager, SettingsModel settings, HttpClient client)
    {
        private TranslationManagerViewModel TranslationManager { get; } = translationManager;
        private HttpClient HttpClient { get; } = client;

        private List<TranslationBaseModelRow> MapList { get; set; } = [];
        private List<TranslationBaseModelRow> FleetList { get; set; } = [];

        private SettingsModel Settings { get; } = settings;

        private string GetDataPath => Path.Combine(Settings.KancolleEoApiFolder, "kcsapi", "api_start2", "getData");
        private string FleetsFilePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EOTools", "parsedFleets.json");

        public async Task Initialize()
        {
            MapList = await TranslationManager.LoadTranslations(TranslationKind.MapName);

            FleetList = await TranslationManager.LoadTranslations(TranslationKind.FleetName);
        }

        public async Task GetNonTranslatedDataAndPushItToTheApi()
        {
            // --- Read untranslated stuff : 
            string text = await File.ReadAllTextAsync(GetDataPath);

            // --- revome svdata=
            text = text[7..];

            JsonObject? mapApi = JsonSerializer.Deserialize<JsonObject>(text);

            if (mapApi is not null)
            {
                await UpdateMaps(mapApi);
                await UpdateAreas(mapApi);
            }

            await UpdateFleets();
        }

        /// <summary>
        /// This thing work with a service from old EO tools
        /// TODO : use EO API instead to create translation issues when untranslated fleet is found
        /// </summary>
        /// <returns></returns>
        private async Task UpdateFleets()
        {
            // --- Fleets unstranslated : 
            JsonArray? fleetsUntranslated = JsonSerializer.Deserialize<JsonArray>(await File.ReadAllTextAsync(FleetsFilePath));

            if (fleetsUntranslated is null) return;
            
            foreach (JsonNode? node in fleetsUntranslated)
            {
                if (node is null) continue;

                string fleetName = node.GetValue<string>();

                TranslationBaseModelRow? tl = FleetList.Find(m =>
                    m.Translations.Any(t => t.Translation == fleetName && t.Language is Language.Japanese));

                if (tl is null)
                {
                    MapNameTranslationModel model = new()
                    {
                        Translations = Enum.GetValues<Language>().Select(l => new TranslationModel()
                            {
                                Language = l,
                                Translation = fleetName,
                            })
                            .ToList(),
                    };

                    HttpResponseMessage response =
                        await HttpClient.PostAsJsonAsync(TranslationKind.FleetName.GetApiRoute(), model);

                    response.EnsureSuccessStatusCode();

                    TranslationBaseModelRow? postedModel =
                        await response.Content.ReadFromJsonAsync<TranslationBaseModelRow>();

                    if (postedModel is not null)
                    {
                        FleetList.Add(postedModel);
                    }
                }
            }
        }

        private async Task UpdateAreas(JsonObject mapApi)
        {
            JsonArray? areas = mapApi["api_data"]?["api_mst_maparea"]?.AsArray();

            if (areas is null) return;

            foreach (JsonNode? area in areas)
            {
                if (area is null) continue;

                string areaName = area["api_name"]?.GetValue<string>() ?? "";
                int? worldId = area["api_id"]?.GetValue<int>();

                TranslationBaseModelRow? tl = MapList.Find(m =>
                    m.Translations.Any(t => t.Translation == areaName && t.Language is Language.Japanese));

                if (tl is null)
                {
                    MapNameTranslationModel model = new()
                    {
                        Translations = Enum.GetValues<Language>().Select(l => new TranslationModel()
                            {
                                Language = l,
                                Translation = l is Language.Japanese ? areaName : $"{areaName} (World {worldId})",
                            })
                            .ToList(),
                    };

                    HttpResponseMessage response =
                        await HttpClient.PostAsJsonAsync(TranslationKind.MapName.GetApiRoute(), model);

                    response.EnsureSuccessStatusCode();

                    TranslationBaseModelRow? postedModel =
                        await response.Content.ReadFromJsonAsync<TranslationBaseModelRow>();

                    if (postedModel is not null)
                    {
                        MapList.Add(postedModel);
                    }
                }
            }
        }

        private async Task UpdateMaps(JsonObject mapApi)
        {
            JsonArray? mapsFromAPI = mapApi["api_data"]?["api_mst_mapinfo"]?.AsArray();

            if (mapsFromAPI is null) return;

            foreach (JsonNode? mapInfo in mapsFromAPI)
            {
                if (mapInfo is null) continue;

                string mapName = mapInfo["api_name"]?.GetValue<string>() ?? "";
                int? mapId = mapInfo["api_no"]?.GetValue<int>();
                int? worldId = mapInfo["api_maparea_id"]?.GetValue<int>();

                TranslationBaseModelRow? tl = MapList.Find(m =>
                    m.Translations.Any(t => t.Translation == mapName && t.Language is Language.Japanese));

                if (tl is null)
                {
                    MapNameTranslationModel model = new()
                    {
                        Translations = Enum.GetValues<Language>().Select(l => new TranslationModel()
                            {
                                Language = l,
                                Translation = l is Language.Japanese ? mapName : $"{mapName} ({worldId}-{mapId})",
                            })
                            .ToList(),
                    };

                    HttpResponseMessage response =
                        await HttpClient.PostAsJsonAsync(TranslationKind.MapName.GetApiRoute(), model);

                    response.EnsureSuccessStatusCode();

                    TranslationBaseModelRow? postedModel =
                        await response.Content.ReadFromJsonAsync<TranslationBaseModelRow>();

                    if (postedModel is not null)
                    {
                        MapList.Add(postedModel);
                    }
                }
            }
        }
    }
}
