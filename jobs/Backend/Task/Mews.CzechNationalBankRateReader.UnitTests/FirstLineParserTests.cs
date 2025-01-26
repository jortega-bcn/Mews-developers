using Mews.Reusable.UnitTests.Attributes;

namespace Mews.CzechNationalBankRateReader.UnitTests
{
    public class FirstLineParserTests
    {
        [Theory]
        [InlineAutoNSubstituteData("24 Jan 2025 #17", "2025-01-24", 17)]
        [InlineAutoNSubstituteData("2 Feb 2021 # 12", "2021-02-02", 12)]
        [InlineAutoNSubstituteData("29 Feb 2020 # 15", "2020-02-29", 15)]
        [InlineAutoNSubstituteData("5 Mar 2000 #29", "2000-03-05", 29)]
        [InlineAutoNSubstituteData("7 Apr 2100 # 19", "2100-04-07", 19)]
        [InlineAutoNSubstituteData("24 May 2025 #22", "2025-05-24", 22)]
        [InlineAutoNSubstituteData("20 Jun 2121 # 120", "2121-06-20", 120)]
        [InlineAutoNSubstituteData("4 Jul 2025 #4", "2025-07-04", 4)]
        [InlineAutoNSubstituteData("24 Dec 2125 #97", "2125-12-24", 97)]        
        public void ParseFirstLine_ParsesFirstRecord_WhenValid(string firstRecord, string expectedDate, int expectedSequence,
            FirstLineParser systemUndeTest)
        {
            var parsedRecord = systemUndeTest.ParseFirstLine(firstRecord);
            Assert.True(parsedRecord != null);
            Assert.True(DateOnly.Parse(expectedDate).Equals(parsedRecord.Date));
            Assert.Equal(expectedSequence, parsedRecord.YearlySequence);
        }

        [Theory]
        #region invalid dates
        [InlineAutoNSubstituteData("29 Feb 2021 # 15")]
        [InlineAutoNSubstituteData("5 Martes 2000 #29")]
        [InlineAutoNSubstituteData("7 Abrigo 2100 # 19")]
        [InlineAutoNSubstituteData("34 May 2025 #22")]
        [InlineAutoNSubstituteData("31 Jun 2121 # 120")]
        [InlineAutoNSubstituteData("4 JulyAug 2025 #4")]
        [InlineAutoNSubstituteData("22 Dic 2125 #97")]
        #endregion
        #region No # present to split content
        [InlineAutoNSubstituteData("29 Feb 2021 15")]
        [InlineAutoNSubstituteData("5 Martes 2000")]
        [InlineAutoNSubstituteData("4 Jul -2025 4")]
        #endregion
        [InlineAutoNSubstituteData("# 19")]
        [InlineAutoNSubstituteData("30 May 2025 #22.1")]
        #region incorrect number
        [InlineAutoNSubstituteData("30 Jun 2121 # 12s0")]
        [InlineAutoNSubstituteData("24 Dec 2125 #aa")]
        #endregion
        public void ParseFirstLine_Throws_WhenFirstRecordInValid(string firstRecord, FirstLineParser systemUndeTest)
        {
            var myTestAction = () => systemUndeTest.ParseFirstLine(firstRecord);
            Assert.Throws<FormatException>(myTestAction);            
        }

        
        
    }
}