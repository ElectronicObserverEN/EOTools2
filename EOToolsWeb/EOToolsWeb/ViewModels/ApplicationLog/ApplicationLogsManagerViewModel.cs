using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Shared.ApplicationLog;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.ApplicationLog;

public partial class ApplicationLogsManagerViewModel : ViewModelBase
{
    [ObservableProperty]
    public partial List<DataChangedLogModel> AllLogs { get; set; } = [];

    [ObservableProperty]
    public partial int Count { get; set; } = 50;

    private HttpClient HttpClient { get; }
    
    public ApplicationLogsManagerViewModel(HttpClient client)
    {
        HttpClient = client;

        PropertyChanged += async (_, e) => await CountChanged(e);
    }

    private async Task CountChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(Count)) return;

        await LoadAllLogs();
    }

    public async Task LoadAllLogs()
    {
        AllLogs = await HttpClient.GetFromJsonAsync<List<DataChangedLogModel>>($"Logs?take={Count}&skip=0") ?? [];
    }
}
