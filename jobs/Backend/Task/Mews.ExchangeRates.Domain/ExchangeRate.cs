namespace Mews.ExchangeRates.Domain
{
    public class ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value)
    {
        public Currency SourceCurrency { get; } = sourceCurrency;

        public Currency TargetCurrency { get; } = targetCurrency;

        public decimal Value { get; } = value;

        public override string ToString()
        {
            return $"1 {SourceCurrency} = {Value} {TargetCurrency}";
        }

        public override bool Equals(object? obj)
        {
            return ToString() == obj?.ToString();
        }
    }
}
