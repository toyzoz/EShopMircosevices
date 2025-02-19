namespace BuildingBlocks.Pagination;

public class PaginatedResult<TEntity>(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
    where TEntity : class
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long Count { get; } = count;
    public IEnumerable<TEntity> Data { get; } = data;
}

public record PaginatedRequest(int PageIndex = 0, int PageSize = 10)
{
    public int PageIndex { get; } = PageIndex;
    public int PageSize { get; } = PageSize;
}