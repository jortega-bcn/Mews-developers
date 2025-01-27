using AutoFixture.Xunit2;
using Mews.CzechNationalBankRateReader.Interfaces;
using Mews.CzechNationalBankRateReader.Models;
using Mews.ExchangeRates.Domain.Configuration;
using Mews.ExchangeRates.Domain.Exceptions;
using Mews.Reusable.UnitTests.Attributes;
using Mews.CzechNationalBankRateReader.UnitTests.Mocks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Mews.CzechNationalBankRateReader.UnitTests
{
    public class ExchangeRateReaderTests
    {
        const string _validUri = "https://localhost";

        [Theory]
        [InlineAutoNSubstituteData(HttpStatusCode.BadRequest)]
        [InlineAutoNSubstituteData(HttpStatusCode.Unauthorized)]
        [InlineAutoNSubstituteData(HttpStatusCode.InternalServerError)]
        public async Task GetExchangeRates_Throws_When_ResponseHttpCodeIsError(
             HttpStatusCode responseCode,
             string responseBody,
             [Frozen] IResponseBodyParser responseBodyParser,
             [Frozen] IOptions<ExchangeRateOptions> options,
             [Frozen] ILogger<ExchangeRateReader> logger
            )
        {
            //Prepare
            options.Value.Returns(new ExchangeRateOptions { SourceUri = _validUri });
            var client = new HttpClient(new CustomHttpClientHandler(responseCode,responseBody));
            var systemUnderTest= new ExchangeRateReader(client, responseBodyParser, options, logger );

            //Act 
            var GetExchangeRatesAction = async () => await systemUnderTest.GetExchangeRatesAsync();
            var exception = await Assert.ThrowsAsync<DataReadException>(GetExchangeRatesAction);

            //Assert            
            Assert.Contains(responseCode.ToString(),exception.Message);
        }

        [Theory]
        [InlineAutoNSubstituteData(HttpStatusCode.OK)]
        public async Task GetExchangeRates_Throws_When_ResponseBodyParserThrows(
            HttpStatusCode responseCode,
             string responseBody,
             string exceptionTestMessage,
             [Frozen] IResponseBodyParser responseBodyParser,
             [Frozen] IOptions<ExchangeRateOptions> options,
             [Frozen] ILogger<ExchangeRateReader> logger
            )
        {
            //Prepare
            options.Value.Returns(new ExchangeRateOptions { SourceUri = _validUri });
            responseBodyParser.ParseBody(responseBody).Throws(new Exception(exceptionTestMessage));
            var client = new HttpClient(new CustomHttpClientHandler(responseCode, responseBody));            
            var systemUnderTest = new ExchangeRateReader(client, responseBodyParser, options, logger);  
            
            //Act
            var GetExchangeRatesAction = async () => await systemUnderTest.GetExchangeRatesAsync();
            var exception = await Assert.ThrowsAnyAsync<Exception>(GetExchangeRatesAction);
            
            //Assert
            Assert.Contains(exceptionTestMessage, exception.Message);
        }
        [Theory]
        [InlineAutoNSubstituteData(HttpStatusCode.OK)]
        public async Task GetExchangeRates_Throws_When_NoExchangeRatesParsed(
            HttpStatusCode responseCode,
             string responseBody,
             CentralBankResponse parsedResponse,
             [Frozen] IResponseBodyParser responseBodyParser,
             [Frozen] IOptions<ExchangeRateOptions> options,
             [Frozen] ILogger<ExchangeRateReader> logger
            )
        {
            //Prepare
            options.Value.Returns(new ExchangeRateOptions { SourceUri = _validUri });
            parsedResponse.ExchangeRates = null;
            responseBodyParser.ParseBody(responseBody).Returns(parsedResponse);
            var client = new HttpClient(new CustomHttpClientHandler(responseCode, responseBody));
            var systemUnderTest = new ExchangeRateReader(client, responseBodyParser, options, logger);

            //Act
            var GetExchangeRatesAction = async () => await systemUnderTest.GetExchangeRatesAsync();
            var exception = await Assert.ThrowsAsync<DataReadException>(GetExchangeRatesAction);

            //Assert
            Assert.NotEmpty(exception.Message);
        }
        [Theory]
        [InlineAutoNSubstituteData(HttpStatusCode.OK)]
        public async Task GetExchangeRates_Throws_When_NoMetadataParsed(
            HttpStatusCode responseCode,
             string responseBody,
             CentralBankResponse parsedResponse,
             [Frozen] IResponseBodyParser responseBodyParser,
             [Frozen] IOptions<ExchangeRateOptions> options,
             [Frozen] ILogger<ExchangeRateReader> logger
            )
        {
            //Prepare
            options.Value.Returns(new ExchangeRateOptions { SourceUri = _validUri });
            parsedResponse.Metadata = null;
            responseBodyParser.ParseBody(responseBody).Returns(parsedResponse);
            var client = new HttpClient(new CustomHttpClientHandler(responseCode, responseBody));
            var systemUnderTest = new ExchangeRateReader(client, responseBodyParser, options, logger);

            //Act
            var GetExchangeRatesAction = async () => await systemUnderTest.GetExchangeRatesAsync();
            var exception = await Assert.ThrowsAsync<DataReadException>(GetExchangeRatesAction);
            
            //Assert
            Assert.NotEmpty(exception.Message);
        }

        [Theory]
        [InlineAutoNSubstituteData(HttpStatusCode.OK)]
        public async Task GetExchangeRates_ReturnsList_When_RatesParsed(
                HttpStatusCode responseCode,
                string responseBody,
                CentralBankResponse parsedResponse,
                [Frozen] IResponseBodyParser responseBodyParser,
                [Frozen] IOptions<ExchangeRateOptions> options,
                [Frozen] ILogger<ExchangeRateReader> logger
                )
        {
            //Prepare
            options.Value.Returns(new ExchangeRateOptions { SourceUri = _validUri });
            responseBodyParser.ParseBody(responseBody).Returns(parsedResponse);
            var client = new HttpClient(new CustomHttpClientHandler(responseCode, responseBody));
            var systemUnderTest = new ExchangeRateReader(client, responseBodyParser, options, logger);

            //Act
            var result = await systemUnderTest.GetExchangeRatesAsync();

            //Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result);
            Assert.True(result.Count() == parsedResponse!.ExchangeRates!.Count());
            var parsedCurrencyCodes = parsedResponse!.ExchangeRates!.Select(per => per.Code);
            Assert.True(result.All(er => parsedCurrencyCodes.Contains(er.SourceCurrency.Code)));
            Assert.True(result.All(er => er.SourceCurrency.Code != ExchangeRateReader.TargetCurrencyCode));
            Assert.True(result.All(er => er.TargetCurrency.Code == ExchangeRateReader.TargetCurrencyCode));
            Assert.True(result.All(er => er.Value != 0m));
            
        }
    }
}
