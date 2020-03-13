using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Dapr;
using Microsoft.AspNetCore.Http;

namespace PublisherAspNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PublisherController : ControllerBase
    {
        private readonly PublishClient _publishClient; 

        public PublisherController(PublishClient client)
        {
            _publishClient = client;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Consumes("application/json")]
        public async Task<IActionResult> SendMessage([FromBody]string message)
        {
            var msg = new Message { Text = message };
            await _publishClient.PublishEventAsync("mytopic", msg);
            return Ok();
        }
    }
}
