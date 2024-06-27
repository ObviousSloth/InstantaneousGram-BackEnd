using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace InstantaneousGram_ContentManagement.Services
{
    public class AccountDeletionSubscriber : BackgroundService
    {
        private readonly IContentManagementService _contentManagementService;
        private readonly IConnection _rabbitMqConnection;

        public AccountDeletionSubscriber(IContentManagementService contentManagementService, IConnection rabbitMqConnection)
        {
            _contentManagementService = contentManagementService;
            _rabbitMqConnection = rabbitMqConnection;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var channel = _rabbitMqConnection.CreateModel();
            channel.ExchangeDeclare(exchange: "user_deletion_exchange", type: ExchangeType.Fanout);
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName, exchange: "user_deletion_exchange", routingKey: "");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var auth0Id = message; // Parse the Auth0Id directly

                await _contentManagementService.DeleteAllContentByUserAsync(auth0Id);
            };

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
