using System.Threading.Tasks;

namespace LJMSCourse.CommandService.Api.Services
{
    public interface IEventProcessorService
    {
        Task ProcessEventAsync(string message);
    }
}