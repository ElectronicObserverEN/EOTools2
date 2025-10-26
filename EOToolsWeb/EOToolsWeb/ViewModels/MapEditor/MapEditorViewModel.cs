using System;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using CommunityToolkit.Mvvm.Input;
using EOToolsWeb.Models.MapEditor;
using EOToolsWeb.Models.MapEditor.Deserialization.MapInfo;
using EOToolsWeb.Models.MapEditor.Deserialization.SpriteSheet;
using EOToolsWeb.Services;
using EOToolsWeb.Shared.MapData;
using EOToolsWeb.Views.MapEditor;
using SkiaSharp;

namespace EOToolsWeb.ViewModels.MapEditor;

public partial class MapEditorViewModel : ViewModelBase
{
    public MapDisplayViewModel MapDisplayViewModel { get; }

    public string Error { get; set; } = "";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FolderButtonText))]
    public partial string? PathToMapFolder { get; set; }

    public string FolderButtonText => string.IsNullOrEmpty(PathToMapFolder) ? "Select a folder" : PathToMapFolder;

    [ObservableProperty] public partial List<string> MapList { get; set; } = [];

    [ObservableProperty] public partial string? SelectedMap { get; set; }
    [ObservableProperty] public partial bool IsMapNotLoading { get; set; } = true;

    [ObservableProperty] public partial List<string> AssetList { get; set; } = [];

    [ObservableProperty] public partial string? SelectedAsset { get; set; }

    [ObservableProperty] public partial bool DisplayAll { get; set; }
    
    private IAvaloniaShowDialogService DialogService { get; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DisplayPreviousNodeCommand))]
    [NotifyCanExecuteChangedFor(nameof(DisplayNextNodeCommand))]
    public partial NodeModel? SelectedNodeModel { get; set; } = new NodeModel();
    
    public bool SelectedNodeIsNotFirstNode => SelectedNodeModel is not null && NodeList.Count is not 0 && SelectedNodeModel.Number != NodeList.Min(node => node.Number);
    public bool SelectedNodeIsNotLastNode => SelectedNodeModel is not null && NodeList.Count is not 0 && SelectedNodeModel.Number != NodeList.Max(node => node.Number);

    public NodeViewModel SelectedNodeViewModel { get; set; } = new(new());
    
    [ObservableProperty] public partial List<NodeModel> NodeList { get; set; } = [];
    public List<NodeType> NodeTypes { get; set; } = Enum.GetValues<NodeType>().ToList();

    private int CurrentWorldId { get; set; }
    private int CurrentMapId { get; set; }
    
    private NodeDataManager NodeDataManager { get; }
    
    public MapEditorViewModel(IAvaloniaShowDialogService dialogService, NodeDataManager nodeManager, MapDisplayViewModel displayViewModel)
    {
        DialogService = dialogService;
        NodeDataManager = nodeManager;
        MapDisplayViewModel = displayViewModel;

        PropertyChanged += OnWorldPathChanged;
        PropertyChanged += OnSelectedMapChanged;

        PropertyChanged += OnSelectedAssetChanged;
        PropertyChanged += OnSelectedNodeChanged;
        PropertyChanged += OnDisplayAllChanged;
        
        SelectedNodeViewModel.PropertyChanged += OnSelectedNodeViewModelPropertyChanged;
    }

    private async void OnDisplayAllChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(DisplayAll)) return;
        
        await LoadMap();
    }

    private async void OnSelectedNodeViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedNodeViewModel.X) and not nameof(SelectedNodeViewModel.Y) and not nameof(SelectedNodeViewModel.NodeType)) return;
        
        if (SelectedNodeModel is null) return;
        
        await LoadMap();

        AddNodeToMap(SelectedNodeModel with
        {
            NodeType = SelectedNodeViewModel.NodeType,
            X = SelectedNodeViewModel.X,
            Y = SelectedNodeViewModel.Y,
        });
    }

    private async void OnSelectedNodeChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedNodeModel)) return;

        SelectedNodeViewModel.SaveModel();
        
        if (SelectedNodeViewModel.Model?.Id > 0)
        {
            await NodeDataManager.EditNode(SelectedNodeViewModel.Model);
        }
        
        SelectedNodeViewModel.Model = SelectedNodeModel;
        SelectedNodeViewModel.LoadFromModel();
    }

    public override async Task OnViewClosing()
    {
        if (SelectedNodeViewModel.Model?.Id > 0)
        {
            await NodeDataManager.EditNode(SelectedNodeViewModel.Model);
        }
        
        await base.OnViewClosing();
    }

    private void ClearMap()
    {
        foreach (IDisposable croppedBitmap in MapDisplayViewModel.GetImages())
        {
            croppedBitmap.Dispose();
        }

        MapDisplayViewModel.MapImages.Clear();
        MapDisplayViewModel.Paths.Clear();
    }

    private void OnSelectedAssetChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(SelectedAsset)) return;

        if (SelectedAsset is null) return;

        ClearMap();

        CroppedBitmap? crop = GetAssetBitmap(SelectedAsset);

        if (crop is null) return;

        MapDisplayViewModel.MapImages.Add(new MapElementModel(crop, 0, 0));
    }

    private async void OnWorldPathChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(PathToMapFolder))
        {
            await LoadWorldFolder();
        }
    }

    private async void OnSelectedMapChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName != nameof(SelectedMap)) return;
        
        IsMapNotLoading = false;
        
        try
        {
            if (string.IsNullOrEmpty(PathToMapFolder)) return;
            if (string.IsNullOrEmpty(SelectedMap)) return;

            await LoadMap();

            await UpdateNodesData();
        }
        finally
        {
            IsMapNotLoading = true;
        }
    }

    private async Task UpdateNodesData()
    {
        if (string.IsNullOrEmpty(PathToMapFolder)) return;
        
        string json = await File.ReadAllTextAsync(Path.Combine(PathToMapFolder, $"{SelectedMap}_info.json"));

        MapInfo? infos = JsonSerializer.Deserialize<MapInfo>(json);

        if (infos is null) return;
        
        // Get node data : 
        List<NodeModel>? nodes = await NodeDataManager.GetNodes(CurrentWorldId, CurrentMapId);
        
        if (nodes is null) return;

        bool anyChange = false;

        // A spot is actually an edge, hence the spot No is actually the edge Id
        foreach (Spot spot in infos.Spots)
        {
            NodeModel newNode = new NodeModel()
            {
                Code = spot.No?.ToString() ?? "0",
                X = spot.X,
                Y = spot.Y,
                Number = spot.No ?? 0,
                MapId = CurrentMapId,
                WorldId = CurrentWorldId,
            };

            if (spot.Offsets?.FirstOrDefault() is {} offset)
            {
                newNode.X += offset.Value.X;
                newNode.Y += offset.Value.Y;
            }
            
            if (nodes.All(node => node.X != newNode.X && node.Y != newNode.X))
            {
                await NodeDataManager.AddNode(newNode);

                anyChange = true;
            }
        }

        foreach (string key in GetPaths())
        {
            anyChange = await LoadPathNodeData(key) || anyChange;
        }

        if (anyChange)
        {
            NodeList = await NodeDataManager.GetNodes(CurrentWorldId, CurrentMapId) ?? [];
        }
        else
        {
            NodeList = nodes;
        }
    }

    private async Task LoadMap()
    {
        if (string.IsNullOrEmpty(PathToMapFolder)) return;
        if (string.IsNullOrEmpty(SelectedMap)) return;

        // read json
        string json = await File.ReadAllTextAsync(Path.Combine(PathToMapFolder, $"{SelectedMap}_image.json"));

        SpriteSheetModel? spriteSheet = JsonSerializer.Deserialize<SpriteSheetModel>(json);

        if (spriteSheet is null) return;

        // we assume that sprite map is 
        // map + world id (057) + map id (01) + _icon_ + enemy id = enemy sprite
        // map + world id (057) + map id (01) + _map + world id (57) + - + map id (1) = map cleared
        // map + world id (057) + map id (01) + _map + world id (57) + - + map id (1) + _point = map paths
        // map + world id (057) + map id (01) + _map + world id (57) + - + map id (1) + _red = map not cleared
        // map + world id (057) + map id (01) + _route_ + edge id ? (11) = path reached overlay

        string worldId = spriteSheet.Frames.Keys.First()[4..6];
        string mapId = SelectedMap[1..2];

        CurrentWorldId = int.Parse(worldId);
        CurrentMapId = int.Parse(mapId);

        // load map
        ClearMap();

        LoadMap(worldId, mapId, spriteSheet);
        LoadMapMainPath(worldId, mapId, spriteSheet);
        
        // Load other parts :
        await LoadParts(worldId, mapId);

        if (DisplayAll)
        {
            foreach (NodeModel node in NodeList)
            {
                AddNodeToMap(node);
            }
        }
    }

    private async Task LoadParts(string worldId, string mapId)
    {
        foreach (string infoJsonFile in GetPaths())
        {
            await LoadOnePath(infoJsonFile, worldId, mapId);
        }
    }

    private List<string> GetPaths()
    {
        if (string.IsNullOrEmpty(PathToMapFolder)) return [];
        if (string.IsNullOrEmpty(SelectedMap)) return [];
        
        string[] files = Directory.GetFiles(PathToMapFolder);

        Regex regex = new Regex($"{SelectedMap}_info([0-9]+).json");

        return files
            .Where(path => regex.IsMatch(path))
            .Select(path => regex.Match(path).Groups[1].Value)
            .ToList();
    }

    private async Task LoadOnePath(string key, string worldId, string mapId)
    {
        if (string.IsNullOrEmpty(PathToMapFolder)) return;

        string json = await File.ReadAllTextAsync(Path.Combine(PathToMapFolder, $"{SelectedMap}_info{key}.json"));

        MapInfo? infos = JsonSerializer.Deserialize<MapInfo>(json);

        if (infos is null) return;

        json = await File.ReadAllTextAsync(Path.Combine(PathToMapFolder, $"{SelectedMap}_image{key}.json"));

        SpriteSheetModel? spriteSheet = JsonSerializer.Deserialize<SpriteSheetModel>(json);

        if (spriteSheet is null) return;

        Bitmap image = new(Path.Combine(PathToMapFolder, $"{SelectedMap}_image{key}.png"));

        PathDisplayViewModel vm = new();

        // Add labels
        foreach (Label label in infos.Labels)
        {
            FrameModel frame = spriteSheet.Frames[$"map0{worldId}0{mapId}_{label.Image}"];

            Rect rect = new Rect(frame.FrameDefinitionModel.X, frame.FrameDefinitionModel.Y,
                frame.FrameDefinitionModel.Width, frame.FrameDefinitionModel.Height);

            CroppedBitmap crop = new CroppedBitmap(image, PixelRect.FromRect(rect, 1));

            MapDisplayViewModel.MapImages.Add(new MapElementModel(crop, label.X, label.Y));
        }

        // Add nodes : 
        foreach (Spot spot in infos.Spots)
        {
            // Don't display current node
            NodeModel? node = NodeList.FirstOrDefault(node => node.Number == spot.No);

            if (SelectedNodeModel != node)
            {
                if (node is null)
                {
                    node = new NodeModel()
                    {
                        Code = spot.No?.ToString() ?? "0",
                        X = spot.X,
                        Y = spot.Y,
                        Number = spot.No ?? 0,
                        MapId = CurrentMapId,
                        WorldId = CurrentWorldId,
                    };

                    if (spot.Offsets?.FirstOrDefault() is {} offset)
                    {
                        node.X += offset.Value.X;
                        node.Y += offset.Value.Y;
                    }
                }

                if (!DisplayAll)
                {
                    AddNodeToMap(node with { NodeType = NodeType.NotVisited });
                }
            }
        }

        // Add paths : 
        foreach (Spot spot in infos.Spots.Where(s => s.Route?.Image is not null && s.Line is not null))
        {
            FrameModel frame = spriteSheet.Frames[$"map0{worldId}0{mapId}_{spot.Route?.Image}"];
            
            Rect rect = new Rect(frame.FrameDefinitionModel.X, frame.FrameDefinitionModel.Y,
                frame.FrameDefinitionModel.Width, frame.FrameDefinitionModel.Height);

            CroppedBitmap crop = new CroppedBitmap(image, PixelRect.FromRect(rect, 1));

            MapDisplayViewModel.MapImages.Add(new MapElementModel(crop, spot.X + spot.Line!.X, spot.Y + spot.Line!.Y));
        }
    }

    private void AddNodeToMap(NodeModel node)
    {
        CroppedBitmap? crop = GetAssetBitmap(node.NodeType);

        // if node is sub/air attack, apply a +8 offset on X
        int offsetX = node.NodeType switch
        {
            NodeType.AirSubNode => 8,
            _ => 0,
        };
            
        if (crop is not null)
        {
            MapDisplayViewModel.MapImages.Add(new MapElementModel(crop, node.X - (crop.Size.Width / 2) + offsetX,
                node.Y - (crop.Size.Height / 2)));
        }
    }

    private async Task<bool> LoadPathNodeData(string pathKey)
    {
        if (string.IsNullOrEmpty(PathToMapFolder)) return false;

        string json = await File.ReadAllTextAsync(Path.Combine(PathToMapFolder, $"{SelectedMap}_info{pathKey}.json"));

        MapInfo? infos = JsonSerializer.Deserialize<MapInfo>(json);
        
        if (infos is null) return false;
        
        // Update Node Infos 
        List<NodeModel>? nodes = await NodeDataManager.GetNodes(CurrentWorldId, CurrentMapId);
        
        if (nodes is null) return false;
        
        bool anyChange = false;

        foreach (Spot spot in infos.Spots)
        {
            NodeModel newNode = new NodeModel()
            {
                Code = spot.No?.ToString() ?? "0",
                X = spot.X,
                Y = spot.Y,
                Number = spot.No ?? 0,
                MapId = CurrentMapId,
                WorldId = CurrentWorldId,
            };
            
            if (spot.Offsets?.FirstOrDefault() is {} offset)
            {
                newNode.X += offset.Value.X;
                newNode.Y += offset.Value.Y;
            }
            
            if (nodes.All(node => node.X != newNode.X && node.Y != newNode.X))
            {
                await NodeDataManager.AddNode(newNode);

                anyChange = true;
            }
        }

        return anyChange;
    }

    private void LoadMap(string worldId, string mapId, SpriteSheetModel spriteSheet)
    {
        if (string.IsNullOrEmpty(PathToMapFolder)) return;

        Bitmap image = new(Path.Combine(PathToMapFolder, $"{SelectedMap}_image.png"));

        FrameModel mapFrame = spriteSheet.Frames[$"map0{worldId}0{mapId}_map{worldId}-{mapId}_red"];

        Rect rect = new Rect(mapFrame.FrameDefinitionModel.X, mapFrame.FrameDefinitionModel.Y,
            mapFrame.FrameDefinitionModel.Width, mapFrame.FrameDefinitionModel.Height);

        CroppedBitmap crop = new CroppedBitmap(image, PixelRect.FromRect(rect, 1));

        MapDisplayViewModel.MapImages.Add(new MapElementModel(crop, 0, 0));

        MapDisplayViewModel.ExportHeight = mapFrame.FrameDefinitionModel.Height;
        MapDisplayViewModel.ExportWidth = mapFrame.FrameDefinitionModel.Width;
    }

    private CroppedBitmap? GetAssetBitmap(NodeType assetIndex)
    {
        return GetAssetBitmap($"map_main_{GetAssetIdFromNodeType(assetIndex)}");
    }

    private CroppedBitmap? GetAssetBitmap(string assetKey)
    {
        string json = System.Text.Encoding.UTF8.GetString(Assets.MapEditor.MapAssetsMapping);

        SpriteSheetModel? spriteSheet = JsonSerializer.Deserialize<SpriteSheetModel>(json);

        if (spriteSheet is null) return null;

        Bitmap assets = new Bitmap(new MemoryStream(Assets.MapEditor.MapAssets));

        FrameModel frame = spriteSheet.Frames[assetKey];

        Rect rect = new Rect(frame.FrameDefinitionModel.X, frame.FrameDefinitionModel.Y,
            frame.FrameDefinitionModel.Width, frame.FrameDefinitionModel.Height);

        return new CroppedBitmap(assets, PixelRect.FromRect(rect, 1));
    }

    private void LoadMapMainPath(string worldId, string mapId, SpriteSheetModel spriteSheet)
    {
        if (string.IsNullOrEmpty(PathToMapFolder)) return;

        Bitmap image = new(Path.Combine(PathToMapFolder, $"{SelectedMap}_image.png"));

        FrameModel mapFrame = spriteSheet.Frames[$"map0{worldId}0{mapId}_map{worldId}-{mapId}_point"];

        Rect rect = new Rect(mapFrame.FrameDefinitionModel.X, mapFrame.FrameDefinitionModel.Y,
            mapFrame.FrameDefinitionModel.Width, mapFrame.FrameDefinitionModel.Height);

        CroppedBitmap crop = new CroppedBitmap(image, PixelRect.FromRect(rect, 1));

        MapDisplayViewModel.MapImages.Add(new MapElementModel(crop, 0, 0));
    }

    private int GetAssetIdFromNodeType(NodeType nodeType) => nodeType switch
    {
        NodeType.AirSubNode => 18,
        NodeType.Ambush => 17,
        NodeType.AirRaid => 46,
        NodeType.Anchor => 52,
        NodeType.Start1 => 58,
        NodeType.Start2 => 59,
        NodeType.Empty => 60,
        NodeType.BossBattle => 62,
        NodeType.Resource => 63,
        NodeType.Storm => 65,
        NodeType.Battle => 66,
        NodeType.NotVisited => 67,
        _ => 67,
    };
    
    private async Task LoadWorldFolder()
    {
        if (string.IsNullOrEmpty(PathToMapFolder) || !Directory.Exists(PathToMapFolder))
        {
            Error = "Invalid map folder path.";
            return;
        }

        IEnumerable<string> maps = Directory.EnumerateFiles(PathToMapFolder);

        MapList = maps
            .Select(file => new FileInfo(file))
            .Select(file => file.Name[..2])
            .Distinct()
            .ToList();
    }

    [RelayCommand]
    private async Task OpenFolderPicker()
    {
        if (await DialogService.ShowFolderPicker(PathToMapFolder) is not { } folder) return;

        PathToMapFolder = folder.TryGetLocalPath() ?? "";
    }

    [RelayCommand]
    private void DisplayIconMapping()
    {
        string json = System.Text.Encoding.UTF8.GetString(Assets.MapEditor.MapAssetsMapping);

        SpriteSheetModel? spriteSheet = JsonSerializer.Deserialize<SpriteSheetModel>(json);

        if (spriteSheet is null) return;

        AssetList = spriteSheet.Frames.Keys.ToList();
    }

    [RelayCommand(CanExecute = nameof(SelectedNodeIsNotFirstNode))]
    private void DisplayPreviousNode()
    {
        if (NodeList.Count is 0) return;
        if (SelectedNodeModel is not { } selectedNode) return;

        int nodeNo = NodeList.IndexOf(selectedNode);

        if (nodeNo == -1) return;

        SelectedNodeModel = NodeList[nodeNo - 1];
    }

    [RelayCommand(CanExecute = nameof(SelectedNodeIsNotLastNode))]
    private void DisplayNextNode()
    {
        if (NodeList.Count is 0) return;
        if (SelectedNodeModel is not {} selectedNode) return;
        
        int nodeNo = NodeList.IndexOf(selectedNode);

        if (nodeNo == -1) return;

        SelectedNodeModel = NodeList[nodeNo + 1];
    }
    
    [RelayCommand]
    private async Task ExportMap()
    {
        try
        {
            using MemoryStream stream = MapDisplayViewModel.GetImageMerged();
            
            await DialogService.SaveFile(stream, "png");
        }
        catch (Exception ex)
        {
            // nothing, ignore
        }
    }
}