using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TS.Contract.DTOs.Task;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]   
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }


    [Authorize]
    [HttpPost("create-task")]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequestDTO request)
    {
        var result = await _taskService.CreateTaskAsync(request);
        return Ok(result);
    }

    [Authorize]
    [HttpPatch("update-task-status")]
    public async Task<IActionResult> UpdateTaskStatus([FromBody] UpdateTaskStatusRequestDTO request)
    {
        var result = await _taskService.UpdateTaskStatusAsync(request);

        if (!result)
            return NotFound("Task not found");

        return Ok(result);
    }
    [Authorize]
    [HttpDelete("delete-task")]
    public async Task<IActionResult> DeleteTask([FromBody] DeleteTaskRequestDTO request)
    {
        var result = await _taskService.DeleteTaskAsync(request);

        if (!result)
            return NotFound("Task not found");

        return Ok("Task deleted successfully");
    }




    //[HttpPost("assign-task")]
    //public async Task<IActionResult> AssignTask([FromBody] AssignTaskRequestDTO dto)
    //{
    //    if (!ModelState.IsValid)
    //        return BadRequest(ModelState);

    //    var result = await _taskService.AssignTaskAsync(dto);
    //    return Ok(result);
    //}

    //[HttpGet("get-tasks")]
    //public async Task<IActionResult> GetAllTasks()
    //{
    //    var tasks = await _taskService.GetAllTasksAsync();
    //    return Ok(tasks);
    //}
}