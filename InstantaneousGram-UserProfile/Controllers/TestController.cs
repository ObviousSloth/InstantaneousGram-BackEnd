using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace InstantaneousGram_UserProfile.Controllers
{
    [Route("userprofile/api/[controller]")]
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
                var host = Environment.GetEnvironmentVariable("RabbitMQ__HostName");
                var port = Environment.GetEnvironmentVariable("RabbitMQ__Port");
                var user = Environment.GetEnvironmentVariable("RabbitMQ__UserName");
                var password = Environment.GetEnvironmentVariable("RabbitMQ__Password");

                return StatusCode(500, $"RabbitMQ connection failed: {ex.Message}\nHost: {host}\nPort: {port}\nUser: {user}\nPassword: {password}");
            }
        }
    }
}
