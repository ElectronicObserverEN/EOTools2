using System.ComponentModel;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace EOTools.Models
{
    public class QuestData : INotifyPropertyChanged
    {
        [JsonIgnore]
        public int QuestID { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("name_jp")]
        public string NameJP { get; set; }

        private string nameEN;

        [JsonPropertyName("name")]
        public string NameEN
        {
            get { return nameEN; }
            set { nameEN = value; OnPropertyChanged(); }
        }

        [JsonPropertyName("desc_jp")]
        public string DescJP { get; set; }

        [JsonPropertyName("desc")]
        public string DescEN { get; set; }

        [JsonIgnore]
        public string DisplayName
        {
            get
            {
                if (string.IsNullOrEmpty(NameEN)) return "NOT-TRANSLATED QUEST";

                return NameEN;
            }
        }

        public QuestData(int _questID, JsonObject _object)
        {
            QuestID = _questID;
            Code = _object["code"].GetValue<string>();
            NameJP = _object["name_jp"].GetValue<string>();
            NameEN = _object["name"].GetValue<string>();
            DescJP = _object["desc_jp"].GetValue<string>();
            DescEN = _object["desc"].GetValue<string>();
        }

        public QuestData(int _questID)
        {
            QuestID = _questID;
            Code = "";
            NameJP = "";
            NameEN = "";
            DescJP = "";
            DescEN = "";
        }

        public QuestData()
        {
            Code = "";
            NameJP = "";
            NameEN = "";
            nameEN = "";
            DescJP = "";
            DescEN = "";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
