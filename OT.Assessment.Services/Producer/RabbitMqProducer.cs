using Microsoft.Extensions.Logging;
using OT.Assessment.Core.Enums;
using OT.Assessment.Model.Response;
using OT.Assessment.Services.Producer.Connection;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OT.Assessment.Services.Producer
{
    public class RabbitMqProducer : IMessageProducer
    {
        private readonly IRabbitMqConnection _connection;
        private readonly ILogger<RabbitMqProducer> _logger;
        public RabbitMqProducer(IRabbitMqConnection connection, ILogger<RabbitMqProducer> logger)
        {
            _connection = connection;
            _logger = logger;
        }
        public async Task<Result> SendMessage<T>(T message, EventQueue QueueName)
        {
            try
            {
                using var channel = await _connection.Connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: QueueName.ToString(),
                    durable: false,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var json = JsonSerializer.Serialize(message);
                var body = Encoding.UTF8.GetBytes(json);

                
                await channel.BasicPublishAsync(
                     exchange: "",
                     routingKey: QueueName.ToString(),
                     body: body
                 );

                return Result.Success();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error publishing message to RabbitMQ");
                return Result.Failure("Failed to publish message");
            }
        }
    }
}
