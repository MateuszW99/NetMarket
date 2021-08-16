namespace Application.Common.Models
{
    public class SearchItemsQuery
    {
        public string Brand { get; init; }
        public string Category { get; init; }
        public string Make { get; init; }
        public string Name { get; init; }
        public string Model { get; init; }
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}