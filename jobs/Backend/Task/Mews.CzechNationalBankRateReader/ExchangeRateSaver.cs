using Mews.ExchangeRates.Domain;
using Mews.ExchangeRates.Domain.Configuration;
using Mews.ExchangeRates.Domain.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mews.CzechNationalBankRateReader;
public class ExchangeRateSaver(IOptions<ExchangeRateOptions> options, ILogger<ExchangeRateSaver> logger) : IExchangeRateSaver
{
    private static readonly Encoding _encoding = Encoding.UTF8;    
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };
    private readonly string _fileLocation = options.Value.DataFilePath;

    public async Task<IEnumerable<ExchangeRate>> GetExchangeRatesAsync()
    {
        if(!File.Exists(this._fileLocation)) 
            return [];
        logger.LogDebug("Reading Data From {fileLocation}", _fileLocation);
        var readContentFile = await File.ReadAllTextAsync(_fileLocation, _encoding);
        if(string.IsNullOrWhiteSpace(readContentFile))
            return [];
        return JsonSerializer.Deserialize<IEnumerable<ExchangeRate>>(readContentFile!) ?? []; 
    }

    public async Task SaveExchangeRatesAsync(IEnumerable<ExchangeRate> exchangeRates)
    {        
        logger.LogInformation("Saving Data To {fileLocation}",_fileLocation);
        var data = JsonSerializer.Serialize(exchangeRates, _jsonSerializerOptions);
        await File.WriteAllTextAsync(_fileLocation, data, _encoding);
    }

}
