namespace TimeToDo.Application.Dtos;
public class ProjectStatisticsTimeRangeDto
{
    public string Name { get; set; }
    public List<int> Data { get; set; } = new List<int>();
    public List<DateTime> WeekStartDates { get; set; } = new List<DateTime>();
}
