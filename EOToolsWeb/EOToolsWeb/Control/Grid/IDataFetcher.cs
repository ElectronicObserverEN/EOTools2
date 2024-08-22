using System.Threading.Tasks;

namespace EOToolsWeb.Control.Grid;

public interface IDataFetcher
{
    public Task<PaginatedResultModel<IGridRowFetched>?> LoadData(int skip, int take);
}
