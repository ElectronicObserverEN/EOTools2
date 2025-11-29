using Avalonia.Controls;
using EOToolsWeb.ViewModels;
using System;
using System.Threading.Tasks;

namespace EOToolsWeb.Services
{
    public interface IAvaloniaShowDialogService : IShowDialogService<Window>
    {
        public Func<object?, string, Task> SaveFileImplementation { set; }
        public Func<MessageViewModel, Task<bool>> ShowConfirmPromptImplementation { set; }
    }
}