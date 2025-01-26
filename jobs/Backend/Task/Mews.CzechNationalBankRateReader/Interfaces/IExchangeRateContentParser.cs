using Mews.CzechNationalBankRateReader.Models;

namespace Mews.CzechNationalBankRateReader.Interfaces
{
    public interface IExchangeRateContentParser
    {
        IEnumerable<CentralBankExchangeRate> ParseContent(string content);
    }
}