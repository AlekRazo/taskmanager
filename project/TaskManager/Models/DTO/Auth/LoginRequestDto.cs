using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.DTO.Auth;

public record LoginRequestDto
{
    [Required]
    public string Username { get; set; } = "";


    [Required]
    public string Password { get; set; } = "";
}