namespace TaskManager.Models.Domain;

public class TaskModel
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public int PriorityId { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime FinishDate { get; set; }
    public DateTime LimitDate { get; set; }
    public int StatusId { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public UserModel? User { get; set; }
    public PriorityModel? Priority { get; set; }
    public StatusModel? Status { get; set; }
}