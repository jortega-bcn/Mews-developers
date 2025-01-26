using Mews.Reusable.UnitTests.Attributes;

namespace Mews.ExchangeRates.Domain.UnitTests
{
    public class ExchangeRateProviderTests
    {
        // todo: Write unit tests
        [Theory, InlineAutoNSubstituteData("2024-10-15", "2023-10-12")]
        public void Test1(string a, string b)
        {
            Assert.Equal(a, b);
        }
    }
}