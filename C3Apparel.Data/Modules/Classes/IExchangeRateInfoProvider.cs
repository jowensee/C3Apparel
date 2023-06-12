
using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IExchangeRateInfoProvider
    {
        ExchangeRateInfo GetCurrentExchangeRate(string sourceCurrency);
        IEnumerable<ExchangeRateInfo> GetAllExchangeRates(int pageNumber, int defaultPageSize);
        int GetAllExchangeRatesCount();
        ExchangeRateInfo GetExchangerRate(int requestsId);
        void Delete(int id);
        void Insert(ExchangeRateInfo exchangeRate);
        void Update(ExchangeRateInfo exchangeRate);
    }
}