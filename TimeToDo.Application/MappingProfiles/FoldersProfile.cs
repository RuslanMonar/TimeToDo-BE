using AutoMapper;
using TimeToDo.Application.Dtos;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Application.MappingProfiles;
public class FoldersProfile : Profile
{
    public FoldersProfile()
    {
        CreateMap<Folder, FolderDto>();
    }
}
