using TaskManager.Common;
using TaskManager.Models.DTO.Tasks;

namespace TaskManager.Services;

public interface ITaskService
{
    Task<PagedResultDto<TaskResponseDto>> GetAllAsync(int page, int pageSize, short? priorityId, short? statusId, int? userId, DateOnly? fechaInicial, DateOnly? fechaFinal);
    Task<TaskResponseDto> GetByIdAsync(int id);
    Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
    Task<TaskResponseDto> UpdateAsync(int id, UpdateTaskDto dto);
    Task<TaskResponseDto> DeleteAsync(int id);
}