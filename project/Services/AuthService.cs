using Microsoft.EntityFrameworkCore;
using TaskManager.Common.Exceptions;
using TaskManager.Models;
using TaskManager.Models.Domain;
using TaskManager.Models.DTO;
using TaskManager.Models.DTO.Auth;
using TaskManager.Repositories;

namespace TaskManager.Services;

public class AuthService : IAuthService
{
    private readonly TaskManagerDbContext _context;
    private readonly ITokenRepository _tokenRepository;

    public AuthService(TaskManagerDbContext context, ITokenRepository tokenRepository)
    {
        _context = context;
        _tokenRepository = tokenRepository;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.Username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
        {
            throw new UnauthorizedException("El usuario o contraseña son incorrectos.");
        }

        var jwtToken = _tokenRepository.CreateJWTToken(user);

        return new LoginResponseDto
        {
            JwtToken = jwtToken
        };
    }

    public async Task<RegisterUserResponseDto> Register(RegisterUserRequestDto dto)
    {
        var exists = await _context.Users.AnyAsync(u => u.UserName == dto.Username);

        if (exists)
            throw new BusinessException("El usuario ya se encuentra registrado.");
        
        var user = new UserModel
        {
            UserName = dto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(dto.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return new RegisterUserResponseDto()
        {
            Id = user.Id,
            UserName = user.UserName
        };
    }
}