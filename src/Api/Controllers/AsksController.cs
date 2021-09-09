using System.Threading.Tasks;
using Application.Common.Models;
using Application.Models.ApiModels.Asks.Commands;
using Application.Models.ApiModels.Asks.Queries;
using Application.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AsksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AsksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AskObject>> GetAskById(string id)
        {
            var result = await _mediator.Send(new GetAskByIdQuery() { Id = id });
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<AskObject>>> GetUserAsks()
        {
            var result = await _mediator.Send(new GetUserAsksQuery());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "UserAccess")]
        public async Task<ActionResult> CreateAsk([FromBody] CreateAskCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserAccess")]
        public async Task<ActionResult> UpdateAsk(string id, [FromBody] UpdateAskCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserAccess")]
        public async Task<ActionResult> DeleteAsk(string id)
        {
            await _mediator.Send(new DeleteAskCommand(){ Id = id });
            return Ok();
        }
    }
}