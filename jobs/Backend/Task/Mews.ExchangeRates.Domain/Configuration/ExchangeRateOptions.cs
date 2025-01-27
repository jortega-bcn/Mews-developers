using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.ExchangeRates.Domain.Configuration;

public class ExchangeRateOptions
{
    [Required]
    [NotNull]
    public string[]? Currencies { get; set; }

    /// <summary>
    /// This configuration does not belong to domain, but infra. I decided to keep it in the same place for simplicity.
    /// </summary>
    [Required]
    [NotNull]
    public string? SourceUri { get; set; }

    /// <summary>
    /// This configuration does not belong to domain, but infra
    /// </summary>

    [Required]
    [NotNull]
    public string? DataFilePath { get; set; }
}
