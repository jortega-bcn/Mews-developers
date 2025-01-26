using CsvHelper.Configuration;
using CsvHelper;
using Mews.CzechNationalBankRateReader.Exceptions;
using Mews.CzechNationalBankRateReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mews.CzechNationalBankRateReader.Interfaces;

namespace Mews.CzechNationalBankRateReader
{
    public class ResponseBodyParser(IFirstLineParser firstLineParser, IExchangeRatesContentParser contentParser): IResponseBodyParser
    {
        const string EndOfLineChar="\n";

        public CentralBankResponse ParseBody(string body)
        {
            var result = new CentralBankResponse();
            int endOfFirstLine = body.IndexOf(EndOfLineChar);
            if (endOfFirstLine < 1)
            {
                throw new InvalidBodyException(body);
            }
            string firstLine = body.Substring(0, endOfFirstLine);
            result.Metadata = firstLineParser.ParseFirstLine(firstLine);
            result.ExchangeRates = contentParser.ParseContent(body.Substring(endOfFirstLine));
            return result;
        }
    }
}
