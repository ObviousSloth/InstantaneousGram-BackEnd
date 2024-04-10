using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InstantaneousGram_Login.Controllers
{
    [Route("api/Value")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET: api/<Value>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}
