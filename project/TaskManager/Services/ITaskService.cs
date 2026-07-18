using TaskManager.Common;
using TaskManager.Models.DTO.Task;

namespace TaskManager.Services;

public interface ITaskService
{
    Task<PagedResultDto<TaskResponseDto>> GetAllAsync(int page, int pageSize);
    Task<TaskResponseDto> GetByIdAsync(int id);
    Task<TaskResponseDto> CreateAsync(CreateTaskDto dto);
    Task<TaskResponseDto> UpdateAsync(int id, UpdateTaskDto dto);
    Task DeleteAsync(int id);
}