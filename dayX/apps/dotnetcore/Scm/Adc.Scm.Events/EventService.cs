using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Dapr;

namespace Adc.Scm.Events
{
    public class EventService
    {
        private readonly EventServiceOptions _options;
        private readonly PublishClient _publishClient;

        public EventService(IOptions<EventServiceOptions> options, PublishClient publishClient)
        {
            _options = options.Value;
            _publishClient = publishClient;
        }

        public async Task Send(EventBase evt)
        {
            await _publishClient.PublishEventAsync(_options.TopicName, evt);
        }
    }
}
