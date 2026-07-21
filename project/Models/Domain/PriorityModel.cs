namespace TaskManager.Models.Domain;

public class PriorityModel
{
    public short Id { get; set; }
    public string Priority { get; set; } = "";

    public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}