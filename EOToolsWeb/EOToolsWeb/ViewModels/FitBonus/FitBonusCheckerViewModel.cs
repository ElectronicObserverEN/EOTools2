using EOToolsWeb.Control.Grid;
using System.Threading.Tasks;

namespace EOToolsWeb.ViewModels.FitBonus;

public class FitBonusCheckerViewModel : ViewModelBase
{
    public FitBonusIssuesFetcher Fetcher { get; }

    public PaginationViewModel Pagination { get; }

    public FitBonusCheckerViewModel(FitBonusIssuesFetcher fetcher)
    {
        Fetcher = fetcher;

        Pagination = new PaginationViewModel()
        {
            Fetcher = Fetcher
        };

        Fetcher.PropertyChanged += async (_,e) => await OnSoftwareFilterChanged(e);
    }

    private async Task OnSoftwareFilterChanged(System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not nameof(Fetcher.SoftwareVersionFilter)) return;

        await Pagination.Reload();
    }

    public async Task Initialize()
    {
        await Pagination.Reload();
    }
}
