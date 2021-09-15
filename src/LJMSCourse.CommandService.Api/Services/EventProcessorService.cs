using System;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using LJMSCourse.CommandService.Api.Data.Repositories;
using LJMSCourse.CommandService.Api.Models;
using LJMSCourse.CommandService.Api.Models.Dtos;
using Microsoft.Extensions.DependencyInjection;

namespace LJMSCourse.CommandService.Api.Services
{
    public class EventProcessorService : IEventProcessorService
    {
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _scopeFactory;

        public EventProcessorService(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task ProcessEventAsync(string message)
        {
            var eventType = DetermineEventType(message);

            switch (eventType)
            {
                case EventType.PlatformPublished:
                    await AddPlatform(message);
                    break;
                case EventType.Undetermined:
                    break;
            }
        }

        private static EventType DetermineEventType(string message)
        {
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(message);

            switch (eventType?.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("--> EventProcessorService.DetermineEventType: PlatformPublished");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("--> EventProcessorService.DetermineEventType: Undetermined");
                    return EventType.Undetermined;
            }
        }

        private async Task AddPlatform(string platformPublishedMessage)
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<ICommandRepository>();

            var platformDto = JsonSerializer.Deserialize<PlatformPublishDto>(platformPublishedMessage);

            try
            {
                var platform = _mapper.Map<Platform>(platformDto);

                var exists = await repository.ExternalPlatformExistsAsync(platform.ExternalId);
                if (!exists)
                {
                    Console.WriteLine(
                        $"--> EventProcessorService.AddPlatform: Creating Platform External ID = {platform.ExternalId}");
                    await repository.CreatePlatformAsync(platform);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> EventProcessorService.AddPlatform: Could not add platform: {ex.Message}");
            }
        }
    }

    internal enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}