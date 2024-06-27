using RabbitMQ.Client;
using System.Text;
using Microsoft.Extensions.Logging;

namespace InstantaneousGram_UserProfile.Managers
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

        public void PublishUserDeletedEvent(string authId)
        {
            var message = Encoding.UTF8.GetBytes(authId);
            _channel.BasicPublish(exchange: "user_deletion_exchange", routingKey: "", basicProperties: null, body: message);
            _logger.LogInformation($"Published user deletion event for Auth0 ID: {authId}");
        }
    }
}
