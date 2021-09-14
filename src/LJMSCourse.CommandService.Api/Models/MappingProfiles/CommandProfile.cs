using AutoMapper;
using LJMSCourse.CommandService.Api.Models.Dtos;

namespace LJMSCourse.CommandService.Api.Models.MappingProfiles
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
        }
    }
}