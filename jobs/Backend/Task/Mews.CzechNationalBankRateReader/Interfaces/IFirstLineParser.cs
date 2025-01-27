using Mews.CzechNationalBankRateReader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.CzechNationalBankRateReader.Interfaces
{
    public interface IFirstLineParser
    {
        FirstRecord ParseFirstLine(string firstLine);
    }
}
