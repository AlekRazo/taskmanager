using Microsoft.EntityFrameworkCore;
using TaskManager.Common;
using TaskManager.Common.Exceptions;
using TaskManager.Models;
using TaskManager.Models.Domain;
using TaskManager.Models.DTO.Tasks;

namespace TaskManager.Services;

public class TaskService : ITaskService
{ 
    private readonly TaskManagerDbContext _context;

    public TaskService(TaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<TaskResponseDto>> GetAllAsync(int page, int pageSize, short? priorityId, short? statusId, int? userId, DateOnly? fechaInicial, DateOnly? fechaFinal)
    {
        if (!fechaInicial.HasValue && fechaFinal.HasValue)
            throw new BusinessException("Error de proceso", new List<string> { "No puede existir fecha final sin fecha de inicio." });

        if (fechaInicial.HasValue && fechaFinal.HasValue && fechaFinal < fechaInicial)
            throw new BusinessException("Error de proceso", new List<string> { "La fecha final no puede ser anterior a la fecha de inicio." });

        var query = _context.Tasks
            .Include(t => t.Priority)
            .Include(t => t.Status)
            .Include(t => t.User)
            .AsNoTracking()
            .AsQueryable();

        if (priorityId.HasValue)
            query = query.Where(t => t.PriorityId == priorityId.Value);

        if (statusId.HasValue)
            query = query.Where(t => t.StatusId == statusId.Value);

        if (userId.HasValue)
            query = query.Where(t => t.UserId == userId.Value);

        if (fechaInicial.HasValue)
            query = query.Where(t => t.LimitDate >= fechaInicial.Value);

        if (fechaFinal.HasValue)
            query = query.Where(t => t.LimitDate <= fechaFinal.Value);
                
        var items = await query
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
        var task = await _context.Tasks
                .AsNoTracking()
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

        if (task is null)
        {
            throw new NotFoundException($"No existe la tarea con el id {id}.");
        }

        return Map(task);
    }

    public async Task<TaskResponseDto> CreateAsync(CreateTaskDto dto)
    {
        List<string> errors = new List<string>();

        if(dto.CreateDate < DateOnly.FromDateTime(DateTime.Now))
            errors.Add("La fecha de creación no puede ser anterior al día de hoy.");

        if(dto.CreateDate > dto.FinishDate)
            errors.Add("La fecha de creación no puede ser posterior a la fecha de finalización.");

        if(dto.CreateDate > dto.LimitDate)
            errors.Add("La fecha de creación no puede ser posterior a la fecha límite");

        if(dto.FinishDate > dto.LimitDate)
            errors.Add("La fecha de finalización no puede ser posterior a la fecha límite.");

        var exists = await _context.Tasks.AnyAsync(t => t.Title == dto.Title);

        if(exists)
            errors.Add("Ya existe una tarea con el título ingresado.");

        if(errors.Count > 0)
            throw new BusinessException("Error al crear nueva tarea", errors);

        var task = new TaskModel
        {
            Title = dto.Title,
            Description  = dto.Description,
            PriorityId = dto.Priority,
            CreateDate = dto.CreateDate,
            FinishDate = dto.FinishDate,
            LimitDate = dto.LimitDate,
            StatusId = dto.Status,
            UserId = dto.User
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return Map(task);
    }

    public async Task<TaskResponseDto> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _context.Tasks
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

        if(task is null)
            throw new NotFoundException($"No existe la tarea con el id {id}.");

        List<string> errors = new List<string>();

        if(dto.FinishDate > dto.LimitDate)
            errors.Add("La fecha de finalización no puede ser posterior a la fecha límite.");

        //Se debe verificar que no exista otra tarea con el mismo nombre, de forma que sí se pueda modificar aquella con el ID en cuestión
        var exists = await _context.Tasks.AnyAsync(t => t.Title == dto.Title && t.Id != id);

        if(exists)
            errors.Add("Ya existe una tarea con el título ingresado.");

        if(errors.Count > 0)
            throw new BusinessException("Error al modificar tarea", errors);

        task.Title = dto.Title;
        task.Description  = dto.Description;
        task.PriorityId = dto.Priority;
        task.FinishDate = dto.FinishDate;
        task.LimitDate = dto.LimitDate;
        task.StatusId = dto.Status;
        task.UserId = dto.User;
        
        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();

        return Map(task);
    }

    public async Task<TaskResponseDto> DeleteAsync(int id)
    {
        var task = await _context.Tasks
                .Include(t => t.Priority)
                .Include(t => t.Status)
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);

        if(task is null)
            throw new NotFoundException($"No existe la tarea con el id {id}.");
            
        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return Map(task);
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