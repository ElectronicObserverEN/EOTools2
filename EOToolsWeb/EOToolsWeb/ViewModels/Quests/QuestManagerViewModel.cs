using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Shared.Quests;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reactive.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.Quests;

public partial class QuestManagerViewModel : ViewModelBase
{
    public ObservableCollection<QuestModel> QuestListFiltered { get; set; } = new();
    public List<QuestModel> QuestList { get; set; } = [];

    [ObservableProperty]
    private string _filter = "";

    private HttpClient HttpClient { get; }
    private QuestViewModel QuestViewModel { get; }

    public Interaction<QuestViewModel, bool> ShowEditDialog { get; } = new();
    public Interaction<object?, string?> ReadClipboard { get; } = new();

    public QuestManagerViewModel(HttpClient client, QuestViewModel viewModel)
    {
        HttpClient = client;
        QuestViewModel = viewModel;

        PropertyChanged += QuestManagerViewModel_PropertyChanged;
    }

    public async Task LoadQuests()
    {
        if (QuestList.Count > 0) return;

        QuestList = await HttpClient.GetFromJsonAsync<List<QuestModel>>("Quest") ?? [];

        ReloadQuestList();
    }

    private void QuestManagerViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Filter)) ReloadQuestList();
    }

    private bool QuestMatchesFilter(QuestModel quest)
    {
        if (string.IsNullOrEmpty(Filter)) return true;
        if (quest.Code.ToLower().Contains((string)Filter.ToLower())) return true;
        if (quest.NameEN.ToLower().Contains((string)Filter.ToLower())) return true;
        if (quest.DescEN.ToLower().Contains((string)Filter.ToLower())) return true;
        if (quest.NameJP.ToLower().Contains((string)Filter.ToLower())) return true;
        if (quest.DescJP.ToLower().Contains((string)Filter.ToLower())) return true;

        return false;
    }

    public void ReloadQuestList()
    {
        QuestListFiltered.Clear();

        List<QuestModel> quests = QuestList
            .Where(QuestMatchesFilter)
            .ToList();

        foreach (QuestModel quest in quests)
        {
            QuestListFiltered.Add(quest);
        }
    }

    [RelayCommand]
    public async Task AddQuest()
    {
        try
        {
            QuestModel model = new();

            QuestViewModel.Model = model;
            await QuestViewModel.LoadFromModel();

            if (await ShowEditDialog.Handle(QuestViewModel))
            {
                QuestViewModel.SaveChanges();

                HttpResponseMessage response = await HttpClient.PostAsJsonAsync("Quest", model);

                response.EnsureSuccessStatusCode();

                QuestModel? postedModel = await response.Content.ReadFromJsonAsync<QuestModel>();

                if (postedModel is not null)
                {
                    QuestList.Add(postedModel);

                    ReloadQuestList();
                }
            }
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }


    [RelayCommand]
    private async Task EditQuest(QuestModel vm)
    {
        try
        {
            QuestViewModel.Model = vm;
            await QuestViewModel.LoadFromModel();

            if (await ShowEditDialog.Handle(QuestViewModel))
            {
                QuestViewModel.SaveChanges();

                HttpResponseMessage response = await HttpClient.PutAsJsonAsync("Quest", vm);

                response.EnsureSuccessStatusCode();

                ReloadQuestList();
            }
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }
    
    [RelayCommand]
    public async Task RemoveQuest(QuestModel vm)
    {
        try
        {
            await HttpClient.DeleteAsync($"Quest/{vm.Id}");

            QuestList.Remove(vm);

            ReloadQuestList();
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    public async Task UpdateTranslations()
    {
        try
        {
            await HttpClient.PutAsync("Quest/pushQuests", null);
        }
        catch (Exception ex)
        {
            await HandleException(ex);
        }
    }

    [RelayCommand]
    public async Task AddQuestFromClipboard()
    {
        string? questJson = await ReadClipboard.Handle(null);

        JsonObject quests = JsonSerializer.Deserialize<JsonObject>(questJson);

        foreach (KeyValuePair<string, JsonNode> quest in quests)
        {
            QuestModel newModel = new()
            {
                ApiId = int.Parse(quest.Key),

                Code = quest.Key,

                NameJP = quest.Value["name_jp"].GetValue<string>() ?? "",
                DescJP = quest.Value["desc_jp"].GetValue<string>() ?? "",

                NameEN = quest.Value["name_jp"].GetValue<string>() ?? "",
                DescEN = quest.Value["desc_jp"].GetValue<string>() ?? "",
            };

            bool questExists = QuestList
                .Any(q => q.ApiId == newModel.ApiId && q.Code == newModel.Code);

            if (!questExists)
            {
                HttpResponseMessage response = await HttpClient.PostAsJsonAsync("Quest", newModel);

                response.EnsureSuccessStatusCode();
            }
        }

        QuestList.Clear();
        await LoadQuests();
    }

}
