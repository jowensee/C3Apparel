using System.Collections.Generic;

namespace C3Apparel.Data.Pricing
{
    public interface IExchangeRateRetriever
    {
        ExchangeRateItem GetExchangeRate(string sourceCurrency, string targetCurrency);
        IEnumerable<ExchangeRateItem> GetAllExchangeRates(string[] sourceCurrencies);
    }
}