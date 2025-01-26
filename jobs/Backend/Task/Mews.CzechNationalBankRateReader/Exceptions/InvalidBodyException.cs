using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.CzechNationalBankRateReader.Exceptions
{
    public class InvalidBodyException(string body): Exception($"The body is {body}");
}
