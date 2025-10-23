namespace EOToolsWeb.Shared.MapData;

public record NodeModel
{
    public int? Id { get; set; }
    
    public int MapId { get; set; }
    
    public int WorldId { get; set; }
    
    public int Number { get; set; }
    
    public string Code { get; set; } = "";

    public NodeType NodeType { get; set; } = NodeType.Empty;
    
    public double X { get; set; }
    public double Y { get; set; }
}