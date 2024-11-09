using Microsoft.Data.SqlClient;
using OT.Assessment.Repository.Implementation;
using OT.Assessment.Repository.Interface;
using OT.Assessment.Services.BusinessLogic.Implementation;
using OT.Assessment.Services.BusinessLogic.Interfaces;
using OT.Assessment.Services.Producer;
using OT.Assessment.Services.Producer.Connection;
using System.Reflection;

namespace OT.Assessment.App.Extensions
{
    public static class ServiceExtensions
    {
       
        public static void ConfigureDependencyInjections(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork>(sp =>
            {
                var connectionString = configuration.GetConnectionString("DatabaseConnection");
                var connection = new SqlConnection(connectionString);
                return new UnitOfWork(connection);
            });

            services.AddScoped<IPlayerService, PlayerService>();
            services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection()); // Doing it this way because I want the connection string to establish as soon as possible | we do not want to create a connection each time we send a request (bad practice it very expensive)
            services.AddScoped<IMessageProducer, RabbitMqProducer>();
        }
    }
}
