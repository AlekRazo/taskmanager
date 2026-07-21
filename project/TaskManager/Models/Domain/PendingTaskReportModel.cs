using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TaskManager.Models.Domain;

[Keyless]
public class PendingTasksReportModel
{
    [Column("username")]
    public string Username { get; set; } = "";

    [Column("total_pendientes")]
    public int TotalPending { get; set; }

    [Column("total_vencidas")]
    public int TotalOverdue { get; set; }
}