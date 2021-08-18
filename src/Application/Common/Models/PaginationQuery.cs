namespace Application.Common.Models
{
    public abstract class PaginationQuery
    {
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}