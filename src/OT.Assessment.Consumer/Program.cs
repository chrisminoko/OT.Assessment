using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using OT.Assessment.Services.BusinessLogic.Implementation;
using OT.Assessment.Services.BusinessLogic.Interfaces;
using OT.Assessment.Repository.Implementation;
using OT.Assessment.Repository.Interface;
using System.Data.SqlClient;
using OT.Assessment.Services.Producer.Connection;
using OT.Assessment.Services.Producer;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(config =>
    {
        config.SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((context, services) =>
    {
        // Register the RabbitMQ hosted service
        services.AddHostedService<RabbitMQHostedService>();
        services.AddScoped<IPlayerService, PlayerService>();
        services.AddScoped<IUnitOfWork>(sp =>
        {
            var connectionString = context.Configuration.GetConnectionString("DatabaseConnection");
            var connection = new SqlConnection(connectionString);
            return new UnitOfWork(connection);
        });
        services.AddSingleton<IRabbitMqConnection>(new RabbitMqConnection()); 
        services.AddScoped<IMessageProducer, RabbitMqProducer>();
        services.AddScoped<IProviderService, ProviderService>();
        services.AddAutoMapper(typeof(Program));

    })
    .Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("Application started {time:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

await host.RunAsync();

logger.LogInformation("Application ended {time:yyyy-MM-dd HH:mm:ss}", DateTime.Now);