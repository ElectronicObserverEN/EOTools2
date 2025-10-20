using CommunityToolkit.Mvvm.ComponentModel;
using EOToolsWeb.Shared.MapData;

namespace EOToolsWeb.ViewModels.MapEditor;

public partial class NodeViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Code { get; set; } = "";

    [ObservableProperty]
    public partial NodeType NodeType { get; set; } = NodeType.Empty;
    
    [ObservableProperty]
    public partial double X { get; set; }
    
    [ObservableProperty]
    public partial double Y { get; set; }
    
    public NodeModel? Model { get; set; }

    public NodeViewModel(NodeModel model)
    {
        Model = model;
        
        LoadFromModel();
    }

    public void LoadFromModel()
    {
        Code = Model?.Code ?? "";
        NodeType =  Model?.NodeType ?? NodeType.Empty;
        X = Model?.X ?? 0;
        Y = Model?.Y ?? 0;
    }
    
    public void SaveModel()
    {
        if (Model is null) return;
        
        Model.Code = Code;
        Model.NodeType = NodeType;
        Model.X = X;
        Model.Y = Y;
    }
}