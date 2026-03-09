using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;
using TS.Contract.DTOs;
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

}
