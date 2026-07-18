namespace TaskManager.Models.DTO.Task;

public record CreateTaskDto
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public string Priority { get; set; } = "";
    public DateTime CreateDate { get; set; }
    public DateTime FinishDate { get; set; }
    public DateTime LimitDate { get; set; }
    public string Status { get; set; } = "";
    public string User { get; set; } = "";
}