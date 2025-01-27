using Mews.Reusable.UnitTests;
using Mews.Reusable.UnitTests.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mews.CzechNationalBankRateReader.UnitTests
{
    public class ExchangeRateContentParserTests
    {
        [Theory]
        [InlineAutoNSubstituteData("ExchangeRatesValidContent.txt", "CZK", 31)]
        public void ParseContent_returns_ok(string contentFileName,
            string expectedDestinationCurrencyCode,
            int expectedCount,
            ExchangeRateContentParser systemUnderTest)
        {
            var content = EmbeddedResourceFileReader.ReadFileContent(Assembly.GetExecutingAssembly(), contentFileName);
            var result = systemUnderTest.ParseContent(content);
            Assert.NotNull(result);
            Assert.True(result.Count() == expectedCount);
            Assert.True(result.All(er => !string.IsNullOrWhiteSpace(er.Country)));
            Assert.True(result.All(er => !string.IsNullOrWhiteSpace(er.Currency)));
            Assert.True(result.All(er => er.Amount > 0));
            Assert.True(result.All(er => er.Code != expectedDestinationCurrencyCode && er.Code != null));
            Assert.True(result.All(er => er.Rate > 0m));
        }

        [Theory]
        [InlineAutoNSubstituteData("ExchangeRatesInValidContent.txt")]
        public void ParseContent_returns_nok(string contentFileName,                                            
                                            ExchangeRateContentParser systemUnderTest)
        {
            var content = EmbeddedResourceFileReader.ReadFileContent(Assembly.GetExecutingAssembly(), contentFileName);
            //Act
            var myAction = () => systemUnderTest.ParseContent(content);
            //Assert
            Assert.Throws<CsvHelper.HeaderValidationException>(myAction);
        }
    }
}
