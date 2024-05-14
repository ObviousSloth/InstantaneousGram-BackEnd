using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstantaneousGram_RabbitMq
{
    public interface IRabbitMQService
    {
        void PublishMessage(string exchangeName, string routingKey, byte[] body);
        void Subscribe(string exchangeName, string queueName, string routingKey, Action<byte[]> handleMessage);
    }
}
