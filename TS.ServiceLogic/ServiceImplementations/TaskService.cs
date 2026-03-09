using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ServiceLogic.DTOs.Task;
using System.Security.Claims;
using TS.Contract.DTOs;
using TS.Contract.DTOs.Task;
using TS.Model.Data;
using TS.Model.Entities;
using TS.ServiceLogic.Interfaces;

public class TaskService : ITaskService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _context;

    public TaskService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
    {
        _httpContextAccessor = httpContextAccessor;
        _context = context;
    }
 
    public async Task<string> DeleteTaskAsync(DeleteTaskRequestDTO request)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId);

        if (task == null)
            return "Task not found";

        _context.Tasks.Remove(task);

        await _context.SaveChangesAsync();

        return $"Task {task.Id} Deleted Successfully";
    }

    public async Task<string> ChangeTaskStatusAsync(ChangeTaskStatusRequestDTO request)
    {
        var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);

        if (task == null)
            return "Task not found";

        task.Status = request.TaskStatus;

        await _context.SaveChangesAsync();

        return $"Task Status Updated to {task.Status}";
    }


    public async Task<string> UpdateTaskAsync(UpdateTaskRequestDTO request)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == request.TaskId);

        if (task == null)
            return "Task not found";

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;
        task.DueDate = DateTime.UtcNow.AddDays(request.DueInDays)

        await _context.SaveChangesAsync();

        return $"Task {task.Id} Updated Successfully";
    }

    public async Task<CreateTaskResponseDTO> CreateTaskAsync(CreateTaskRequestDTO request)
    {
        var user = _httpContextAccessor.HttpContext?.User;

        if (user == null || !user.Identity.IsAuthenticated)
            throw new UnauthorizedAccessException("User not authenticated.");

        var userIdClaim = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRoleClaim = user.FindFirst(ClaimTypes.Role)?.Value;

        if (userRoleClaim != "Admin")
            throw new UnauthorizedAccessException("Only admins can create tasks.");

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int adminId))
        {
            throw new UnauthorizedAccessException("Invalid user identity.");
        }

        var task = new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = DateTime.UtcNow.AddDays(request.DueInDays),
            CreatedByAdminId = adminId
        };

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        return new CreateTaskResponseDTO
        {
            message = $"Task {task.Title} with id {task.Id} created successfully"
        };
    }



    // vansh check
    public async Task<CreateTaskResponseDTO>  GetTaskDetailsAsync(int taskId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId);
        if (task == null)
            return new CreateTaskResponseDTO { message = "Task not found" };
        return new CreateTaskResponseDTO
        {
            message = $"Task Details: Id: {task.Id}, Title: {task.Title}, Description: {task.Description}, Status: {task.Status}, DueDate: {task.DueDate}"
        };
    }
}