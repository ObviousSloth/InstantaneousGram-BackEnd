using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace InstantaneousGram_LikesAndComments.Controllers
{
    [Route("likesandcomments/api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("rabbitmq-test")]
        public IActionResult TestRabbitMQ()
        {
            try
            {
                var factory = new ConnectionFactory()
                {
                    HostName = Environment.GetEnvironmentVariable("RabbitMQ__HostName"),
                    Port = int.Parse(Environment.GetEnvironmentVariable("RabbitMQ__Port")),
                    UserName = Environment.GetEnvironmentVariable("RabbitMQ__UserName"),
                    Password = Environment.GetEnvironmentVariable("RabbitMQ__Password")
                };

                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    return Ok("RabbitMQ connection successful!");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"RabbitMQ connection failed: {ex.Message}");
            }
        }
    }
}
