using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.CzechNationalBankRateReader.Models
{
    public class CentralBankResponse
    {
        public FirstRecord? Metadata { get; set; }
        public IEnumerable<CentralBankExchangeRate>? ExchangeRates { get; set; }
    }
}
