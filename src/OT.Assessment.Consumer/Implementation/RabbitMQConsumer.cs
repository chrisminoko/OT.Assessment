﻿using Microsoft.Extensions.Configuration;
using OT.Assessment.Core.Enums;
using OT.Assessment.Model.Entities;
using OT.Assessment.Services.BusinessLogic.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

public class RabbitMQHostedService : IHostedService
{
    private readonly ILogger<RabbitMQHostedService> _logger;
    private readonly IPlayerService _playerService;
    private readonly IProviderService _providerService;
    private IConnection? _connection;
    private readonly IConfiguration _configuration;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly string _hostName;
    private readonly int _portNumber;
    private readonly string _userName;
    private readonly string _password;
    private readonly string _virtualHost;
    private readonly string _clientProviderName;

    public RabbitMQHostedService(ILogger<RabbitMQHostedService> logger, IConfiguration configuration, IPlayerService playerService, IProviderService providerService)
    {
        _logger = logger;
        _cancellationTokenSource = new CancellationTokenSource();
        _configuration = configuration;
        _hostName = _configuration.GetValue<string>("RabbitMQ:HostName");
        _portNumber = _configuration.GetValue<int>("RabbitMQ:Port");
        _userName = _configuration.GetValue<string>("RabbitMQ:UserName");
        _password = _configuration.GetValue<string>("RabbitMQ:Password");
        _virtualHost = _configuration.GetValue<string>("RabbitMQ:VirtualHost");
        _clientProviderName = _configuration.GetValue<string>("RabbitMQ:ClientName");
        _playerService = playerService;
        _providerService = providerService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMQ Consumer Service starting...");

        var factory = new ConnectionFactory
        {
            HostName = _hostName,
            Port = _portNumber,
            UserName = _userName,
            Password = _password,
            VirtualHost = _virtualHost,
            ClientProvidedName = _clientProviderName
        };

        _connection = await factory.CreateConnectionAsync();

        await using var channel = await _connection.CreateChannelAsync();
        var queueNames = Enum.GetNames(typeof(EventQueue));

        foreach (var queueName in queueNames)
        {
            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (sender, args) =>
            {
                try
                {
                    var body = args.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    _logger.LogInformation("Received message from queue {QueueName}: {Message}", queueName, message); 

                    await ProcessMessage(queueName, message);
                    await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing message from queue {QueueName}", queueName); // I would prefer to actually have a table to insert data here for every failed message and also save the exeception to help with debugging 
                    await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: false);
                }
            };

            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
            await channel.BasicConsumeAsync(
                queue: queueName,
                autoAck: false,
                consumer: consumer);

            _logger.LogInformation("Started consuming from queue: {QueueName}", queueName);
        }

        // Keep the service running
        while (!_cancellationTokenSource.Token.IsCancellationRequested)
        {
            await Task.Delay(1000, _cancellationTokenSource.Token);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("RabbitMQ Consumer Service stopping...");
        _cancellationTokenSource.Cancel();

        if (_connection != null && _connection.IsOpen)
        {
            await _connection.CloseAsync();
            _connection.Dispose();
        }

        _cancellationTokenSource.Dispose();
        _logger.LogInformation("RabbitMQ Consumer Service stopped");
    }

    private async Task ProcessMessage(string queueName, string message)
    {
        if (Enum.TryParse<EventQueue>(queueName, out var eventQueue))
        {
            switch (eventQueue)
            {
               
                case EventQueue.CasinoWager:
                    var casionwager = JsonSerializer.Deserialize<CasinoWager>(message);
                    await  _playerService.ProcessCasinoWagerCreationAsync(casionwager);
                    break;  
                case EventQueue.CreateProvider:
                    var provider = JsonSerializer.Deserialize<Provider>(message); // I just added this so to demonstrate that it can process different topics
                    await  _providerService.ProcessProviderCreationAsync(provider);
                    break;

                default:
                    throw new ArgumentException($"Unknown event type: {queueName}");
            }
        }
    }


}


