using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Identity.Requests;
using Application.Identity.Responses;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class IdentityController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]UserRegistrationRequest request)
        {
            var registrationResponse = await _identityService.RegisterAsync(request.Email, request.UserName, request.Password);

            if (!registrationResponse.Success)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    ErrorMessages = registrationResponse.ErrorMessages
                });
            }
            
            return Ok(new AuthSuccessResponse()
            {
                Token = registrationResponse.Token
            });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var loginResponse = await _identityService.LoginAsync(request.Email, request.Password);

            if (!loginResponse.Success)
            {
                return BadRequest(new AuthFailedResponse()
                {
                    ErrorMessages = loginResponse.ErrorMessages
                });
            }
            
            return Ok(new AuthSuccessResponse()
            {
                Token = loginResponse.Token
            });
        }


    }
}