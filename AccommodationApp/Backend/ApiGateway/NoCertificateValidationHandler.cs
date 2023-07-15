using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;

namespace ApiGateway
{
  
    public class NoCertificateValidationHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Disable certificate validation
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;

            using (HttpClient client = new HttpClient(handler))
            {
                // Forward the request
                return await client.SendAsync(request, cancellationToken);
            }
        }
    }

}
