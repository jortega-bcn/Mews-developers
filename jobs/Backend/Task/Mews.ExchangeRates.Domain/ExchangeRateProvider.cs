using Mews.ExchangeRates.Domain.Contracts;
using Mews.ExchangeRates.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace Mews.ExchangeRates.Domain
{
    public class ExchangeRateProvider(
        IExchangeRateReader exchangeRateReader, 
        ILogger<ExchangeRateProvider> logger) : IExchangeRateProvider
    {

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            logger.LogInformation("Reading Exchange Rates from source");
            var readRates = await ReadRatesFromSource(exchangeRateReader);
            return readRates.Where(r => currencies.Select(c => c.Code).Contains(r.SourceCurrency.Code));
        }

        private static async Task<IEnumerable<ExchangeRate>> ReadRatesFromSource(IExchangeRateReader exchangeRateReader)
        {
            try
            {
                var rates = await exchangeRateReader.GetExchangeRatesAsync();
                return rates;
            }
            catch (Exception ex)
            {
                throw new DataReadException(ex.Message, ex);
            }
        }
    }
}
