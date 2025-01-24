using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.ExchangeRates.Domain.Configuration
{
    public class ExchangeRateOptions
    {
        [Required]
        [NotNull]
        public string[] Currencies { get; set; } = new string[0];
    }
}
