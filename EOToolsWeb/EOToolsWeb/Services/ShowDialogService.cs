using Avalonia.Controls;
using EOToolsWeb.ViewModels;
using System;
using System.Threading.Tasks;
using Avalonia.Platform.Storage;

namespace EOToolsWeb.Services
{
    public class ShowDialogService : IAvaloniaShowDialogService
    {
        public async Task ShowMessage(string title, string message)
        {
            MessageViewModel vm = new()
            {
                Title = title,
                Message = message,
            };

            await ShowDialog(vm);
        }

        public Func<Window, Task<bool>> ShowWindow { get; set; } = _ =>
        {
            TaskCompletionSource<bool> result = new();
            result.SetResult(false);

            return result.Task;
        };

        public Func<MessageViewModel, Task> ShowDialog { get; set; } = _ => Task.CompletedTask;
        
        public Func<string?, Task<IStorageFolder?>> ShowFolderPicker { get; set; } = _ =>
        {
            TaskCompletionSource<IStorageFolder?> result = new();
            result.SetResult(null);

            return result.Task;
        };

        public Func<object?, string, Task> SaveFileImplementation { private get; set; } = (_, _) => Task.CompletedTask; 
        
        public Task SaveFile(object? content, string extension) => SaveFileImplementation(content, extension);
    }
}
