using OT.Assessment.Core.Enums;
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
        public RabbitMqProducer(IRabbitMqConnection connection)
        {
            _connection = connection;
        }
        public async Task SendMessage<T>(T message, EventQueue QueueName)
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
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Error sending message: {ex.Message}");
                throw; 
            }
        }
    }
}
