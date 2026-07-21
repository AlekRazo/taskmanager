using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Common;
using TaskManager.Models.DTO.Reports;
using TaskManager.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _service;

    public ReportsController(IReportService service)
    {
        _service = service;
    }

    [HttpGet]
    [Route("pending-tasks")]
    public async Task<IActionResult> GetReports()
    {
        var response = await _service.GetPendingTasks();

        return Ok(ApiResponse<List<PendingTasksReportDto>>.SuccessResponse(response,"Reporte generado exitosamente.", 200));
    }
}