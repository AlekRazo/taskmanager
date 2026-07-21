using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.DTO.Auth;

public record LoginRequestDto
{
    [Required(ErrorMessage = "Este campo es obligatorio.")]
    [DataType(DataType.EmailAddress)]
    public string Username { get; set; } = "";


    [Required(ErrorMessage = "Este campo es obligatorio.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = "";
}