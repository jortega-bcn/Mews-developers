using Mews.CzechNationalBankRateReader.Models;

namespace Mews.CzechNationalBankRateReader.Interfaces
{
    public interface IResponseBodyParser
    {
        CentralBankResponse ParseBody(string body);
    }
}