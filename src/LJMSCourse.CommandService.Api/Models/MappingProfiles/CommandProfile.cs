using AutoMapper;
using LJMSCourse.CommandService.Api.Models.Dtos;
using LJMSCourse.CommandService.Api.Protos;

namespace LJMSCourse.CommandService.Api.Models.MappingProfiles
{
    public class CommandProfile : Profile
    {
        public CommandProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<CommandCreateDto, Command>();
            CreateMap<Command, CommandReadDto>();
            CreateMap<PlatformPublishDto, Platform>()
                .ForMember(dest => dest.ExternalId, 
                    opt => opt.MapFrom(src => src.Id));
            CreateMap<GrpcPlatformModel, Platform>()
                .ForMember(dest => dest.ExternalId,
                    opt => opt.MapFrom(src => src.PlatformId))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Commands, 
                    opt => opt.Ignore());
        }
    }
}