using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Assessment.Services.Producer.Connection
{
    public class RabbitMqConnection : IRabbitMqConnection, IDisposable
    {
        private IConnection? _connection;
        public IConnection Connection => _connection!;

        public RabbitMqConnection()
        {
            InitializeConnection();
        }

        private async void InitializeConnection()
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                ClientProvidedName = "OT.Assessment.App" // Helps identify your application in RabbitMQ management UI
            };

            // Optional: Add connection recovery settings
            factory.AutomaticRecoveryEnabled = true;
            factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(10);

            _connection = await factory.CreateConnectionAsync();
        }

        public void Dispose()
        {
            if (_connection?.IsOpen == true)
            {
                _connection?.CloseAsync();
            }
            _connection?.Dispose();
        }
    }
}
