using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Adc.Scm.Search.Indexer
{
    public class ContactIndexer
    {
        private readonly ContactIndexerProcessor _processor;

        public ContactIndexer(ContactIndexerProcessor processor)
        {
            _processor = processor;
        }

        [FunctionName("ContactIndexer")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "scmcontacts")]HttpRequest req, ILogger log)
        {
            string body = await new StreamReader(req.Body).ReadToEndAsync();
            var data = JObject.Parse(body)["data"].ToString();
            var msg = JsonConvert.DeserializeObject<ContactMessage>(data);
            //await _processor.Process(msg);
         
            log.LogInformation($"C# ServiceBus topic trigger function processed message: {msg.EventType}");
            return new OkResult();
        }

        [FunctionName("Subscribe")]
        public async Task<IActionResult> Subscribe([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "dapr/subscribe")]HttpRequest request, ILogger log)
        {
            await Task.Delay(0);
            string [] subscriptions = {"scmcontacts"};
            return new OkObjectResult(JsonConvert.SerializeObject(subscriptions));
        }
    }
}
