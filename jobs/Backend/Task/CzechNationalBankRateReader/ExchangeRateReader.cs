using Mews.ExchangeRates.Domain;
using Mews.ExchangeRates.Domain.Contracts;

namespace CzechNationalBankRateReader
{
    public class ExchangeRateReader : IExchangeRateReader
    {
        public ExchangeRateReader(HttpClient httpClient, )
        public Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
        {
            
        }
    }
}
