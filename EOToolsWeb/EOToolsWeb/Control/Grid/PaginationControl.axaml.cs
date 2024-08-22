using Avalonia.Controls;

namespace EOToolsWeb.Control.Grid;

public partial class PaginationControl : UserControl
{
    public PaginationViewModel? ViewModel => DataContext as PaginationViewModel;

    public PaginationControl()
    {
        InitializeComponent();
    }
}