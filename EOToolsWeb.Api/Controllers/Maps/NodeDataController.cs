using EOToolsWeb.Api.Database;
using EOToolsWeb.Shared.MapData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EOToolsWeb.Api.Controllers.Maps;

[ApiController]
[Route("[controller]")]
public class NodeDataController(EoToolsDbContext db) : ControllerBase
{
    private EoToolsDbContext Database { get; } = db;

    [HttpGet]
    [Authorize(AuthenticationSchemes = "TokenAuthentication")]
    public List<NodeModel> Get(int worldId, int mapId)
    {
        return Database.Nodes
            .Where(node => node.MapId == mapId)
            .Where(node => node.WorldId == worldId)
            .OrderBy(node => node.Number)
            .ToList();
    }

    [HttpPut]
    public async Task<IActionResult> Put(NodeModel newData)
    {
        NodeModel? savedData = Database.Nodes
            .FirstOrDefault(tl => tl.Id == newData.Id);

        if (savedData is null)
        {
            return NotFound();
        }

        savedData.Code = newData.Code;
        savedData.NodeType = newData.NodeType;
        savedData.X = newData.X;
        savedData.Y = newData.Y;

        Database.Nodes.Update(savedData);
        await Database.SaveChangesAsync();

        return Ok(savedData);
    }

    [HttpPost]
    public async Task<IActionResult> Post(NodeModel newData)
    {
        await Database.Nodes.AddAsync(newData);
        await Database.SaveChangesAsync();

        return Ok(newData);
    }

}
