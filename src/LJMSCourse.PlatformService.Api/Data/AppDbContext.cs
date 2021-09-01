using LJMSCourse.PlatformService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LJMSCourse.PlatformService.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}