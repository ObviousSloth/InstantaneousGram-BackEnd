using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;


namespace InstantaneousGram_RabbitMq

{
    public class RabbitMQService : IRabbitMQService, IDisposable // Define a class RabbitMQService that implements IRabbitMQService interface and IDisposable interface for resource cleanup
    {
        private readonly ConnectionFactory _connectionFactory; // Declare a readonly field for ConnectionFactory to create RabbitMQ connections
        private IConnection _connection; // Declare a field for RabbitMQ connection
        private IModel _channel; // Declare a field for RabbitMQ channel

        // Constructor to initialize RabbitMQService with connection details
        public RabbitMQService(string hostName, int port, string userName, string password)
        {
            // Create a new ConnectionFactory instance with provided connection details
            _connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };
        }

        // Method to publish a message to RabbitMQ
        public void PublishMessage(string exchangeName, string routingKey, byte[] body)
        {
            // Ensure a connection to RabbitMQ is established
            EnsureConnection();

            // Publish the message to the specified exchange and routing key
            _channel.BasicPublish(exchange: exchangeName,
                                  routingKey: routingKey,
                                  basicProperties: null,
                                  body: body);
        }

        // Method to subscribe to messages from RabbitMQ
        public void Subscribe(string exchangeName, string queueName, string routingKey, Action<byte[]> handleMessage)
        {
            // Ensure a connection to RabbitMQ is established
            EnsureConnection();

            // Declare the exchange on the channel
            _channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Topic);

            // Declare the queue on the channel
            _channel.QueueDeclare(queue: queueName,
                                  durable: false,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);

            // Bind the queue to the exchange with the specified routing key
            _channel.QueueBind(queue: queueName,
                               exchange: exchangeName,
                               routingKey: routingKey);

            // Create a consumer for handling received messages
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                // Extract the message body as byte array
                var body = ea.Body.ToArray();

                // Call the provided handleMessage action with the message body
                handleMessage(body);
            };

            // Start consuming messages from the queue
            _channel.BasicConsume(queue: queueName,
                                  autoAck: true,
                                  consumer: consumer);
        }

        // Method to ensure a connection to RabbitMQ is established
        private void EnsureConnection()
        {
            // If no connection exists or the existing connection is closed
            if (_connection == null || !_connection.IsOpen)
            {
                // Create a new connection using the connection factory
                _connection = _connectionFactory.CreateConnection();

                // Create a new channel on the connection
                _channel = _connection.CreateModel();
            }
        }

        // IDisposable implementation to clean up resources
        public void Dispose()
        {
            // Dispose the channel and connection if they exist
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}