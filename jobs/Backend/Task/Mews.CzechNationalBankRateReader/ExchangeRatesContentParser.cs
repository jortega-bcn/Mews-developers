using CsvHelper.Configuration;
using CsvHelper;
using Mews.CzechNationalBankRateReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mews.CzechNationalBankRateReader.Interfaces;

namespace Mews.CzechNationalBankRateReader
{
    public class ExchangeRatesContentParser: IExchangeRatesContentParser
    {
        public IEnumerable<CentralBankExchangeRate> ParseContent(string content)
        {
            var config = CsvConfiguration.FromAttributes<CentralBankExchangeRate>();
            using (var reader = new StringReader(content))
            using (var csv = new CsvReader(reader, config))
            {
                List<CentralBankExchangeRate> records = csv.GetRecords<CentralBankExchangeRate>().ToList();
                return records;
            }
        }
    }
}
