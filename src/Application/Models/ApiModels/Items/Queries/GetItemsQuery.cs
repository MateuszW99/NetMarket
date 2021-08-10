using Application.Common.Models;
using Application.Models.DTOs;
using MediatR;

namespace Application.Models.ApiModels.Items.Queries
{
    public class GetItemsQuery : IRequest<PaginatedList<ItemObject>>
    {
        public SearchItemsQuery SearchQuery { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}