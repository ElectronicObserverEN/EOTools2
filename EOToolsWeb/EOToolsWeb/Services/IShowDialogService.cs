using Avalonia.Platform.Storage;
using EOToolsWeb.ViewModels;
using System;
using System.Threading.Tasks;

namespace EOToolsWeb.Services
{
    public interface IShowDialogService<TWindowType>
    {
        public Task ShowMessage(string title, string message);
        public Task<bool> ShowConfirmPrompt(string title, string message);

        public Func<TWindowType, Task<bool>> ShowWindow { get; set; }
        public Func<MessageViewModel, Task> ShowDialog { get; set; }
        
        public Func<string?, Task<IStorageFolder?>> ShowFolderPicker { get; set; }

        public Task SaveFile(object? content, string extension);
    }
}