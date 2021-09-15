using System.Threading.Tasks;
using Application.Common.Models;
using Application.Models.ApiModels.Transactions.Commands;
using Application.Models.ApiModels.Transactions.Queries;
using Application.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    
    [ApiController]
    [Authorize(Policy = "AdminAccess")]
    [Route("api/[controller]")]
    public class AdminPanelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminPanelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("orders")]
        public async Task<ActionResult<PaginatedList<TransactionObject>>> GetOrders(
            [FromQuery] SearchTransactionsQuery query)
        {
            var result = await _mediator.Send(new GetTransactionsQuery()
            {
                SearchTransactionsQuery = query
            });

            return Ok(result);
        }

        [HttpPut("orders/{id}")]
        public async Task<IActionResult> UpdateTransaction(string id,
            [FromBody] UpdateTransactionCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}