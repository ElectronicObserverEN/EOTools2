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
        }
        
        base.OnPropertyChanged(change);
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
        
        foreach (MapElementModel image in ViewModel.MapImages)
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