namespace TaskManager.Models.DTO;
public record RegisterUserResponseDto
{
    public int Id { get; set; }
    public string UserName { get; set; } = "";
}