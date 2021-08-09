using System.Threading.Tasks;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<ActionResult<ItemObject>> GetItemById([FromQuery] string id)
        {
            var result = await _mediator.Send(new GetItemByIdQuery(id));
            return Ok(result);
        }
    }
}