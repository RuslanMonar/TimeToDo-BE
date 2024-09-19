namespace TimeToDo.Domain.Entities;
public class Folder
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Guid UserId { get; set; }
    public string Color { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();
}
