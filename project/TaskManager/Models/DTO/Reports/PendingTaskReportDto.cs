namespace TaskManager.Models.DTO.Reports;

public class PendingTasksReportDto
{
    public string Usuario { get; set; } = "";
    public int TotalPendientes { get; set; }
    public int TotalVencidas { get; set; }
}