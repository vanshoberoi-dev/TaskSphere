using Microsoft.AspNetCore.Mvc;
using TS.Contract;
using TS.ServiceLogic.Interfaces;

namespace TS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDTO dto)
        {
            var result = await _authService.RegisterAsync(dto);
            return Ok(result);
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(CreateRoleRequestDTO dto)
        {
            var result = await _authService.CreateRoleAsync(dto);
            return Ok(result);
        }
    }
}