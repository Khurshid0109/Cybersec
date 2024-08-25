using Cybersec.Service.Helpers;

namespace Cybersec.Service.Extentions;
public class PaginationViewModel<T>
    where T : class
{
    public int ListCount => Items.Count();
    public int PageSize { get; set; }
    public long TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int MaxPageNumber => PageSize == 0 ? 0 : (int)Math.Ceiling((double)TotalCount / PageSize);
    public IEnumerable<T> Items { get; set; } = [];

    public static Task<PaginationViewModel<T>> CreateAsync(IQueryable<T> query, PaginationParams @params)
    {
        var list = query
            .Skip((@params.PageNumber - 1) * @params.PageSize)
            .Take(@params.PageSize);

        var pg = new PaginationViewModel<T>
        {
            PageNumber = @params.PageNumber,
            TotalCount = query.LongCount(),
            Items = [.. list],
            PageSize = @params.PageSize
        };
        return Task.FromResult(pg);
    }
}

public class PaginationParams
{
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value > 0 ? value : 1;
    }
    private int _pageNumber = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 0 ? value : Constants.PAGINATION_PAGE_SIZE;
    }
    private int _pageSize = Constants.PAGINATION_PAGE_SIZE;

}

