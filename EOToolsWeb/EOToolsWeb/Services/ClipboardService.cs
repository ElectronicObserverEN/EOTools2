using System;
using System.Threading.Tasks;

namespace EOToolsWeb.Services
{
    public class ClipboardService : IAvaloniaClipboardService
    {
        public Task CopyToClipboard(string content) => CopyToClipboardImplementation(content);

        public Func<string, Task> CopyToClipboardImplementation { private get; set; } = (_) => Task.CompletedTask; 
    }
}
