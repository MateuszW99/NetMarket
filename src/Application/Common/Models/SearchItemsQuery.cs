namespace Application.Common.Models
{
    public class SearchItemsQuery : PaginationQuery
    {
        public string Brand { get; init; }
        public string Category { get; init; }
        public string Make { get; init; }
        public string Name { get; init; }
        public string Model { get; init; }
        public string Gender { get; init; }
        public string MinPrice { get; set; }
        public string MaxPrice { get; set; }
    }
}