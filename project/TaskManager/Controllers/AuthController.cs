using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Common;
using TaskManager.Models;
using TaskManager.Models.DTO.Auth;
using TaskManager.Repositories;

namespace TaskManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly TaskManagerDbContext _context;
    private readonly ITokenRepository _tokenRepository;

    public AuthController(TaskManagerDbContext context, ITokenRepository tokenRepository)
    {
        _context = context;
        _tokenRepository = tokenRepository;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserRequestDto registerUserRequestDto)
    {
        var exists = await _context.Users.AnyAsync(u => u.UserName == registerUserRequestDto.Username);

        if (exists)
            return Conflict(ApiResponse<string>.ErrorResponse(new List<string> {"El usuario ya se encuentra registrado."}, "Ocurrió un error", 409));
        
        var user = new UserModel
        {
            UserName = registerUserRequestDto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(registerUserRequestDto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok(ApiResponse<string>.SuccessResponse("Usario registrado exitosamente", "Éxito", 201));
    }
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == loginRequestDto.Username);

        if (user != null && BCrypt.Net.BCrypt.Verify(loginRequestDto.Password, user.Password))
        {
            var jwtToken = _tokenRepository.CreateJWTToken(user);

            var response = new LoginResponseDto
            {
                JwtToken = jwtToken
            };

            return Ok(ApiResponse<LoginResponseDto>.SuccessResponse(response, "Bienvenido.", 200));
        }

        return BadRequest(ApiResponse<string>.ErrorResponse(new List<string> {"El usuario o contraseña son incorrectos."}, "Ocurrió un error", 409));
    }
}