namespace TaskManager.Models.DTO.Tasks;
public record TaskResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Priority { get; set; } = "";
    public DateOnly CreateDate { get; set; }
    public DateOnly FinishDate { get; set; }
    public DateOnly LimitDate { get; set; }
    public string Status { get; set; } = "";
    public string User { get; set; } = "";
}