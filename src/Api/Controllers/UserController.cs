using System.Threading.Tasks;
using Application.Models.ApiModels.UserSettings.Commands;
using Application.Models.ApiModels.UserSettings.Queries;
using Application.Models.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = "User")]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<UserSettingsObject>> GetUserSettings()
        {
            var result = await _mediator.Send(new GetUserSettingsQuery());
            return Ok(result);
        }

        [HttpGet("level")]
        public async Task<ActionResult<string>> GetUserSellerLevel()
        {
            var result = await _mediator.Send(new GetUserSellerLevelQuery());
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserSettings([FromBody] UpdateUserSettingsCommand command)
        {
            await _mediator.Send(command);
            return Ok();
        }
    }
}