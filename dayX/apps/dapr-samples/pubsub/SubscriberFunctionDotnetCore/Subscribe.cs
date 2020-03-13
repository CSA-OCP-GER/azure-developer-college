using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace SubscriberFunctionDotnetCore
{
    public static class Subscribe
    {
        [FunctionName("Subscribe")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dapr/subscribe")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            await Task.Delay(0);
            string [] subscriptions = { "mytopic" };
            return new OkObjectResult(subscriptions);
        }
    }
}
