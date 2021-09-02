using System.Threading.Tasks;
using LJMSCourse.PlatformService.Api.Models.Dtos;

namespace LJMSCourse.PlatformService.Api.Services
{
    public interface ICommandDataService
    {
        Task SendPlatformToCommand(PlatformReadDto platformReadDto);
    }
}