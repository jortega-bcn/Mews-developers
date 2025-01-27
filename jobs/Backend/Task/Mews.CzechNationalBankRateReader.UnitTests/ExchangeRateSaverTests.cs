using AutoFixture.Xunit2;
using Mews.ExchangeRates.Domain;
using Mews.ExchangeRates.Domain.Configuration;
using Mews.Reusable.UnitTests.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mews.CzechNationalBankRateReader.UnitTests;
public class ExchangeRateSaverTests
{
    [Theory, AutoNSubstituteData]
    public async Task SavesAndReadsCorrecly(
        List<ExchangeRate> data,
        [Frozen] IOptions<ExchangeRateOptions> options,
        ILogger<ExchangeRateSaver> logger)
    {
        var config = new ExchangeRateOptions();
        config.DataFilePath = "data/ExchangeRateSaverTests.json";
        options.Value.Returns(config);
        ExchangeRateSaver systemUnderTest = new ExchangeRateSaver(options, logger);
        await systemUnderTest.SaveExchangeRatesAsync(data);
        var read = await systemUnderTest.GetExchangeRatesAsync();
        Assert.True(data.SequenceEqual(read));
    }
}
