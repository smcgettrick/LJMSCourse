using System.Collections.Generic;
using System.Threading.Tasks;
using LJMSCourse.CommandService.Api.Models;

namespace LJMSCourse.CommandService.Api.Services
{
    public interface IGrpcPlatformService
    {
        Task<IEnumerable<Platform>> GetAllRemotePlatformsAsync();
    }
}