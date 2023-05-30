namespace C3Apparel.Data.Pricing
{
    public class ExchangeRateItem
    {
        public string SourceCurrency { get; }
        public string TargetCurrency { get; }
        public decimal Rate { get; }

        public ExchangeRateItem(string sourceCurrency, string targetCurrency, decimal rate)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Rate = rate;
        }
    }
}