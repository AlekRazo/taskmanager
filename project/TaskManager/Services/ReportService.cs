using Microsoft.EntityFrameworkCore;
using TaskManager.Models;
using TaskManager.Models.Domain;
using TaskManager.Models.DTO.Reports;

namespace TaskManager.Services;

public class ReportService : IReportService
{
    TaskManagerDbContext _context;

    public ReportService(TaskManagerDbContext context)
    {
        _context = context;
    }

    public async Task<List<PendingTasksReportDto>> GetPendingTasks()
    {
        var report = await _context.Database
        .SqlQuery<PendingTasksReportModel>($"EXEC sp_GetPendingTasks")
        .ToListAsync();

        return report.Select(x => new PendingTasksReportDto
        {
            Usuario = x.Username,
            TotalPendientes = x.TotalPending,
            TotalVencidas = x.TotalOverdue
        }).ToList();
    }
}