using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TS.Contract.DTOs.Task;
using TS.Model.Data;
using TS.ServiceLogic.ServiceInterfaces;

namespace TS.ServiceLogic.ServiceImplementations
{
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

        public async Task<string> AssignTaskAsync(AssignTaskRequestDTO request)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User not authenticated.");
            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoleClaim = user.FindFirst(ClaimTypes.Role)?.Value;
            if (userRoleClaim != "Admin")
                throw new UnauthorizedAccessException("Only admins can assign tasks.");

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);
            if (task == null)
                return "Task not found";
            var assignee = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.AssigneeEmail);
            if (assignee == null)
                return "Assignee not found";

            task.AssigneeId = assignee.Id;

            await _context.SaveChangesAsync();
            return $"Task '{task.Title}' with ID '{task.Id}' assigned to User with email '{assignee.Email}' successfully";

        }



        public async Task<string> ChangeTaskStatusAsync(ChangeTaskStatusRequestDTO request)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User not authenticated.");

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoleClaim = user.FindFirst(ClaimTypes.Role)?.Value;

            if (userRoleClaim != "Admin")
                throw new UnauthorizedAccessException("Only admins can change task status.");

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out _))
                throw new UnauthorizedAccessException("Invalid user identity.");

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {request.TaskId} was not found.");

            task.Status = request.TaskStatus;

            await _context.SaveChangesAsync();

            return $"Task Status Updated to {task.Status}";
        }

        public async Task<IEnumerable<GetTaskResponseDTO>> GetTasksAsync()
        {
            var tasks = await _context.Tasks
                .AsNoTracking()
                .Select(t => new GetTaskResponseDTO
                {
                    TaskId = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    AssigneeEmail = t.Assignee != null ? t.Assignee.Email : null,
                    Status = t.Status,
                    DueDate = t.DueDate,
                    Remarks = t.Remarks,
                    CreatedByAdminId = t.CreatedByAdminId,
                    CreatedOn = t.CreatedOn
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<GetTaskResponseDTO> GetTaskByIDAsync(int taskId)
        {
            var response = await _context.Tasks
                .AsNoTracking()
                .Where(t => t.Id == taskId)
                .Select(t => new GetTaskResponseDTO
                {
                    TaskId = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    AssigneeEmail = t.Assignee != null ? t.Assignee.Email : null,
                    Status = t.Status,
                    DueDate = t.DueDate,
                    CreatedByAdminId = t.CreatedByAdminId,
                    CreatedOn = t.CreatedOn
                })
                .FirstOrDefaultAsync();

            if (response == null)
                throw new KeyNotFoundException($"Task with ID {taskId} was not found.");

            return response;
        }


        public async Task<string> DeleteTaskAsync(DeleteTaskRequestDTO request)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User not authenticated.");

            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoleClaim = user.FindFirst(ClaimTypes.Role)?.Value;

            if (userRoleClaim != "Admin")
                throw new UnauthorizedAccessException("Only admins can delete tasks.");

            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out _))
                throw new UnauthorizedAccessException("Invalid user identity.");

            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == request.TaskId);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {request.TaskId} was not found.");

            _context.Tasks.Remove(task);

            await _context.SaveChangesAsync();

            return $"Task {task.Id} Deleted Successfully";
        }


        public async Task<string> UpdateTaskAsync(UpdateTaskRequestDTO request)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User not authenticated.");


            var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userRoleClaim = user.FindFirst(ClaimTypes.Role)?.Value;


            if (userRoleClaim != "Admin")
                throw new UnauthorizedAccessException("Only admins can update tasks.");


            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out _))
                throw new UnauthorizedAccessException("Invalid user identity.");
            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == request.TaskId);

            if (task == null)
                throw new KeyNotFoundException($"Task with ID {request.TaskId} was not found.");

            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = request.Status;
            task.DueDate = DateTime.UtcNow.AddDays(request.DueInDays);

            await _context.SaveChangesAsync();

            return $"Task {task.Id} Updated Successfully";
        }

    }
}