namespace TaskManager.Models.Domain;

public class UserModel
{
    public int Id { get; set; }
    public string UserName { get; set; } = "";
    public string Password { get; set; } = "";
    public DateTime CreatedAt { get; set; }

    public ICollection<TaskModel> Tasks { get; set; } = new List<TaskModel>();
}