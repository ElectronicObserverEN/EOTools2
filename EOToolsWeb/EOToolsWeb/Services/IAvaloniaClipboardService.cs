using System;
using System.Threading.Tasks;

namespace EOToolsWeb.Services
{
    public interface IAvaloniaClipboardService : IClipboardService
    {
        public Func<string, Task> CopyToClipboardImplementation { set; }
    }
}