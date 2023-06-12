
using System.Collections.Generic;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Sql;

namespace C3Apparel.Data.Modules.Classes
{
    public class ExchangeRateInfoProvider : BaseRepository, IExchangeRateInfoProvider
    {
        public ExchangeRateInfoProvider(IConfigurationService configurationService) : base(configurationService)
        {
        }
        public ExchangeRateInfo GetCurrentExchangeRate(string sourceCurrency)
        {
            var sSql =
                $@"SELECT TOP 1 * FROM C3_ExchangeRate WHERE ({nameof(ExchangeRateInfo.ExchangeRateSourceCurrency)} = '{sourceCurrency}') AND ({nameof(ExchangeRateInfo.ExchangeRateValidFrom)} IS NULL OR {nameof(ExchangeRateInfo.ExchangeRateValidFrom)} <=GETDATE())
                                    AND ({nameof(ExchangeRateInfo.ExchangeRateValidTo)} IS NULL OR {nameof(ExchangeRateInfo.ExchangeRateValidTo)} >=GETDATE())";

            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return null;
            }

            var row = ds.Tables[0].Rows[0];

            return new ExchangeRateInfo
            {
                ExchangeRateSourceCurrency = row[nameof(ExchangeRateInfo.ExchangeRateSourceCurrency)].ToSafeString(),
                ExchangeRateAUDValue = row[nameof(ExchangeRateInfo.ExchangeRateAUDValue)].ToDecimal(),
                ExchangeRateNZDValue = row[nameof(ExchangeRateInfo.ExchangeRateNZDValue)].ToDecimal(),
            };
        }

        public IEnumerable<ExchangeRateInfo> GetAllExchangeRates(int pageNumber, int defaultPageSize)
        {
            throw new System.NotImplementedException();
        }

        public int GetAllExchangeRatesCount()
        {
            throw new System.NotImplementedException();
        }

        public ExchangeRateInfo GetExchangerRate(int requestsId)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new System.NotImplementedException();
        }

        public void Insert(ExchangeRateInfo exchangeRate)
        {
            throw new System.NotImplementedException();
        }

        public void Update(ExchangeRateInfo exchangeRate)
        {
            throw new System.NotImplementedException();
        }
    }
}