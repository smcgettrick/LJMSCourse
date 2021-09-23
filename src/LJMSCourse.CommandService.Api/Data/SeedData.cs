using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LJMSCourse.CommandService.Api.Data.Repositories;
using LJMSCourse.CommandService.Api.Models;
using LJMSCourse.CommandService.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LJMSCourse.CommandService.Api.Data
{
    public static class SeedData
    {
        public static async Task Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var grpcClient = serviceScope.ServiceProvider.GetService<IGrpcPlatformService>();
            var platforms = await grpcClient.GetAllRemotePlatformsAsync();

            await SeedAsync(serviceScope.ServiceProvider.GetService<ICommandRepository>(), platforms);
        }

        private static async Task SeedAsync(ICommandRepository repository, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("--> SeedData.Seed: Seeding CommandDb");

            foreach (var platform in platforms) await repository.CreatePlatformAsync(platform);
        }
    }
}