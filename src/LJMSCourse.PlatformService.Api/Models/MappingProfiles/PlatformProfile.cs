using AutoMapper;
using LJMSCourse.PlatformService.Api.Models.Dtos;
using LJMSCourse.PlatformService.Api.Protos;

namespace LJMSCourse.PlatformService.Api.Models.MappingProfiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
            CreateMap<PlatformReadDto, PlatformPublishDto>();
            CreateMap<Platform, GrpcPlatformModel>()
                .ForMember(dest => dest.PlatformId, 
                    opt => opt.MapFrom(src => src.Id));
        }
    }
}