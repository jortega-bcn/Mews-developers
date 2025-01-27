using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.CzechNationalBankRateReader.Models
{
    [Delimiter("|")]
    [CultureInfo("en-US")]
    public class CentralBankExchangeRate
    {
        public string Country { get; set; } = null!;
        public string Currency { get; set; } = null!;
        public int Amount { get; set; }
        public string Code { get; set; } = null!;
        public decimal Rate { get; set; }
    }
}
