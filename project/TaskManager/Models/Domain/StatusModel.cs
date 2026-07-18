namespace TaskManager.Models.Domain;

public class StatusModel
{
    public int Id { get; set; }
    public string Status { get; set; } = "";

    public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}