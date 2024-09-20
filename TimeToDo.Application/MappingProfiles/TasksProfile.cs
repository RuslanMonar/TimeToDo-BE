using AutoMapper;
using TimeToDo.Application.Dtos;

namespace TimeToDo.Application.MappingProfiles;
public class TasksProfile : Profile
{
    public TasksProfile()
    {
        CreateMap<Domain.Entities.Task, TaskDto>();
    }
}
