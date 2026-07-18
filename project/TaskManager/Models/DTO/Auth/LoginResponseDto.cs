namespace TaskManager.Models.DTO.Auth;
public record LoginResponseDto
{
    public string JwtToken { get; set; } = "";
}