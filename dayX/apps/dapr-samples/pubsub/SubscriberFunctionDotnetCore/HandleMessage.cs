using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SubscriberFunctionDotnetCore
{
    public static class HandleMessage
    {
        [FunctionName("HandleMessage")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "mytopic")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            var data = JObject.Parse(requestBody)["data"].ToString();
            var msg = JsonConvert.DeserializeObject<Message>(data);

            log.LogInformation(msg.Text);

            return new OkResult();
        }
    }
}
