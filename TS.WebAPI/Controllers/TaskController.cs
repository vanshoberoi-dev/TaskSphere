using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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