﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InstantaneousGram_Authentication.Controllers
{
    [Route("api/Value")]
    [ApiController]
    [Authorize]
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
