using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using InstantaneousGram_ContentManagement.Services;

namespace InstantaneousGram_ContentManagement.Managers
{
    public class RabbitMQSubscriber
    {
        private readonly IModel _channel;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQSubscriber(IConnection connection, IServiceProvider serviceProvider)
        {
            _channel = connection.CreateModel();
            _serviceProvider = serviceProvider;
            _channel.ExchangeDeclare(exchange: "user_deletion_exchange", type: ExchangeType.Fanout);
        }

        public void Subscribe()
        {
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "user_deletion_exchange", routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var auth0Id = message; // Parse the Auth0Id directly

                await HandleUserDeletedEvent(auth0Id);
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }

        private async Task HandleUserDeletedEvent(string auth0Id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var contentManagementService = scope.ServiceProvider.GetRequiredService<IContentManagementService>();
                await contentManagementService.DeleteAllContentByUserAsync(auth0Id);
            }
        }
    }
}
