namespace TaskManager.Models.Domain;

public class TaskModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public short PriorityId { get; set; }
    public DateOnly CreateDate { get; set; }
    public DateOnly FinishDate { get; set; }
    public DateOnly LimitDate { get; set; }
    public short StatusId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public UserModel? User { get; set; }
    public PriorityModel? Priority { get; set; }
    public StatusModel? Status { get; set; }
}