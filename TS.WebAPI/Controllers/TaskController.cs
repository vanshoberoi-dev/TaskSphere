using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TS.Contract.DTOs.Task;
using TS.ServiceLogic.Interfaces;

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
                return BadRequest($"Error Occurred : {ex.InnerException}");
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
                return BadRequest($"Error occured : {ex.InnerException}");
            }

        }


        [HttpGet("get-all-tasks")]
        public async Task<IActionResult> GetTasks() {
            try {
                return Ok(await _taskService.GetTasksAsync());
            }
            catch (Exception ex) { return BadRequest($"Error Occured : {ex.InnerException}"); }
        }


        [HttpGet("get-task/{id}")]
        public async Task<IActionResult> GetTaskByID(int id)
        {
            try
            {
                return Ok(await _taskService.GetTaskByIDAsync(id));
            }
            catch (Exception ex) {
                return BadRequest($"Error Occured : {ex.InnerException}");
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
                return BadRequest($"Error occured : {ex.InnerException}");
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
                return BadRequest($"Error Occured : {ex.InnerException}");
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
            catch (Exception ex) { 
                    return BadRequest($"Error Occured : {ex.InnerException}");
            }
        }

    }
}