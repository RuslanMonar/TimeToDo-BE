using AutoMapper;
using TimeToDo.Application.Commands;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Application.MappingProfiles;
public class CreateTaskSessionProfile : Profile
{
    public CreateTaskSessionProfile()
    {
        CreateMap<CreateTaskSessionCommand, TaskSession>();
    }
}
