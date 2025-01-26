using System.Net;
using System.Text;

namespace Mews.CzechNationalBankRateReader.UnitTests.Mocks
{
    /// <summary>
    /// This class is used to mock HttpClient in unit tests.
    /// </summary>    
    public class CustomHttpClientHandler(HttpStatusCode returnCode, string content) : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(returnCode)
            {
                Content = new StringContent(content, Encoding.UTF8)
            };
            return Task.FromResult(response);
        }
    }
}