using System;
using System.Threading.Tasks;
using Avalonia.Controls;

namespace EOToolsWeb.Services
{
    public interface IAvaloniaShowDialogService : IShowDialogService<Window>
    {
        public Func<object?, string, Task> SaveFileImplementation { set; }
    }
}