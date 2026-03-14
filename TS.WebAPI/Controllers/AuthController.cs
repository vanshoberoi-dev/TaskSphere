using Microsoft.AspNetCore.Authorization;
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
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = message });
            }

        }


        [HttpPost("register-user")]
        public async Task<IActionResult> RegisterUser(RegisterUserRequestDTO request)
        {
            try
            {
                var result = await _authService.RegisterUserAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = message });
            }

        }

        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser(LoginUserRequestDTO request)
        {
            try
            {
                return Ok(await _authService.LoginUserAsync(request));
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = message });
            }
        }

        [HttpGet("get-users")]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var result = await _authService.GetUsersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = message });
            }
        }

        [Authorize]
        [HttpPut("delete-user")]
        public async Task<IActionResult> DeleteUser(DeleteUserRequestDTO request)
        {
            try
            {
                var result = await _authService.DeleteUserAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException?.Message ?? ex.Message;
                return BadRequest(new { error = message });
            }
        }
    }
}
