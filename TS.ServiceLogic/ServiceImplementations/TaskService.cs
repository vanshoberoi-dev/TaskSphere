using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        var user = _httpContextAccessor.HttpContext.User;

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier).Value;

        int userId = int.Parse(userIdClaim);

        if (userId != 1)
        {
            throw new UnauthorizedAccessException("User is not authorized");
        }

        var userEmailClaim = user.FindFirst(ClaimTypes.Email).Value;

        var task = new TaskEntity
        {
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            DueDate = request.DueDate,
            CreatedByAdminEmail = userEmailClaim
        };

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();

        var response = new CreateTaskResponseDTO
        {
            message = $"Task {task.Title} with id {task.Id} created successfully"
        };

        return response;
    }
}