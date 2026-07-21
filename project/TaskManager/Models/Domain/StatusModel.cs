namespace TaskManager.Models.Domain;

public class StatusModel
{
    public short Id { get; set; }
    public string Status { get; set; } = "";

    public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}