using AutoFixture.Xunit2;
using Mews.ExchangeRates.Domain.Contracts;
using Mews.Reusable.UnitTests.Attributes;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mews.ExchangeRates.Domain.UnitTests;
public class FaultTolerantExchangeRateReaderTests()
{
    [Theory]
    [InlineAutoNSubstituteData()]
    public async Task GetExchangeRatesAsync_ReturnsSaved_WhenSavedForToday(
        List<ExchangeRate> savedRates,
        [Frozen] IExchangeRateSaver exchangeRateSaver,
        FaultTolerantExchangeRateReader systemUnderTest)
    {
        //Prepare
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        MockSaver(today, savedRates, exchangeRateSaver);

        //Act
        var result = await systemUnderTest.GetExchangeRatesAsync();

        //Assert
        Assert.True(result.SequenceEqual(savedRates));
    }
    [Theory]
    [InlineAutoNSubstituteData()]
    public async Task GetExchangeRatesAsync_ReturnsSaved_WhenExceptionReading(
        List<ExchangeRate> savedRates,
        [Frozen] IExchangeRateReader innerReader,
        [Frozen] IExchangeRateSaver exchangeRateSaver,
        FaultTolerantExchangeRateReader systemUnderTest)
    {
        //Prepare
        MockSaver(GetDateBeforeToday(), savedRates, exchangeRateSaver);
        innerReader.GetExchangeRatesAsync().ThrowsAsync(new Exception("testing"));

        //Act
        var result = await systemUnderTest.GetExchangeRatesAsync();

        //Assert
        Assert.True(result.SequenceEqual(savedRates));
    }

    [Theory]
    [InlineAutoNSubstituteData()]
    public async Task GetExchangeRatesAsync_ReadsNew_WhenSavedForBeforeToday(
        List<ExchangeRate> savedRates,
        List<ExchangeRate> readRates,
        [Frozen] IExchangeRateReader innerReader,
        [Frozen] IExchangeRateSaver exchangeRateSaver,
        FaultTolerantExchangeRateReader systemUnderTest)
    {
        //Prepare        
        MockSaver(GetDateBeforeToday(), savedRates, exchangeRateSaver);
        innerReader.GetExchangeRatesAsync().Returns(readRates);

        //Act
        var result = await systemUnderTest.GetExchangeRatesAsync();

        //Assert
        Assert.True(result.SequenceEqual(readRates));
        Received.InOrder(async () =>
        {
            await innerReader.GetExchangeRatesAsync();
            await exchangeRateSaver.SaveExchangeRatesAsync(readRates);
        }
        );
    }

    private static DateOnly GetDateBeforeToday()
    {
        return DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-5));
    }

    private static void MockSaver(DateOnly date, List<ExchangeRate> savedRates, IExchangeRateSaver exchangeRateSaver)
    {
        savedRates.ForEach(r => r.Date = date);
        exchangeRateSaver.GetExchangeRatesAsync().Returns(savedRates);
    }

    [Theory]
    [InlineAutoNSubstituteData()]
    public async Task GetExchangeRatesAsync_ReadsNew_WhenNotSaved(
        List<ExchangeRate> readRates,
        [Frozen] IExchangeRateReader innerReader,
        [Frozen] IExchangeRateSaver exchangeRateSaver,
        FaultTolerantExchangeRateReader systemUnderTest)
    {
        //Prepare
        var savedRates = new List<ExchangeRate>();
        exchangeRateSaver.GetExchangeRatesAsync().Returns(savedRates);
        innerReader.GetExchangeRatesAsync().Returns(readRates);

        //Act
        var result = await systemUnderTest.GetExchangeRatesAsync();

        //Assert
        Assert.True(result.SequenceEqual(readRates));
        Received.InOrder(async () =>
            {
                await innerReader.GetExchangeRatesAsync();
                await exchangeRateSaver.SaveExchangeRatesAsync(readRates);
            }
        );
        
    }

}
