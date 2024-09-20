using AutoMapper;
using TimeToDo.Application.Dtos;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Application.MappingProfiles;
public class ProjectsProfile : Profile
{
    public ProjectsProfile()
    {
        CreateMap<Project, ProjectDto>();
    }
}
