using System.ComponentModel.DataAnnotations;

namespace TaskManager.Models.DTO.Tasks;

public record UpdateTaskDto
{
    [Required(ErrorMessage = "Este campo es obligatorio.")]
    public string Title { get; set; } = "";
    
    [MaxLength(500, ErrorMessage = "Descripción no permite más de 500 caracteres.")]
    public string Description { get; set; } = "";
    
    [Required(ErrorMessage = "Este campo es obligatorio.")]
    public short Priority { get; set; }

    [DataType(DataType.Date)]
    public DateOnly FinishDate { get; set; }

    [DataType(DataType.Date)]
    public DateOnly LimitDate { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    public short Status { get; set; }

    [Required(ErrorMessage = "Este campo es obligatorio.")]
    public int User { get; set; }
}