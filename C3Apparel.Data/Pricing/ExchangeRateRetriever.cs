using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Modules.Classes;

namespace C3Apparel.Data.Pricing
{
    public class ExchangeRateRetriever : IExchangeRateRetriever
    {
        private readonly IExchangeRateInfoProvider _exchangeRateInfoProvider;
        public ExchangeRateRetriever(IExchangeRateInfoProvider exchangeRateInfoProvider)
        {
            _exchangeRateInfoProvider = exchangeRateInfoProvider;
        }

        private bool IsValidTargetCurrency(string targetCurrency)
        {
            return targetCurrency == CurrencyConstants.AUD || targetCurrency == CurrencyConstants.NZD;
        }
        public ExchangeRateItem GetExchangeRate(string sourceCurrency, string targetCurrency)
        {
            if (!IsValidTargetCurrency(targetCurrency))
            {
                return null;
            }
            
            var whereCondition = 
                $@"({nameof(ExchangeRateInfo.ExchangeRateSourceCurrency)} = '{sourceCurrency}') AND ({nameof(ExchangeRateInfo.ExchangeRateValidFrom)} IS NULL OR {nameof(ExchangeRateInfo.ExchangeRateValidFrom)} <=GETDATE())
                                    AND ({nameof(ExchangeRateInfo.ExchangeRateValidTo)} IS NULL OR {nameof(ExchangeRateInfo.ExchangeRateValidTo)} >=GETDATE())";
            var exchangeItem = _exchangeRateInfoProvider.Get(whereCondition).FirstOrDefault();

            if (exchangeItem == null)
            {
                return null;
            }

            var rate = exchangeItem.ExchangeRateAUDValue;

            if (targetCurrency == CurrencyConstants.NZD)
            {
                rate = exchangeItem.ExchangeRateNZDValue;
            }

            return new ExchangeRateItem(sourceCurrency, targetCurrency, rate);
        }

        public IEnumerable<ExchangeRateItem> GetAllExchangeRates(string[] sourceCurrencies)
        {
            var rates = new List<ExchangeRateItem>();
            foreach (var sourceCurrency in sourceCurrencies)
            {

                var rate = GetExchangeRate(sourceCurrency, CurrencyConstants.AUD);

                if (rate != null)
                {
                    rates.Add(rate);
                }
                
                rate = GetExchangeRate(sourceCurrency, CurrencyConstants.NZD);

                if (rate != null)
                {
                    rates.Add(rate);
                }
            }

            return rates;
        }
    }
}