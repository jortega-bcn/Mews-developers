using Mews.CzechNationalBankRateReader.Interfaces;
using Mews.CzechNationalBankRateReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.CzechNationalBankRateReader
{
    public class FirstLineParser : IFirstLineParser
    {
        public FirstRecord ParseFirstLine(string firstLine)
        {
            var firstLineValues = firstLine.Split('#');
            var firstRecord = new FirstRecord
            {
                Date = DateOnly.Parse(firstLineValues[0]),
                YearlySequence = int.Parse(firstLineValues[1]),
            };
            return firstRecord;
        }
    }
}
