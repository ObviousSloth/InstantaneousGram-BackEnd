using System;
using InstantaneousGram_ImageAndVideoProcessing.Settings;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
namespace InstantaneousGram_ImageAndVideoProcessing.Managers
{
 
    public class RabbitMQManager
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _queueName;
        private readonly RabbitMQSettings _rabbitMQSettings;
        private const string ExchangeName = "TestExchange";   // Hardcoded exchange name
        private const string RoutingKey = "TestKey";          // Hardcoded routing key
        private const string QueueName = "TestImageQueue";    // Hardcoded queue name


        public RabbitMQManager(string queueName, RabbitMQSettings rabbitMQSettings)
        {
            _rabbitMQSettings = rabbitMQSettings;
            var factory = new ConnectionFactory() { HostName = _rabbitMQSettings.Host };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _queueName = queueName;

            _channel.QueueDeclare(queue: _queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }
        /*        public RabbitMQManager(RabbitMQSettings rabbitMQSettings)
                {
                    _rabbitMQSettings = rabbitMQSettings;
                    var factory = new ConnectionFactory() { HostName = _rabbitMQSettings.Host };
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    _queueName = "TestImageQueue";

                    _channel.QueueDeclare(queue: _queueName,
                                          durable: false,
                                          exclusive: false,
                                          autoDelete: false,
                                          arguments: null);
                }*/

        public RabbitMQManager(RabbitMQSettings rabbitMQSettings)
        {
            _connection = new ConnectionFactory() { HostName = rabbitMQSettings.Host }.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: QueueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType.Direct); // Declare exchange
            _channel.QueueBind(queue: QueueName,
                               exchange: ExchangeName,
                               routingKey: RoutingKey);  // Bind queue to exchange with routing key
        }

        public void SendMessage(string message)
        {
            var body = System.Text.Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "TestExchange",
                                  routingKey: "TestKey",
                                  basicProperties: null,
                                  body: body);
            Console.WriteLine(" [x] Sent {0}", message);
        }

        /*     public void ReceiveMessages()
             {
                 var consumer = new EventingBasicConsumer(_channel);
                 consumer.Received += (model, ea) =>
                 {
                     var body = ea.Body.ToArray();
                     var message = System.Text.Encoding.UTF8.GetString(body);
                     Console.WriteLine(" [x] Received {0}", message);
                 };
                 _channel.BasicConsume(queue: _queueName,
                                       autoAck: true,
                                       consumer: consumer);

                 Console.WriteLine(" Press [enter] to exit.");
                 Console.ReadLine();
             }*/
        public void ReceiveMessages()
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = System.Text.Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            _channel.BasicConsume(queue: QueueName,
                                  autoAck: true,
                                  consumer: consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public void CloseConnection()
        {
            _channel.Close();
            _connection.Close();
        }
    }

}
