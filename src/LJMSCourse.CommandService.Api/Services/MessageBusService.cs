using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LJMSCourse.CommandService.Api.Services
{
    public class MessageBusService : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IEventProcessorService _eventProcessorService;
        private IModel _channel;
        private IConnection _connection;
        private string _queueName;

        public MessageBusService(IConfiguration configuration, IEventProcessorService eventProcessorService)
        {
            _configuration = configuration;
            _eventProcessorService = eventProcessorService;

            InitializeRabbitMq();
        }

        private void InitializeRabbitMq()
        {
            var factory = new ConnectionFactory
            {
                DispatchConsumersAsync = true,
                HostName = _configuration["RabbitMQ:Host"],
                Port = int.Parse(_configuration["RabbitMQ:Port"])
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare("trigger", ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(_queueName, "trigger", "");

            Console.WriteLine(
                $"--> MessageBusService.InitializeRabbitMq: Listening to {factory.HostName}:{factory.Port}");

            _connection.ConnectionShutdown += RabbitMq_ConnectionShutdown;
        }

        private static void RabbitMq_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> MessageBusService.RabbitMq_ConnectionShutdown: Connection shutdown");
        }

        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }

            base.Dispose();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);
            consumer.Received += async (handler, ea) =>
            {
                Console.WriteLine("--> MessageBusService.ExecuteAsync: Message received");

                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                await _eventProcessorService.ProcessEventAsync(message);
            };

            _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}