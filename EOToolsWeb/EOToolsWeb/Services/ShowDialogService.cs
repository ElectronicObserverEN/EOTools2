﻿using Avalonia.Controls;
using EOToolsWeb.ViewModels;
using System;
using System.Threading.Tasks;

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
    }
}
