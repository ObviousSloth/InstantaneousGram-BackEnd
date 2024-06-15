using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using InstantaneousGram_LikesAndComments.Repositories;

namespace InstantaneousGram_LikesAndComments.Services
{
    public class UserDeletionService : BackgroundService
    {
        private readonly IModel _channel;
        private readonly IConnection _connection;
        private readonly IServiceProvider _serviceProvider;

        public UserDeletionService(IConnection connection, IServiceProvider serviceProvider)
        {
            _connection = connection;
            _channel = _connection.CreateModel();
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.ExchangeDeclare(exchange: "user_deletion_exchange", type: ExchangeType.Fanout);

            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queue: queueName, exchange: "user_deletion_exchange", routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                if (int.TryParse(message, out int userId))
                {
                    await HandleUserDeletion(userId);
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task HandleUserDeletion(int userId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var commentRepository = scope.ServiceProvider.GetRequiredService<ICommentRepository>();
                var likeRepository = scope.ServiceProvider.GetRequiredService<ILikeRepository>();

                await commentRepository.DeleteCommentsByUserIdAsync(userId.ToString());
                await likeRepository.DeleteLikesByUserIdAsync(userId.ToString());
            }
        }
    }
}
