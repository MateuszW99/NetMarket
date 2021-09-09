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
    [Authorize(Policy = "SupervisorAccess")]
    [Route("api/[controller]")]
    public class SupervisorPanelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupervisorPanelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("orders")]
        public async Task<ActionResult<PaginatedList<TransactionObject>>> GetAssignedOrders(
            [FromQuery] SearchTransactionsQuery query)
        {
            var result = await _mediator.Send(new GetTransactionsQuery()
            {
                SearchTransactionsQuery = query
            });

            return Ok(result);
        }

        [HttpGet("orders/{id}")]
        public async Task<ActionResult<PaginatedList<TransactionObject>>> GetOrder(string id)
        {
            var result = await _mediator.Send(new GetTransactionByIdQuery()
            {
                Id = id
            });

            return Ok(result);
        }

        [HttpPut("orders/{id}")]
        public async Task<IActionResult> UpdateTransaction(string id,
            [FromBody] UpdateTransactionStatusCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}