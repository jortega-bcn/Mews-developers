using AutoFixture.Xunit2;
using Mews.ExchangeRates.Domain.Contracts;
using Mews.ExchangeRates.Domain.Exceptions;
using Mews.Reusable.UnitTests.Attributes;
using Mews.Reusable.UnitTests;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System.ComponentModel.DataAnnotations;

namespace Mews.ExchangeRates.Domain.UnitTests;

public class ExchangeRateProviderTests
{

    [Theory, InlineAutoNSubstituteData()]
    public async Task GetExchangeRatesAsync_Throws_WhenReadFails(
        string testReadExceptionMessage,
        List<Currency> currencies,
        [Frozen] IFaultTolerantExchangeRateReader exchangeRateReader,
        ExchangeRateProvider systemUnderTest
        )
    {
        //Prepare
        exchangeRateReader.GetExchangeRatesAsync().Throws(new Exception(testReadExceptionMessage));

        //Act
        var GetExchangeRatesAction = async () => await systemUnderTest.GetExchangeRatesAsync(currencies);

        //Assert
        var exception = await Assert.ThrowsAsync<DataReadException>(GetExchangeRatesAction);
        Assert.Equal(exception.InnerException!.Message, testReadExceptionMessage);
    }

    [Theory, InlineAutoNSubstituteData()]
    public async Task GetExchangeRatesAsync_ReturnsEmptyList_WhenRequestedCurrenciesNotFound(
        List<Currency> currencies,
        List<ExchangeRate> readExchangeRates,
        [Frozen] IExchangeRateReader exchangeRateReader,
        ExchangeRateProvider systemUnderTest)
    {
        //Prepare
        exchangeRateReader.GetExchangeRatesAsync().Returns(readExchangeRates);

        //Act
        var result = await systemUnderTest.GetExchangeRatesAsync(currencies);

        //Assert       
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Theory, InlineAutoNSubstituteData()]
    public async Task GetExchangeRatesAsync_ReturnsOneElement_WhenARequestedCurrencyFound(
        List<Currency> currencies,
        List<ExchangeRate> readExchangeRates,
        [Frozen] IFaultTolerantExchangeRateReader exchangeRateReader,
        ExchangeRateProvider systemUnderTest)
    {
        //Prepare
        var selectedCurrencyCode = readExchangeRates.First().SourceCurrency.Code;
        currencies.Add(new Currency(selectedCurrencyCode));        
        exchangeRateReader.GetExchangeRatesAsync().Returns(readExchangeRates);

        //Act
        var result = await systemUnderTest.GetExchangeRatesAsync(currencies);

        //Assert       
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Single(result);
        var resultExchangeRate = result.Single();
        Assert.NotNull(resultExchangeRate);
        Assert.Equal(selectedCurrencyCode, resultExchangeRate.SourceCurrency.Code);
        Assert.NotEqual(selectedCurrencyCode, resultExchangeRate.TargetCurrency.Code);
    }

    [Theory, InlineAutoNSubstituteData()]
    public async Task GetExchangeRatesAsync_ReturnsCorrectElements_WhenRequestedCurrenciesFound(
        List<ExchangeRate> nonMatchingReadExchangeRates,
        List<ExchangeRate> matchingReadExchangeRates,
        [Frozen] IFaultTolerantExchangeRateReader exchangeRateReader,
        ExchangeRateProvider systemUnderTest)
    {
        //Prepare        
        var currencies = matchingReadExchangeRates.Select(x => new Currency(x.SourceCurrency.Code));
        var readExchangeRates = matchingReadExchangeRates.Concat(nonMatchingReadExchangeRates).Shuffle();
        exchangeRateReader.GetExchangeRatesAsync().Returns(readExchangeRates);

        //Act
        var result = await systemUnderTest.GetExchangeRatesAsync(currencies);

        //Assert       
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(matchingReadExchangeRates.Count, result.Count());
        
        //For each element, there is only one with same currency Code and it is the same instance
        Assert.True(matchingReadExchangeRates.All(er => result.Single(resultER => resultER.SourceCurrency.Code == er.SourceCurrency.Code) == er));
        
        Func<ExchangeRate, string> sourceCurrencyCode = er => er.SourceCurrency.Code;
        Assert.True(result.OrderBy(sourceCurrencyCode).SequenceEqual(matchingReadExchangeRates.OrderBy(sourceCurrencyCode)));
    }


}
