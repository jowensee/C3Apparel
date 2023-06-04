
using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IExchangeRateInfoProvider
    {
        ExchangeRateInfo GetCurrentExchangeRate(string sourceCurrency);
    }
}