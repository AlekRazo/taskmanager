using Microsoft.EntityFrameworkCore;
using TaskManager.Common;
using TaskManager.Models;
using TaskManager.Models.Domain;
using TaskManager.Models.DTO.Task;

namespace TaskManager.Services;

public class TaskService : ITaskService
{ 
    private readonly TaskManagerDbContext _context;

    public TaskService(TaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<TaskResponseDto>> GetAllAsync(int page, int pageSize)
    {
        var items = await _context.Tasks
                .AsNoTracking()
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.User)
                .OrderBy(p => p.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

        var total = await _context.Tasks.CountAsync();

        return new PagedResultDto<TaskResponseDto>
        {
            Items = items.Select(x => Map(x)),
            TotalItems = total,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<TaskResponseDto> GetByIdAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        return task is null ? null : Map(task);
    }

    public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskResponseDto> UpdateAsync(int id, UpdateTaskDto dto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    private static TaskResponseDto Map(TaskModel task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority is not null ? task.Priority.Priority : "Desconocida" ,
            CreateDate = task.CreateDate,
            FinishDate = task.FinishDate,
            LimitDate = task.LimitDate,
            Status = task.Status is not null ? task.Status.Status : "Desconocido",
            User = task.User is not null ? task.User.UserName : "Usuario Desconocido"
        };
    }
}