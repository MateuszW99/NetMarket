using System.Threading.Tasks;
using Application.Models.ApiModels.Transactions.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Authorize(Policy = "UserAccess")]
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TransactionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateTransaction([FromBody] InitializeTransactionCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}