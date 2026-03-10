using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceLogic.DTOs.Task;
using System.Security.Claims;
using TS.Contract.DTOs.Task;

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

            catch (Exception ex) {
                return BadRequest($"Error Occurred : {ex.Message}");
            }
        }

        [HttpPost("assign-task")]
        public async Task<IActionResult> AssignTask(AssignTaskRequestDTO request)
        {
            try
            {
                var result = await _taskService.AssignTaskAsync(request);
                return Ok(result);
            }
            catch (Exception ex) {
                return BadRequest($"Error occured : {ex.Message}");
            }

        }


        [HttpGet("get-all-tasks")]
        public async Task<IActionResult> GetTasks() {
            try {
                return Ok(await _taskService.GetTasksAsync());
            }
            catch (Exception ex) { return BadRequest($"Error Occured : {ex.Message}"); }
        }


        [HttpGet("get-task/{id}")]
        public async Task<IActionResult> GetTaskByID(int id)
        {
            try
            {
                return Ok(await _taskService.GetTaskByIDAsync(id));
            }
            catch (Exception ex) {
                return BadRequest($"Error Occured : {ex.Message}");
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
            catch (Exception ex) {
                return BadRequest($"Error occured : {ex.Message}");
            }



        }
        [Authorize]
        [HttpDelete("delete-task")]
        public async Task<IActionResult> DeleteTask(DeleteTaskRequestDTO request)
        {
            try {
                var result = await _taskService.DeleteTaskAsync(request);
                return Ok("Task deleted successfully");
            }
            catch (Exception ex) {
                return BadRequest($"Error Occured : {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("update-task")]
        public async Task<IActionResult> UpdateTask(UpdateTaskRequestDTO request)
        {
            var result = await _taskService.UpdateTaskAsync(request);

            if (result == "Task not found")
                return NotFound(result);

            return Ok(result);
        }

    }
}