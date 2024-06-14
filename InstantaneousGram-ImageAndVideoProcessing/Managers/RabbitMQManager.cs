using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InstantaneousGram_ImageAndVideoProcessing.Managers
{
    public class RabbitMQManager
    {
        private readonly IModel _channel;
        private readonly ILogger<RabbitMQManager> _logger;

        public RabbitMQManager(IConnection connection, ILogger<RabbitMQManager> logger)
        {
            _channel = connection.CreateModel();
            _logger = logger;
            _channel.ExchangeDeclare(exchange: "user_deletion_exchange", type: ExchangeType.Fanout);
        }

        public void SubscribeToUserDeletedEvent(Func<int, Task> handleUserDeletedAsync)
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "user_deletion_exchange", routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                if (int.TryParse(message, out int userId))
                {
                    _logger.LogInformation($"Received user deletion event for user ID: {userId}");
                    await handleUserDeletedAsync(userId);
                }
                else
                {
                    _logger.LogWarning($"Failed to parse user ID from message: {message}");
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            _logger.LogInformation("Subscribed to user deletion events");
        }
    }
}
