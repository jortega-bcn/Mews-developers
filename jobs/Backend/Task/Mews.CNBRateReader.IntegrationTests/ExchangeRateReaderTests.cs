using AutoFixture.Xunit2;
using Castle.Core.Logging;
using Mews.CzechNationalBankRateReader;
using Mews.ExchangeRates.Domain.Configuration;
using Mews.Reusable.UnitTests.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Globalization;

namespace Mews.CNBRateReader.IntegrationTests
{
    public class ExchangeRateReaderTests
    {
        [Theory]
        [InlineAutoNSubstituteData("CZK",
            "https://www.cnb.cz/en/financial-markets/foreign-exchange-market/central-bank-exchange-rate-fixing/central-bank-exchange-rate-fixing/daily.txt")]
        public async Task GetExchangeRatesAsync_RetrievesAListOfCurrencies(string expectedDestinationCurrencyCode,
            string url,
            [Frozen]  IOptions<ExchangeRateOptions> options,
            [Frozen] ILogger<ExchangeRateReader> logger
            )
        {
            //Prepare
            options.Value.Returns(new ExchangeRateOptions { SourceUri = url });
            var systemUnderTest = new ExchangeRateReader(
                                            new HttpClient(),
                                            new ResponseBodyParser(
                                                new FirstLineParser(),
                                                new ExchangeRateContentParser()),
                                            options,
                                            logger);
            //Act
            var result = await systemUnderTest.GetExchangeRatesAsync();
            
            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count() > 5);
            Assert.True(result.All(er => er.SourceCurrency.Code != expectedDestinationCurrencyCode));
            Assert.True(result.All(er => er.TargetCurrency.Code == expectedDestinationCurrencyCode));
            Assert.True(result.All(er => er.Value > 0m));
        }
    }
}