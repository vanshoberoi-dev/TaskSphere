using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TS.Contract.DTOs.Task;
using TS.ServiceLogic.Interfaces;
using static TS.ServiceLogic.Common.Exceptions;

namespace TS.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }


        [Authorize]
        [HttpPost("create-task")]
        public async Task<IActionResult> CreateTask(CreateTaskRequestDTO request)
        {
            try
            {
                var result = await _taskService.CreateTaskAsync(request);
                return Ok(result);
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

        [Authorize]
        [HttpPost("assign-task")]
        public async Task<IActionResult> AssignTask(AssignTaskRequestDTO request)
        {
            try
            {
                var result = await _taskService.AssignTaskAsync(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // 403 (only admin allowed)
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message }); // task/user not found
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpGet("get-all-tasks")]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                return Ok(await _taskService.GetTasksAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }



        [HttpGet("get-task/{TaskId}")]
        public async Task<IActionResult> GetTaskByID(int TaskId)
        {
            try
            {
                return Ok(await _taskService.GetTaskByIDAsync(TaskId));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message }); // 404
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize]
        [HttpPatch("change-task-status")]
        public async Task<IActionResult> ChangeTaskStatus(ChangeTaskStatusRequestDTO request)
        {
            try
            {
                var result = await _taskService.ChangeTaskStatusAsync(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(); // 403 (not creator)
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("delete-task")]
        public async Task<IActionResult> DeleteTask(DeleteTaskRequestDTO request)
        {
            try
            {
                var result = await _taskService.DeleteTaskAsync(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // admin only
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [Authorize]
        [HttpPut("update-task")]
        public async Task<IActionResult> UpdateTask(UpdateTaskRequestDTO request)
        {
            try
            {
                var result = await _taskService.UpdateTaskAsync(request);
                return Ok(result);
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid(); // admin only
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}