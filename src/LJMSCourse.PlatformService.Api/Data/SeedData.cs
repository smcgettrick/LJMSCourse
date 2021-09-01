using System;
using System.Linq;
using LJMSCourse.PlatformService.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LJMSCourse.PlatformService.Api.Data
{
    public static class SeedData
    {
        public static void Seed(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            Seed(serviceScope.ServiceProvider.GetService<AppDbContext>());
        }

        private static void Seed(AppDbContext context)
        {
            if (!context.Platforms.Any())
            {
                Console.WriteLine("--> SeedData.Seed: Seeding PlatformDb");

                context.Platforms.AddRange(
                    new Platform
                    {
                        Name = "DotNet",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Platform
                    {
                        Name = "SQL Server Express",
                        Publisher = "Microsoft",
                        Cost = "Free"
                    },
                    new Platform
                    {
                        Name = "Kubernetes",
                        Publisher = "Cloud Native Computing Foundation",
                        Cost = "Free"
                    }
                );

                context.SaveChanges();
            }
            else
            {
                Console.WriteLine("--> SeedData.Seed: PlatformDb already seeded");
            }
        }
    }
}