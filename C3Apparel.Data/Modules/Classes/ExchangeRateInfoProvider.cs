
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

        public IEnumerable<ExchangeRateInfo> GetAllExchangeRates(int pageNumber, int itemsPerPage)
        {
            var sSql = $@"SELECT * FROM C3_ExchangeRate
                        ORDER BY ExchangeRateValidFrom DESC";

            if (itemsPerPage > 0)
            {
                sSql += $" OFFSET {(pageNumber - 1) * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY";
            }
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return Enumerable.Empty<ExchangeRateInfo>();
            }

            
            var results = new List<ExchangeRateInfo>();

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                results.Add(CreateExchangeRateInfo(ds.Tables[0].Rows[i]));
            }

            return results;
        }

        private ExchangeRateInfo CreateExchangeRateInfo(DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            return new ExchangeRateInfo
            {
                ExchangeRateID = row[nameof(ExchangeRateInfo.ExchangeRateID)].ToInt(),
                ExchangeRateSourceCurrency = row[nameof(ExchangeRateInfo.ExchangeRateSourceCurrency)].ToSafeString(),
                ExchangeRateAUDValue = row[nameof(ExchangeRateInfo.ExchangeRateAUDValue)].ToDecimal(),
                ExchangeRateNZDValue = row[nameof(ExchangeRateInfo.ExchangeRateNZDValue)].ToDecimal(),
                ExchangeRateValidFrom = row[nameof(ExchangeRateInfo.ExchangeRateValidFrom)].ToDateTime(),
                ExchangeRateValidTo =  row[nameof(ExchangeRateInfo.ExchangeRateValidTo)].ToDateTime(),
            };
        }
        public int GetAllExchangeRatesCount()
        {
            var sSql = $@"SELECT COUNT(*) FROM C3_ExchangeRate";

           
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return 0;
            }

            return ds.Tables[0].Rows[0][0].ToInt();

        }

        public ExchangeRateInfo GetExchangerRate(int id)
        {
            var sSql = $"SELECT * FROM C3_ExchangeRate WHERE ExchangeRateId={id}";

         
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return null;
            }

            return CreateExchangeRateInfo(ds.Tables[0].Rows[0]);
        }

        public void Delete(int id)
        {
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            ExecuteCommand("DELETE FROM C3_ExchangeRate WHERE ExchangeRateID = @Id", parameters);
        }

        public void Insert(ExchangeRateInfo exchangeRate)
        {
            
            var parameters = new Dictionary<string, object>
            {
                { "@ExchangeRateSourceCurrency", exchangeRate.ExchangeRateSourceCurrency },
                { "@ExchangeRateAUDValue", exchangeRate.ExchangeRateAUDValue },
                { "@ExchangeRateNZDValue", exchangeRate.ExchangeRateNZDValue },
                { "@ExchangeRateValidFrom", exchangeRate.ExchangeRateValidFrom },
                { "@ExchangeRateValidTo", exchangeRate.ExchangeRateValidTo },
                
            };
            
            ExecuteCommand($@"INSERT INTO C3_ExchangeRate (ExchangeRateSourceCurrency, ExchangeRateAUDValue, ExchangeRateNZDValue,ExchangeRateValidFrom,
                             ExchangeRateValidTo, ExchangeRateGuid,ExchangeRateLastModified) VALUES   
                     (@ExchangeRateSourceCurrency, @ExchangeRateAUDValue, @ExchangeRateNZDValue, @ExchangeRateValidFrom, @ExchangeRateValidTo, 
                      NEWID(), GETDATE())", parameters);
        }

        public void Update(ExchangeRateInfo exchangeRate)
        {
            
            var parameters = new Dictionary<string, object>
            {
                { "@Id", exchangeRate.ExchangeRateID },
                { "@ExchangeRateSourceCurrency", exchangeRate.ExchangeRateSourceCurrency },
                { "@ExchangeRateAUDValue", exchangeRate.ExchangeRateAUDValue },
                { "@ExchangeRateNZDValue", exchangeRate.ExchangeRateNZDValue },
                { "@ExchangeRateValidFrom", exchangeRate.ExchangeRateValidFrom },
                { "@ExchangeRateValidTo", exchangeRate.ExchangeRateValidTo },
                
            };
            
            ExecuteCommand($@"UPDATE C3_ExchangeRate SET ExchangeRateSourceCurrency=@ExchangeRateSourceCurrency, 
                           ExchangeRateAUDValue=@ExchangeRateAUDValue, ExchangeRateNZDValue=@ExchangeRateNZDValue,
                                ExchangeRateValidFrom=@ExchangeRateValidFrom, ExchangeRateValidTo= @ExchangeRateValidTo, 
                                ExchangeRateLastModified=GETDATE()
                                    WHERE ExchangeRateID=@Id", parameters);
        }
    }
}