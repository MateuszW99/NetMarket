namespace Application.Common.Models
{
    public class ItemsWithCategoryQuery : PaginationQuery
    {
        public string Category { get; set; }
    }
}