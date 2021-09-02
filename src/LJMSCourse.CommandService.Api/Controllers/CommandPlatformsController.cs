using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace LJMSCourse.CommandService.Api.Controllers
{
    [ApiController]
    [Route("/api/v1/[controller]")]
    public class CommandPlatformsController : ControllerBase
    {
        public CommandPlatformsController()
        {
            
        }

        [HttpPost]
        public async Task ReceivePlatform()
        {
            Console.WriteLine("--> CommandPlatformsController.ReceivePlatform: Received POST request");
        }
    }
}