using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.ExchangeRates.Domain.Exceptions
{
    public class DataReadException(string message, Exception? innerException) : Exception(message, innerException)
    {
        public DataReadException(string message) : this(message, null) { }
    }
}
