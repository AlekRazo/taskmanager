using TaskManager.Models.DTO;
using TaskManager.Models.DTO.Auth;

namespace TaskManager.Services;

public interface IAuthService
{
    Task<LoginResponseDto> Login(LoginRequestDto dto);
    Task<RegisterUserResponseDto> Register(RegisterUserRequestDto dto);

}