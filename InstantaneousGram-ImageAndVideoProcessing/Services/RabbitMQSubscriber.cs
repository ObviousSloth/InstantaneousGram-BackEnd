using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using InstantaneousGram_ImageAndVideoProcessing.Services;

public class RabbitMQSubscriber : BackgroundService
{
    private readonly IModel _channel;
    private readonly ILogger<RabbitMQSubscriber> _logger;
    private readonly IImageAndVideoService _imageAndVideoService;

    public RabbitMQSubscriber(IConnection connection, ILogger<RabbitMQSubscriber> logger, IImageAndVideoService imageAndVideoService)
    {
        _channel = connection.CreateModel();
        _logger = logger;
        _imageAndVideoService = imageAndVideoService;

        _channel.ExchangeDeclare(exchange: "user_deletion_exchange", type: ExchangeType.Fanout);
        var queueName = _channel.QueueDeclare().QueueName;
        _channel.QueueBind(queue: queueName, exchange: "user_deletion_exchange", routingKey: "");

        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received message: {message}");

            await _imageAndVideoService.DeleteMediaByUserIdAsync(message);
        };

        _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RabbitMQ subscriber running.");
        return Task.CompletedTask;
    }
}
