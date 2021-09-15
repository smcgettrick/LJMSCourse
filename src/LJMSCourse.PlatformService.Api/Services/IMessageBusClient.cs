using System.Threading.Tasks;
using LJMSCourse.PlatformService.Api.Models.Dtos;

namespace LJMSCourse.PlatformService.Api.Services
{
    public interface IMessageBusClient
    {
        Task PublishPlatform(PlatformPublishDto platformPublishDto);
    }
}