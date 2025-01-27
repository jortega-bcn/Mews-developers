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
    public class ResponseBodyParser(IFirstLineParser firstLineParser, IExchangeRateContentParser contentParser): IResponseBodyParser
    {
        public const string EndOfLineChar="\n";

        public CentralBankResponse ParseBody(string body)
        {
            
            int endOfFirstLine = body.IndexOf(EndOfLineChar);
            if (endOfFirstLine < 0)
            {
                throw new InvalidBodyException(body);
            }
            string firstLine = body.Substring(0, endOfFirstLine);
            var result = new CentralBankResponse
            {
                Metadata = firstLineParser.ParseFirstLine(firstLine),
                ExchangeRates = contentParser.ParseContent(body.Substring(endOfFirstLine))
            };
            return result;
        }
    }
}
