namespace Mews.ExchangeRates.Domain.Contracts
{
    public interface IExchangeRateProvider
    {        
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync(IEnumerable<Currency> currencies);
    }
}