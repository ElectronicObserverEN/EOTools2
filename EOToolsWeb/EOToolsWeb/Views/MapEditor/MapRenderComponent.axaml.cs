using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;

namespace EOToolsWeb.Views.MapEditor;

public class MapRenderComponent : Avalonia.Controls.Control
{
    public static readonly StyledProperty<ObservableCollection<MapElementModel>> ImagesProperty =
        AvaloniaProperty.Register<MapRenderComponent, ObservableCollection<MapElementModel>>(nameof(Images));

    public ObservableCollection<MapElementModel> Images
    {
        get => GetValue(ImagesProperty);
        set => SetValue(ImagesProperty, value);
    }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        if (change.Property == ImagesProperty)
        {
            var (oldValue, newValue) = change.GetOldAndNewValue<ObservableCollection<MapElementModel>>();
            oldValue?.CollectionChanged -= OnImageCollectionChanged;
            newValue?.CollectionChanged += OnImageCollectionChanged;
        }
        base.OnPropertyChanged(change);
    }

    private void OnImageCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (Images.FirstOrDefault()?.Image is {} image)
        {
            Height = image.Size.Height;
            Width = image.Size.Width;
        }
        
        InvalidateVisual();
    }


    public sealed override void Render(DrawingContext context)
    {
        bool first = true;
        
        foreach (MapElementModel image in Images)
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