namespace Cybersec.Service.Extentions;
public static class PaginationExtention
{
    public static Task<PaginationViewModel<T>> ToPaginationAsync<T>(
       this IQueryable<T> query,
       PaginationParams @params
       ) where T : class
       => PaginationViewModel<T>.CreateAsync(query, @params);

    public static Task<PaginationViewModel<T>> ToPaginationAsync<T>(
        this IEnumerable<T> list,
         PaginationParams @params
        ) where T : class
        => list.AsQueryable().ToPaginationAsync(@params);
}
