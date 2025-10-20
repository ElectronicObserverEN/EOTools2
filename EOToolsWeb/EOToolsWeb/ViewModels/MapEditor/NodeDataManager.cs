using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using EOToolsWeb.Shared.MapData;

namespace EOToolsWeb.ViewModels.MapEditor;

public class NodeDataManager
{
    private HttpClient HttpClient { get; }

    public NodeDataManager(HttpClient client)
    {
        HttpClient = client;
    }

    public async Task<List<NodeModel>?> GetNodes(int worldId, int mapId)
    {
        return await HttpClient.GetFromJsonAsync<List<NodeModel>>($"NodeData?worldId={worldId}&mapId={mapId}") ?? [];
    }
    
    public async Task AddNode(NodeModel node)
    {
        await HttpClient.PostAsJsonAsync("NodeData", node);
    }
    
    public async Task EditNode(NodeModel node)
    {
        await HttpClient.PutAsJsonAsync("NodeData", node);
    }
}