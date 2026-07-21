using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.DTO.Auth;

public record RegisterUserRequestDto
{
    [Required(ErrorMessage = "Este campo es obligatorio.")]
    public string Username { get; set; } = "";

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
}