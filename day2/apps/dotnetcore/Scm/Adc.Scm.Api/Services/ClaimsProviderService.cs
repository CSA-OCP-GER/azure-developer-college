using Microsoft.AspNetCore.Http;
using System;

namespace Adc.Scm.Api.Services
{
    /// <summary>
    /// Provides access to claims of current user.
    /// </summary>
    public class ClaimsProviderService
    {
        public Guid GetUserId(HttpContext context)
        {
            // We have not integrated Identity yet. So we just return an empty Guid
            return Guid.Empty;
        }
    }
}
