namespace TimeToDo.Domain.Entities;
public class TaskSession
{
    public int Id { get; set; }
    public int TaskId { get; set; }
    public Guid SessionId { get; set; }
    public DateTime StartDate { get; set; }
    public TimeSpan TimerStart { get; set; }
    public TimeSpan? TimerPause { get; set; }
    public TimeSpan? TimerEnd { get; set; }
    public int SessionDurationMinutes { get; set; }
    public bool IsFullItteration { get; set; }

    public Task Task { get; set; }
}
