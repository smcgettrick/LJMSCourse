using System.Collections.Generic;
using System.Threading.Tasks;
using LJMSCourse.PlatformService.Api.Models;

namespace LJMSCourse.PlatformService.Api.Data.Repositories
{
    public interface IPlatformRepository
    {
        Task<IEnumerable<Platform>> GetAllAsync();
        Task<Platform> GetByIdAsync(int id);
        Task CreateAsync(Platform platform);
    }
}