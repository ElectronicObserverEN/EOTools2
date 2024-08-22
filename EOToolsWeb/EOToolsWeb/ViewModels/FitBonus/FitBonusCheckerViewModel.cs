using System.Threading.Tasks;
using EOToolsWeb.Control.Grid;

namespace EOToolsWeb.ViewModels.FitBonus;

public partial class FitBonusCheckerViewModel : ViewModelBase
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
    }

    public async Task Initialize()
    {
        await Pagination.Reload();
    }
}
