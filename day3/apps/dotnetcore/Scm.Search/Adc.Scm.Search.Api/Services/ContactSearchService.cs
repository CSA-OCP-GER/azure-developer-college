﻿using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Adc.Scm.Search.Api.Services
{
    public class ContactSearchService
    {
        private readonly ContactSearchOptions _options;

        public ContactSearchService(IOptions<ContactSearchOptions> options)
        {
            _options = options.Value;
        }

        public async Task<object> Search(Guid userId, string phrase)
        {
            var client = new SearchServiceClient(_options.ServiceName, new SearchCredentials(_options.AdminApiKey));
            var indexClient = client.Indexes.GetClient(_options.IndexName);
            var result = await indexClient.Documents.SearchAsync($"(UserId:{userId.ToString()}) AND ({phrase})", 
                new SearchParameters() 
                {
                    QueryType = QueryType.Full
                });

            return result.Results;
        }
    }
}
