using System.Text.Json.Nodes;

namespace EOToolsWeb.Shared.Quests
{
    public class QuestTrackerData
    {
        public int QuestID { get; set; }

        public JsonArray? QuestData { get; set; }
    }
}
