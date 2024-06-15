using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using InstantaneousGram_LikesAndComments.Services;

namespace InstantaneousGram_LikesAndComments.Managers
{
    public class RabbitMQListener : BackgroundService
    {
        private readonly IModel _channel;
        private readonly ILogger<RabbitMQListener> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RabbitMQListener(IConnection connection, ILogger<RabbitMQListener> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _channel = connection.CreateModel();
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;

            _channel.ExchangeDeclare(exchange: "user_deletion_exchange", type: ExchangeType.Fanout);
            _channel.QueueDeclare(queue: "user_deletion_queue", durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: "user_deletion_queue", exchange: "user_deletion_exchange", routingKey: "");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                _logger.LogInformation($"Received user deletion event for user ID: {content}");

                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var likeService = scope.ServiceProvider.GetRequiredService<ILikeService>();
                    var commentService = scope.ServiceProvider.GetRequiredService<ICommentService>();

                    try
                    {
                        await likeService.DeleteLikesByUserIdAsync(content);
                        await commentService.DeleteCommentsByUserIdAsync(content);
                        _logger.LogInformation($"Successfully deleted likes and comments for user ID: {content}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error deleting likes and comments for user ID: {content}: {ex.Message}");
                    }
                }

                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("user_deletion_queue", false, consumer);
            return Task.CompletedTask;
        }

        public override void Dispose()
        {
            _channel.Close();
            _channel.Dispose();
            base.Dispose();
        }
    }
}
