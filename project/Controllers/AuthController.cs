using Microsoft.AspNetCore.Mvc;
using TaskManager.Common;
using TaskManager.Models.DTO;
using TaskManager.Models.DTO.Auth;
using TaskManager.Services;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;

    public AuthController(IAuthService service)
    {
        _service = service;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto dto)
    {
        var response = await _service.Register(dto);

        return Ok(ApiResponse<RegisterUserResponseDto>.SuccessResponse(response,"Usuario registrado exitosamente.", 201));
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        var response = await _service.Login(dto);
        
        return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response, "Bienvenido.", 200));
    }
}