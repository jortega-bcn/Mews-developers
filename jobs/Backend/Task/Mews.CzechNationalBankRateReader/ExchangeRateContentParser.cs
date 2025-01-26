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
    public class ExchangeRateContentParser: IExchangeRateContentParser
    {
        public IEnumerable<CentralBankExchangeRate> ParseContent(string content)
        {
            var config = CsvConfiguration.FromAttributes<CentralBankExchangeRate>();
            using var reader = new StringReader(content);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecords<CentralBankExchangeRate>();
            // records need to be read before csv (Reader) will be disposed by using statement
            return records.ToList();
        }
    }
}
