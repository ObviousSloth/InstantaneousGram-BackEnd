/*
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using InstantaneousGram_ImageAndVideoProcessing.Managers;

namespace InstantaneousGram_ImageAndVideoProcessing.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class SubscribeRabbitMQTest : ControllerBase
    {
        private readonly RabbitMQManager _rabbitMQManager;


        public SubscribeRabbitMQTest(RabbitMQManager rabbitMQManager)
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
        [HttpPost("close")]
        public IActionResult CloseSubscription()
        {

            try
            {
                _rabbitMQManager.CloseConnection();
                return Ok(" Unsubscribed successfully.");
            }
            catch (Exception)
            {

                throw;

            }




        }
    }
}
*/