using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Identity.Requests;
using Application.Identity.Responses;
using Application.Models.ApiModels.Supervisors.Queries;
using Application.Models.ApiModels.Transactions.Commands;
using Application.Models.ApiModels.Transactions.Queries;
using Application.Models.DTOs;
using Domain;
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
        private readonly IIdentityService _identityService;

        public AdminPanelController(IMediator mediator, IIdentityService identityService)
        {
            _mediator = mediator;
            _identityService = identityService;
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
        
        [HttpGet("supervisors")]
        public async Task<IActionResult> GetSupervisors([FromQuery]GetSupervisorsQuery query)
        {
            
            var result = await _mediator.Send(new GetSupervisorsQuery()
            {
                PageIndex = query.PageIndex,
                PageSize = query.PageSize,
                SearchQuery = query.SearchQuery
            });

            return Ok(result);
        }
        
        [HttpPost("supervisors")]
        public async Task<IActionResult> AddSupervisor([FromBody]UserRegistrationRequest request)
        {
            var registrationResponse = await _identityService.RegisterAsync(request.Email, request.UserName, request.Password, Roles.Supervisor);

            if (!registrationResponse.Success)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    ErrorMessages = registrationResponse.ErrorMessages
                });
            }
            
            return Ok();
        }
        
        [HttpDelete("supervisors/{id}")]
        public async Task<IActionResult> DeleteSupervisor(string id)
        {
            var deleteResponse = await _identityService.DeleteUserAsync(id);

            if (!deleteResponse.Success)
            {
                return BadRequest(deleteResponse);
            }
            
            return Ok(deleteResponse);
        }
    }
}