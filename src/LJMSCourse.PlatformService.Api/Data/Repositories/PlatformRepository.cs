using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LJMSCourse.PlatformService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LJMSCourse.PlatformService.Api.Data.Repositories
{
    public class PlatformRepository : IPlatformRepository
    {
        private readonly AppDbContext _context;

        public PlatformRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Platform>> GetAllAsync()
        {
            return await _context.Platforms.ToListAsync();
        }

        public async Task<Platform> GetByIdAsync(int id)
        {
            return await _context.Platforms.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task CreateAsync(Platform platform)
        {
            _ = platform ?? throw new ArgumentNullException(nameof(platform));

            await _context.Platforms.AddAsync(platform);
            await _context.SaveChangesAsync();
        }
    }
}
