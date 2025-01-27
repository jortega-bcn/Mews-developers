using Mews.ExchangeRates.Domain.Contracts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.ExchangeRates.Domain;
public class FaultTolerantExchangeRateReader(
    IExchangeRateReader innerReader,
    IExchangeRateSaver exchangeRateSaver,
    ILogger<FaultTolerantExchangeRateReader> logger) : IFaultTolerantExchangeRateReader
{    
    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var cachedRates = await exchangeRateSaver.GetExchangeRatesAsync();
        if (cachedRates.Any())
        {
            //We may need to check date for each rate individually
            if (today.Equals(cachedRates.Max(r => r.Date)))
            {
                logger.LogInformation("Returning cached rates.");
                return cachedRates;
            }
        }
        IEnumerable<ExchangeRate> readRates = await ReadNewRates();        
        return readRates;
    }

    private async Task<IEnumerable<ExchangeRate>> ReadNewRates()
    {
        try
        {
            var read = await innerReader.GetExchangeRatesAsync();
            await SaveRates(exchangeRateSaver, read);
            return read;
        }
        catch (Exception  ex) 
        {
            logger.LogWarning(ex, "Error occurred while reading Rates. Returning cached rates.");
            return await exchangeRateSaver.GetExchangeRatesAsync();
        }
    }

    private async Task SaveRates(IExchangeRateSaver exchangeRateSaver, IEnumerable<ExchangeRate> read)
    {
        try
        {
            await exchangeRateSaver.SaveExchangeRatesAsync(read);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error occurred while saving Rates.");
        }
    }
}
