using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Common;
using TaskManager.Models.DTO.Tasks;
using TaskManager.Services;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    ITaskService _service;

    public TasksController(ITaskService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] short? priorityId = null,
        [FromQuery] short? statusId = null,
        [FromQuery] int? userId = null,
        [FromQuery] DateOnly? fechaInicial = null,
        [FromQuery] DateOnly? fechaFinal = null)
    {
        var response = await _service.GetAllAsync(page, pageSize, priorityId, statusId, userId, fechaInicial, fechaFinal);
        return Ok(ApiResponse<PagedResultDto<TaskResponseDto>>.SuccessResponse(response, "Success", 200));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var response = await _service.GetByIdAsync(id);
        return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(response, "Éxito", 200));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTaskDto dto)
    {
        var response = await _service.CreateAsync(dto);
        string locationUri = $"/api/Tasks/{response.Id}";

        return Created(locationUri, ApiResponse<TaskResponseDto>.SuccessResponse(response, "Éxito", 201));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTaskDto dto)
    {
        var response = await _service.UpdateAsync(id, dto);
        return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(response, "Éxito", 200));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var response = await _service.DeleteAsync(id);
        return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(response, "Éxito", 200));
    }
}