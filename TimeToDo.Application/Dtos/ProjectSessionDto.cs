namespace TimeToDo.Application.Dtos;
public class ProjectSessionDto
{
    public string Name { get; set; }  // Назва проекту
    public List<TaskSessionDataDto> Data { get; set; } = new List<TaskSessionDataDto>();
}
