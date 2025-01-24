using Mews.ExchangeRates.Domain.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace Mews.ExchangeRates.Domain
{
    public class ExchangeRateProvider : IExchangeRateProvider
    {
        private readonly IExchangeRateReader _exchangeRateReader;
        private readonly ILogger<ExchangeRateProvider> _logger;

        public ExchangeRateProvider(IExchangeRateReader exchangeRateReader, ILogger<ExchangeRateProvider> logger)
        {
            _exchangeRateReader = exchangeRateReader;
            _logger = logger;
        }

        /// <summary>
        /// Should return exchange rates among the specified currencies that are defined by the source. But only those defined
        /// by the source, do not return calculated exchange rates. E.g. if the source contains "CZK/USD" but not "USD/CZK",
        /// do not return exchange rate "USD/CZK" with value calculated as 1 / "CZK/USD". If the source does not provide
        /// some of the currencies, ignore them.
        /// </summary>
        public IEnumerable<ExchangeRate> GetExchangeRates(IEnumerable<Currency> currencies)
        {
            return currencies.Select(c=> new ExchangeRate(c,c,123.456m));
        }

        public Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies)
        {
            var rates = GetExchangeRates(currencies);
            return Task.FromResult(rates);
        }
    }
}
