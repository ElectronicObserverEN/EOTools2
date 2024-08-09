using EOToolsWeb.ViewModels;
using ReactiveUI;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace EOToolsWeb.Services
{
    public class ShowDialogService
    {
        public Interaction<MessageViewModel, object?> ShowDialog { get; } = new();
        
        public async Task ShowMessage(string title, string message)
        {
            MessageViewModel vm = new()
            {
                Title = title,
                Message = message,
            };

            await ShowDialog.Handle(vm);
        }
    }
}
