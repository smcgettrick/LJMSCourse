using System;
using LJMSCourse.PlatformService.Api.Data;
using LJMSCourse.PlatformService.Api.Data.Repositories;
using LJMSCourse.PlatformService.Api.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace LJMSCourse.PlatformService.Api
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            if (_environment.IsProduction())
            {
                var builder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("PlatformDb"))
                {
                    Password = Configuration["SA_PASSWORD"]
                };

                Console.WriteLine("--> Startup.ConfigureServices: Using MSSQL Database");
                services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.ConnectionString));
            }
            else
            { 
                Console.WriteLine("--> Startup.ConfigureServices: Using InMemory Database");
                services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("PlatformDb"));
            }

            services.AddScoped<IPlatformRepository, PlatformRepository>();
            services.AddHttpClient<ICommandDataService, HttpCommandDataService>();

            services.AddControllers();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "LJMSCourse.PlatformService.Api", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LJMSCourse.PlatformService.Api v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            SeedData.Seed(app, env.IsProduction());
            
        }
    }
}