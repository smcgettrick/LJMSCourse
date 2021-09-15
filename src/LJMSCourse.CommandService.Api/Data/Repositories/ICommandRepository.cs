using System.Collections.Generic;
using System.Threading.Tasks;
using LJMSCourse.CommandService.Api.Models;

namespace LJMSCourse.CommandService.Api.Data.Repositories
{
    public interface ICommandRepository
    {
        Task<IEnumerable<Platform>> GetAllPlatformsAsync();
        Task CreatePlatformAsync(Platform platform);
        Task<bool> PlatformExistsAsync(int platformId);
        Task<bool> ExternalPlatformExistsAsync(int externalPlatformId);

        Task<IEnumerable<Command>> GetAllCommandsForPlatform(int platformId);
        Task<Command> GetCommandAsync(int platformId, int commandId);
        Task CreateCommandAsync(int platformId, Command command);
    }
}