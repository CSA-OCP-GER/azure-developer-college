using System;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Adc.Scm.Search.Indexer.Startup))]

namespace Adc.Scm.Search.Indexer
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<ContactIndexerOptions>()
                .Configure<IConfiguration>((settings, configuration) => configuration.GetSection("ContactIndexerOptions").Bind(settings));

            builder.Services.AddScoped<ContactIndexerProcessor>();
        }
    }
}