using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LJMSCourse.PlatformService.Api.Models.Dtos;
using Microsoft.Extensions.Configuration;

namespace LJMSCourse.PlatformService.Api.Services
{
    public class HttpCommandDataService : ICommandDataService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpCommandDataService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(platformReadDto),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync($"{_configuration["CommandService"]}", httpContent);

            Console.WriteLine(response.IsSuccessStatusCode
                ? "--> HttpCommandDataService.SendPlatformToCommand: OK"
                : "--> HttpCommandDataService.SendPlatformToCommand: FAIL");
        }
    }
}