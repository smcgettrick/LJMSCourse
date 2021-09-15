using System;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LJMSCourse.PlatformService.Api.Models.Dtos;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace LJMSCourse.PlatformService.Api.Services
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;

        public MessageBusClient(IConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration["RabbitMQ:Host"],
                Port = int.Parse(configuration["RabbitMQ:Port"])
            };

            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);

                _connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;

                Console.WriteLine(
                    $"--> MessageBusClient.MessageBusClient: Connected to RabbitMq ({factory.HostName}:{factory.Port})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> MessageBusClient.MessageBusClient: Connection failed to RabbitMq ({factory.HostName}:{factory.Port}) : {ex.Message}");
            }
        }

        public async Task PublishPlatform(PlatformPublishDto platformPublishDto)
        {
            var message = JsonSerializer.Serialize(platformPublishDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> MessageBusClient.PublishPlatform: Sending Message");
                await SendMessageAsync(message);
            }
            else
                Console.WriteLine("--> MessageBusClient.PublishPlatform: Cannot Send Message (Connection Closed)");
        }

        private async Task SendMessageAsync(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger", routingKey: "", basicProperties: null, body: body);

            Console.WriteLine($"--> MessageBusClient.SendMessageAsync: Sent message: {message}");

            await Task.CompletedTask;
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            
            if (!_channel.IsOpen) 
                return;
            
            _channel.Close();
            _connection.Close();
        }

        private static void RabbitMq_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> MessageBusClient.RabbitMq_ConnectionShutdown: RabbitMQ Connection Shutdown");
        }
    }
}