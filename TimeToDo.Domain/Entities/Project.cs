namespace TimeToDo.Domain.Entities;
public class Project
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int FolderId { get; set; }
    public Guid UserId { get; set; }
    public string Color { get; set; }

    public Folder Folder { get; set; }
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}
