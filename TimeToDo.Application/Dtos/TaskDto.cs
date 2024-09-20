using TimeToDo.Domain.Enums;

namespace TimeToDo.Application.Dtos;
public class TaskDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public Priority Priority { get; set; }

    public int TomatoCount { get; set; }

    public int TomatoLenght { get; set; }

    public int ProjectId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime? DateCompleted { get; set; }

    public string? Description { get; set; }
}
