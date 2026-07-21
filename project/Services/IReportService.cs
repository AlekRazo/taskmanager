using TaskManager.Models.DTO.Reports;

namespace TaskManager.Services;

public interface IReportService
{
    Task<List<PendingTasksReportDto>> GetPendingTasks();
}