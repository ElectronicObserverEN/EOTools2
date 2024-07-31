using EOToolsWeb.Shared.UseItem;
using ReactiveUI;

namespace EOToolsWeb.ViewModels.UseItem;

public class UseItemManagerViewModel
{
    public Interaction<object?, UseItemId?> ShowPicker { get; } = new();
}
