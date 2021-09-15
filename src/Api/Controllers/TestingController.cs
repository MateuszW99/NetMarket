using System.Threading.Tasks;
using Application.Common.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;

namespace Api.Controllers
{
    // Development purpose only
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class TestingController : ControllerBase
    {
        private readonly IHttpService _httpService;

        public TestingController(IHttpService httpService)
        {
            _httpService = httpService;
        }

        [HttpGet("get")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Get()
        {
            var userId = _httpService.GetUserId();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest();
            }

            return Ok(new {Id = userId, Role = HttpContext.User.IsInRole(Roles.User)});
        }
    }
}