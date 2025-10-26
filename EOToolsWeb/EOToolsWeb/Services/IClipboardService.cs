using System.Threading.Tasks;

namespace EOToolsWeb.Services
{
    public interface IClipboardService
    {
        public Task CopyToClipboard(string content);
    }
}