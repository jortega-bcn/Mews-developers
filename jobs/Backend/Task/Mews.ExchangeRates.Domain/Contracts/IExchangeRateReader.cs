using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.ExchangeRates.Domain.Contracts
{
    public interface IExchangeRateReader
    {
        Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync();
    }
}
