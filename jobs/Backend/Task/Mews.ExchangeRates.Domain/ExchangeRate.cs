namespace Mews.ExchangeRates.Domain;

public class ExchangeRate(Currency sourceCurrency, Currency targetCurrency, decimal value, DateOnly date)
{
    public Currency SourceCurrency { get; } = sourceCurrency;

    public Currency TargetCurrency { get; } = targetCurrency;

    public decimal Value { get; } = value;

    public DateOnly Date { get; set; } = date;

    public override string ToString()
    {
        return $"1 {SourceCurrency} = {Value} {TargetCurrency} on {Date:yyyy-MM-dd}.";
    }

    public override bool Equals(object? obj)
    {
        return ToString() == obj?.ToString();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ToString());
    }
}
