using EOToolsWeb.Shared.Translations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Translations
{
    public class ShipTranslationManager(TranslationManagerViewModel translationManager)
    {
        private TranslationManagerViewModel TranslationManager { get; } = translationManager;

        private Dictionary<string, string> ShipList { get; set; } = [];
        private Dictionary<string, string> SuffixList {get; set; } = [];

        public async Task Initialize()
        {
            ShipList = (await TranslationManager.LoadTranslations(TranslationKind.ShipsName))
                .ToDictionary(t => t.GetTranslation(Language.Japanese)?.Translation ?? "", t => t.GetTranslation(Language.English)?.Translation ?? "");

            SuffixList = (await TranslationManager.LoadTranslations(TranslationKind.ShipsSuffixes))
                .ToDictionary(t => t.GetTranslation(Language.Japanese)?.Translation ?? "", t => t.GetTranslation(Language.English)?.Translation ?? "");
        }

        public string TranslateName(string rawData)
        {
            // save current ship name to prevent suffix replacements that can show up in names
            // tre suffix can be found in Intrepid which gets you In Trepid
            string currentShipName = "";

            foreach (var s in ShipList.OrderByDescending(s => s.Key.Length))
            {
                if (rawData.Equals(s.Key)) return s.Value;

                if (rawData.StartsWith(s.Key))
                {
                    var pos = rawData.IndexOf(s.Key);
                    rawData = rawData.Remove(pos, s.Key.Length).Insert(pos, s.Value);
                    currentShipName = s.Key;
                }
            }

            var name = rawData; // prevent suffix from being replaced twice.

            foreach (var sf in SuffixList.OrderByDescending(sf => sf.Key.Length))
            {
                if (rawData.Contains(sf.Key))
                {
                    var pos = rawData.IndexOf(sf.Key);

                    if (pos < currentShipName.Length) continue;

                    rawData = rawData.Remove(pos, sf.Key.Length).Insert(pos, new string('0', sf.Value.Length));
                    name = name.Remove(pos, sf.Key.Length).Insert(pos, sf.Value);

                    if (rawData.Substring(pos - 1, 1).Contains(" ") == false)
                    {
                        rawData = rawData.Insert(pos, " ");
                        name = name.Insert(pos, " ");
                    }
                }
            }

            return name;
        }
    }
}
