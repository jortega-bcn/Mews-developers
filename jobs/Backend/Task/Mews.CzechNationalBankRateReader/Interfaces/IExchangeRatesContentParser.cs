using Mews.CzechNationalBankRateReader.Models;

namespace Mews.CzechNationalBankRateReader.Interfaces
{
    public interface IExchangeRatesContentParser
    {
        IEnumerable<CentralBankExchangeRate> ParseContent(string content);
    }
}