using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LJMSCourse.CommandService.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace LJMSCourse.CommandService.Api.Data.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
        {
            return await _context.Platforms.ToListAsync();
        }

        public async Task CreatePlatformAsync(Platform platform)
        {
            _ = platform ?? throw new ArgumentNullException(nameof(platform));

            await _context.Platforms.AddAsync(platform);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> PlatformExistsAsync(int platformId)
        {
            return await _context.Platforms.AnyAsync(p => p.Id == platformId);
        }

        public async Task<bool> ExternalPlatformExistsAsync(int externalPlatformId)
        {
            return await _context.Platforms.AnyAsync(p => p.ExternalId == externalPlatformId);
        }

        public async Task<IEnumerable<Command>> GetAllCommandsForPlatform(int platformId)
        {
            return await _context.Commands
                .Where(c => c.PlatformId == platformId)
                .OrderBy(c => c.Platform.Name)
                .ToListAsync();
        }

        public async Task<Command> GetCommandAsync(int platformId, int commandId)
        {
            return await _context.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateCommandAsync(int platformId, Command command)
        {
            _ = command ?? throw new ArgumentNullException(nameof(command));

            command.PlatformId = platformId;
            await _context.Commands.AddAsync(command);
            await _context.SaveChangesAsync();
        }
    }
}