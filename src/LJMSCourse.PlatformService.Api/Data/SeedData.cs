using System;
using System.Linq;
using LJMSCourse.PlatformService.Api.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LJMSCourse.PlatformService.Api.Data
{
    public static class SeedData
    {
        public static void Seed(IApplicationBuilder app, bool isProduciton)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            Seed(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduciton);
        }

        private static void Seed(AppDbContext context, bool isProduction)
        {
            if (isProduction)
            {
                Console.WriteLine("--> SeedData.Seed: Attempting to apply migrations");
                try
                {
                    context.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> SeedData.Seed: Could not apply migrations: {ex.Message}");
                }
            }

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