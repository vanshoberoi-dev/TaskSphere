using Microsoft.AspNetCore.Mvc;
using TS.Contract.DTOs.Auth;
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

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(CreateRoleRequestDTO request)
        {
            try
            {
                var result = await _authService.CreateRoleAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequestDTO request)
        {
            try {
                var result = await _authService.RegisterUserAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser(LoginUserRequestDTO request)
        {
            try {
                return Ok(await _authService.LoginUserAsync(request));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}