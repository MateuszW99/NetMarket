using System.Threading.Tasks;
using Application.Common.Models;
using Application.Models.ApiModels.Bids.Commands;
using Application.Models.ApiModels.Bids.Queries;
using Application.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BidsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BidsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BidObject>> GetBidById(string id)
        {
            var result = await _mediator.Send(new GetBidByIdQuery() { Id = id });
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedList<BidObject>>> GetUserBids()
        {
            var result = await _mediator.Send(new GetUserBidsQuery());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "UserAccess")]
        public async Task<ActionResult> CreateBid([FromBody] CreateBidCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "UserAccess")]
        public async Task<ActionResult> UpdateBid(string id, [FromBody] UpdateBidCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        [Authorize(Policy = "UserAccess")]
        public async Task<ActionResult> DeleteBid(string id)
        {
            await _mediator.Send(new DeleteBidCommand(){ Id = id });
            return Ok();
        }
    }
}