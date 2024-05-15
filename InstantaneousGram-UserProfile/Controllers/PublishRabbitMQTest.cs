
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Policy;
using System.Text;
using InstantaneousGram_UserProfile.Managers;

namespace InstantaneousGram_UserProfile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishRabbitMQTest : ControllerBase
    {
        private readonly RabbitMQManager _rabbitMQManager;
        public PublishRabbitMQTest(RabbitMQManager rabbitMQSManager) 
        {
            _rabbitMQManager = rabbitMQSManager;
        }

        [HttpPost]
        public IActionResult SendMessage(string message)
        {
            _rabbitMQManager.SendMessage(message);
            return Ok("Message published successfully.");
        }
        [HttpPost("send")]
        public IActionResult SendMessageNew([FromBody] string message)
        {
            _rabbitMQManager.SendMessage(message);
            return Ok("Message sent successfully");
        }
    }
}
