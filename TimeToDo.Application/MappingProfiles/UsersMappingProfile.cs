using AutoMapper;
using TimeToDo.Application.Dtos;
using TimeToDo.Domain.Entities;

namespace TimeToDo.Application.MappingProfiles;
public class UsersMappingProfile : Profile
{
    public UsersMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName));
    }
}
