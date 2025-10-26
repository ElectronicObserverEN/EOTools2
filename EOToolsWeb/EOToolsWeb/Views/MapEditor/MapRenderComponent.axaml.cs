using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using EOToolsWeb.ViewModels.MapEditor;

namespace EOToolsWeb.Views.MapEditor;

public class MapRenderComponent : Avalonia.Controls.Control
{
    public static readonly StyledProperty<MapDisplayViewModel> ViewModelProperty =
        AvaloniaProperty.Register<MapRenderComponent, MapDisplayViewModel>(nameof(ViewModel));

    public MapDisplayViewModel ViewModel
    {
        get => GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == ViewModelProperty)
        {
            var (oldValue, newValue) = change.GetOldAndNewValue<MapDisplayViewModel>();
            oldValue?.MapImages?.CollectionChanged -= OnImageCollectionChanged;
            newValue?.MapImages?.CollectionChanged += OnImageCollectionChanged;
            
            oldValue?.Paths?.CollectionChanged -= OnPathCollectionChanged;
            newValue?.Paths?.CollectionChanged += OnPathCollectionChanged;
            
            oldValue?.PropertyChanged -= OnMapVisibleChanged;
            newValue?.PropertyChanged += OnMapVisibleChanged;

            foreach (PathDisplayViewModel vm in oldValue?.Paths ?? [])
            {
                vm.PropertyChanged -= OnPathShownChanged;
            }
        }
        
        base.OnPropertyChanged(change);
    }

    private void OnMapVisibleChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(ViewModel.DisplayMap)) return;
        
        InvalidateVisual();
    }

    private void OnPathCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.Action is NotifyCollectionChangedAction.Add)
        {
            foreach (PathDisplayViewModel vm in e.NewItems)
            {
                vm.PropertyChanged += OnPathShownChanged;
            }
        }
        else if (e.Action is NotifyCollectionChangedAction.Remove)
        {
            foreach (PathDisplayViewModel vm in e.OldItems)
            {
                vm.PropertyChanged -= OnPathShownChanged;
            }
        }
        
        InvalidateVisual();
    }

    private void OnPathShownChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(PathDisplayViewModel.Shown)) return;
        
        InvalidateVisual();
    }

    private void OnImageCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (ViewModel.MapImages.FirstOrDefault()?.Image is {} image)
        {
            Height = image.Size.Height;
            Width = image.Size.Width;
        }
        
        InvalidateVisual();
    }
    
    public sealed override void Render(DrawingContext context)
    {
        bool first = true;
        
        foreach (MapElementModel image in ViewModel.GetElementsToDisplay())
        {
            if (first)
            {
                context.DrawImage(image.Image, new Rect(image.X, image.Y, image.Image.Size.Width, image.Image.Size.Height));
                
                first = false;
            }
            else
            {
                using (context.PushRenderOptions(new RenderOptions() with { BitmapBlendingMode = BitmapBlendingMode.SourceOver }))
                {
                    context.DrawImage(image.Image, new Rect(image.X, image.Y, image.Image.Size.Width, image.Image.Size.Height));
                }
            }
        }
        
        base.Render(context);
    }
}