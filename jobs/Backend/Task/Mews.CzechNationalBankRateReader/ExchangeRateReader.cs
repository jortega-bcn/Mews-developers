using Mews.CzechNationalBankRateReader.Interfaces;
using Mews.ExchangeRates.Domain;
using Mews.ExchangeRates.Domain.Configuration;
using Mews.ExchangeRates.Domain.Contracts;
using Mews.ExchangeRates.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Data;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography.X509Certificates;

namespace Mews.CzechNationalBankRateReader
{
    public class ExchangeRateReader(HttpClient httpClient, 
        IResponseBodyParser responseBodyParser,
        IOptions<ExchangeRateOptions> options,
        ILogger<ExchangeRateReader> logger) 
        : IExchangeRateReader
    {
        const string TargetCurrencyCode = "CZK";

        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, options.Value.SourceUri);
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new DataReadException($"Reading exchange rates returned {response.StatusCode}");
            }
            var body = await response.Content.ReadAsStringAsync();            
            logger.LogDebug("read data: {body}", body);
            var rates = responseBodyParser.ParseBody(body);

            var targetCurrency = new Currency(TargetCurrencyCode);

            return rates.ExchangeRates.Select(er => new ExchangeRate(
                                                            new Currency(er.Code),
                                                            targetCurrency,
                                                            er.Rate / er.Amount));
        }
    }
}
