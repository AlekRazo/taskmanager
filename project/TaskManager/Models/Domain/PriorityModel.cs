namespace TaskManager.Models.Domain;

public class PriorityModel
{
    public int Id { get; set; }
    public string Priority { get; set; } = "";

    public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}