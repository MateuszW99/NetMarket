using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Common;
using Application.Common.Models;
using Application.Models.ApiModels.Items.Commands;
using Application.Models.ApiModels.Items.Queries;
using Application.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemObject>> GetItemById(string id)
        {
            var result = await _mediator.Send(new GetItemByIdQuery(id));
            return Ok(result);
        }
        
        [HttpGet]
        public async Task<ActionResult<List<ItemObject>>> GetItems([FromQuery] SearchItemsQuery query, [FromBody] PaginationData paginationData)
        {
            var result = await _mediator.Send(new GetItemsQuery()
            {
                SearchQuery =  query,
                PageIndex = paginationData.PageIndex, 
                PageSize = paginationData.PageSize
            });
            
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = "AdminAccess")]
        public async Task<ActionResult> CreateItem([FromBody] CreateItemCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
        
        [HttpPut("{id}")]
        [Authorize(Policy = "AdminAccess")]
        public async Task<ActionResult> CreateItem(string id, [FromBody] UpdateItemCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}