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
    [Authorize(Policy = "UserAccess")]
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
        public async Task<ActionResult<PaginatedList<AskObject>>> GetUserAsks([FromQuery] SearchAsksQuery query)
        {
            var result = await _mediator.Send(new GetUserAsksQuery()
            {
                PageIndex = query.PageIndex, 
                PageSize = query.PageSize
            });
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsk([FromBody] CreateAskCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateAsk(string id, [FromBody] UpdateAskCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAsk(string id)
        {
            await _mediator.Send(new DeleteAskCommand(){ Id = id });
            return Ok();
        }
    }
}