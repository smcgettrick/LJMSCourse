using AutoMapper;
using LJMSCourse.PlatformService.Api.Models.Dtos;

namespace LJMSCourse.PlatformService.Api.Models.MappingProfiles
{
    public class PlatformProfile : Profile
    {
        public PlatformProfile()
        {
            CreateMap<Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
        }
    }
}