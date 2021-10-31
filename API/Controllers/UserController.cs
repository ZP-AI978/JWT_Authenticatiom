using Buyer.Core.User;
using Domain.Entitities.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILoginService service;

        public UserController(ILoginService service)
        {
            this.service = service;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await service.Login(request);

            if (result == string.Empty)
                return Unauthorized();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await service.GetAll();

            if (result.Any())
                return Ok(result);

            return StatusCode(500);
        }
    }
}
