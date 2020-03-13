using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace SubscriberAspNetCore.Controllers
{
    [ApiController]
    public class SubscriberController : ControllerBase
    {
        [Topic("mytopic")]
        [HttpPost("mytopic")]
        public async Task<IActionResult> HandleMessage(Message message)
        {
            var body = await new StreamReader(this.Request.Body).ReadToEndAsync();
            await Task.Delay(0);
            Console.WriteLine(message.Text);
            return Ok();
        }
    }
}