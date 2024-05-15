using InstantaneousGram_ContentManagement.Managers;

namespace InstantaneousGram_ContentManagement.Services
{
    public class RabbitMQTestSubscribe
    {
        private readonly RabbitMQManager _rabbitMQManager;

        public RabbitMQTestSubscribe(RabbitMQManager manager)
        {
            _rabbitMQManager = manager;
        }

        public void StartConsuming()
        {
            
        }
    }
}
