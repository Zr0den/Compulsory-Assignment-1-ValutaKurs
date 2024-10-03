public class ExchangeRate
{
    public string CurrencyCode { get; set; }

    //The conversion rate of the currency from the base currency
    public decimal Value { get; set; }

    public DateTime Timestamp { get; set; }

    public ExchangeRate()
    {
        Value = 1.0m;
    }

    public override string ToString()
    {
        return $"{CurrencyCode} : {Value}";
    }
}