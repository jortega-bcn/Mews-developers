using AutoFixture.Xunit2;
using Mews.CzechNationalBankRateReader.Exceptions;
using Mews.CzechNationalBankRateReader.Interfaces;
using Mews.CzechNationalBankRateReader.Models;
using Mews.ExchangeRates.Domain;
using Mews.Reusable.UnitTests.Attributes;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace Mews.CzechNationalBankRateReader.UnitTests
{
    public class ResponseBodyParserTests
    {
        [Theory]
        [InlineAutoNSubstituteData()]        
        public void ParseBody_IsSuccess_WhenValidContent(
            string firstLine,
            string followingLines,
            List<CentralBankExchangeRate> exchangeRates,
            FirstRecord firstRecord,
            [Frozen]IFirstLineParser firstLineParser, 
            [Frozen]IExchangeRatesContentParser contentParser,
            ResponseBodyParser systemUndeTest)
        {
            //Prepare
            contentParser.ParseContent(Arg.Any<string>()).Returns(exchangeRates);
            firstLineParser.ParseFirstLine(Arg.Any<string>()).Returns(firstRecord);
            
            //Act
            var parsedBody = systemUndeTest.ParseBody(string.Concat(firstLine,"\n",followingLines));
            
            //Assert
            Assert.True(parsedBody != null);
            Assert.True(firstRecord.Date.Equals(parsedBody.Metadata.Date));
            Assert.Equal(firstRecord.YearlySequence,parsedBody.Metadata.YearlySequence);
            Assert.Equal(exchangeRates.Count, parsedBody.ExchangeRates.Count());
        }

        [Theory]
        [InlineAutoNSubstituteData()]
        public void ParseBody_Throws_WhenInValidEndOfLines(string body,
                    ResponseBodyParser systemUndeTest)
        {
            //Check data pre-condition for this test case
            Assert.True(body.IndexOf("\n") < 0); 

            //Prepare
            var myTestAction = () => systemUndeTest.ParseBody(body);
            //Act
            var exception = Assert.Throws<InvalidBodyException>(myTestAction);
            //Assert
            Assert.Contains(body,exception.Message);
        }



        [Theory]
        [InlineAutoNSubstituteData()]
        public void ParseBody_Throws_WhenFirstRecordInValid(string firstLine,
            string followingLines,
            [Frozen] IFirstLineParser firstLineParser,
            ResponseBodyParser systemUndeTest)
        {
            //Prepare
            firstLineParser.ParseFirstLine(firstLine).Throws(new FormatException("mocked in unit test"));
            //Act
            var myTestAction = () => systemUndeTest.ParseBody(string.Concat(firstLine, "\n", followingLines));
            //Assert
            var exception = Assert.Throws<FormatException>(myTestAction);
        }

        [Theory]
        [InlineAutoNSubstituteData()]
        public void ParseBody_Throws_WhenContentInValid(string firstLine,
            string followingLines,
            FirstRecord firstRecord,
            [Frozen] IFirstLineParser firstLineParser,
            [Frozen] IExchangeRatesContentParser contentParser,
            ResponseBodyParser systemUndeTest)
        {
            //Prepare
            contentParser.ParseContent(Arg.Any<string>()).Throws(new FormatException("mocked in unit test"));
            firstLineParser.ParseFirstLine(Arg.Any<string>()).Returns(firstRecord);
            //Act
            var myTestAction = () => systemUndeTest.ParseBody(string.Concat(firstLine, "\n", followingLines));
            //Assert
            var exception = Assert.Throws<FormatException>(myTestAction);

        }


    }
}