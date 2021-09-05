namespace Application.Common.Models
{
    public class SearchTransactionsQuery : PaginationQuery
    {
        public string Status { get; set; }
    }
}