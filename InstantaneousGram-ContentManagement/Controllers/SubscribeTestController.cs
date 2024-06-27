
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using InstantaneousGram_ContentManagement.Managers;

namespace InstantaneousGram_ContentManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubscribeTest : ControllerBase
    {
        private readonly RabbitMQManager _rabbitMQManager;
        

        public SubscribeTest(RabbitMQManager rabbitMQManager)
        {
         _rabbitMQManager = rabbitMQManager;
        }

        [HttpPost]
        public IActionResult StartSubscription()
        {

            try
            {
                _rabbitMQManager.ReceiveMessages();
                return Ok("Message subscribed successfully.");
            }
            catch (Exception)
            {

                throw;

            }
           
           
        }

  
    }
}
