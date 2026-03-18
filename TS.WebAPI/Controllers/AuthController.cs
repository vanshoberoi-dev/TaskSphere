using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TS.Contract.DTOs.Auth;
using TS.ServiceLogic.Interfaces;
using static TS.ServiceLogic.Common.Exceptions;

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
            catch (InvalidOperationException ex) // duplicate email
            {
                return Conflict(new { error = ex.Message }); // 409
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("login-user")]
        public async Task<IActionResult> LoginUser(LoginUserRequestDTO request)
        {
            try
            {
                return Ok(await _authService.LoginUserAsync(request));
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message }); // 401
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
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
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message }); // 404
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(); // 403
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}


