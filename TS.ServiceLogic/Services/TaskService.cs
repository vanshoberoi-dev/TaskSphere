using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TS.Contract.DTOs.Task;
using TS.Model.Data;
using TS.ServiceLogic.Interfaces;
using TS.ServiceLogic.Common;
using static TS.ServiceLogic.Common.Exceptions;

namespace TS.ServiceLogic.Services
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
            int userId = Utility.ValidateUserAndGetId(_httpContextAccessor.HttpContext?.User);

            var task = new TaskEntity
            {
                Title = request.Title,
                Description = request.Description,
                Status = request.Status,
                DueDate = DateTime.UtcNow.AddDays(request.DueInDays),
                CreatedById = userId
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
            Utility.ValidateAdminAndGetId(_httpContextAccessor.HttpContext?.User);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);

            if (task == null)
                throw new NotFoundException($"Task with ID {request.TaskId} not found.");

            var assignee = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.AssigneeEmail);

            if (assignee == null)
                throw new NotFoundException($"User with email {request.AssigneeEmail} not found.");

            if (task.AssigneeId != null && !request.ForcedAssign)
                throw new NotFoundException($"Task is already assigned. Use ForcedAssign to reassign.");

            var previousAssigneeId = task.AssigneeId;

            task.AssigneeId = assignee.Id;

            await _context.SaveChangesAsync();

            if (previousAssigneeId != null && request.ForcedAssign)
            {
                return $"Task '{task.Title}' (ID: {task.Id}) reassigned to '{assignee.Email}' successfully.";
            }

            return $"Task '{task.Title}' (ID: {task.Id}) assigned to '{assignee.Email}' successfully.";
        }

        public async Task<string> ChangeTaskStatusAsync(ChangeTaskStatusRequestDTO request)
        {
            var userId = Utility.ValidateUserAndGetId(_httpContextAccessor.HttpContext?.User);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);

            if (task == null)
                throw new NotFoundException($"Task with ID {request.TaskId} was not found.");

            if (userId != task.CreatedById)
            {
                throw new UnauthorizedAccessException($"Only the creator of the task can change its status. Task created by user ID {task.CreatedById}.");
            }

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
                    CreatedById = t.CreatedById,
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
                    CreatedById = t.CreatedById,
                    CreatedOn = t.CreatedOn
                })
                .FirstOrDefaultAsync();

            if (response == null)
                throw new NotFoundException($"Task with ID {taskId} was not found.");

            return response;
        }

        public async Task<string> DeleteTaskAsync(DeleteTaskRequestDTO request)
        {
            Utility.ValidateAdminAndGetId(_httpContextAccessor.HttpContext?.User);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);

            if (task == null)
                throw new NotFoundException ($"Task with ID {request.TaskId} was not found.");

            if ((task.Status == TS.Contract.Enums.TaskStatus.InProgress || task.Status == TS.Contract.Enums.TaskStatus.Completed) && !request.ForceDelete)
            {
                return "InProgress or Completed tasks cannot be deleted without ForceDelete.";
            }

            task.IsDeleted = true;
            await _context.SaveChangesAsync();

            return $"Task {task.Id} Deleted Successfully";
        }

        public async Task<string> UpdateTaskAsync(UpdateTaskRequestDTO request)
        {
            Utility.ValidateAdminAndGetId(_httpContextAccessor.HttpContext?.User);

            var task = await _context.Tasks.FirstOrDefaultAsync(t => t.Id == request.TaskId);

            if (task == null)
                throw new NotFoundException ($"Task with ID {request.TaskId} was not found.");

            task.Title = request.Title;
            task.Description = request.Description;
            task.Status = request.Status;
            task.DueDate = DateTime.UtcNow.AddDays(request.DueInDays);
            task.Remarks = request.Remarks;

            await _context.SaveChangesAsync();

            return $"Task {task.Id} Updated Successfully";
        }
    }
}