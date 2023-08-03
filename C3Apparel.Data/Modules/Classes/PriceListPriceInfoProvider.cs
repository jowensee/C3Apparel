
using System.Collections.Generic;
using System.Data;
using System.Linq;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Sql;

namespace C3Apparel.Data.Modules.Classes
{
    public class PriceListPriceInfoProvider : BaseRepository, IPriceListPriceInfoProvider
    {
        public PriceListPriceInfoProvider(IConfigurationService configurationService) : base(configurationService)
        {
        }

        public void Insert(PriceListPriceInfo price)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@versionId", price.PriceVersionID },
                { "@currency", price.PriceCurrency },
                { "@brandId", price.PriceBrandID },
                { "@brandName", price.PriceBrandName },
                { "@collection", price.PriceCollection },
                { "@c3Style", price.PriceC3Style },
                { "@description", price.PriceDescription },
                { "@sizes", price.PriceSizes },
                { "@colours", price.PriceColours },
                { "@colourDesc", price.PriceColourDesc },
                { "@coo", price.PriceCoo },
                { "@group", price.PriceGroup },
                { "@supplierStyle", price.PriceSupplierStyle },
                { "@col1FreightSurcharge", price.PriceCol1FreightSurcharge },
                { "@col1UnitPrice", price.PriceCol1UnitPrice },
                { "@col1MOQUnit", price.PriceCol1MOQUnit },
                { "@col2FreightSurcharge", price.PriceCol2FreightSurcharge },
                { "@col2UnitPrice", price.PriceCol2UnitPrice },
                { "@col2MOQUnit", price.PriceCol2MOQUnit },
                { "@col3FreightSurcharge", price.PriceCol3FreightSurcharge },
                { "@col3UnitPrice", price.PriceCol3UnitPrice },
                { "@col3MOQUnit", price.PriceCol3MOQUnit },
                { "@col4FreightSurcharge", price.PriceCol4FreightSurcharge },
                { "@col4UnitPrice", price.PriceCol4UnitPrice },
                { "@col4MOQUnit", price.PriceCol4MOQUnit }
                
            };
            
            ExecuteCommand($@"INSERT INTO [dbo].[C3_PricingListPrices]
           ([PriceVersionID]
           ,[PriceCurrency]
           ,[PriceBrandID]
           ,[PriceBrandName]
           ,[PriceCollection]
           ,[PriceC3Style]
           ,[PriceDescription]
           ,[PriceSizes]
           ,[PriceColours]
           ,[PriceColourDesc]
           ,[PriceCoo]
           ,[PriceGroup]
           ,[PriceSupplierStyle]
           ,[PriceCol1FreightSurcharge]
           ,[PriceCol1UnitPrice]
           ,[PriceCol1MOQUnit]
           ,[PriceCol2FreightSurcharge]
           ,[PriceCol2UnitPrice]
           ,[PriceCol2MOQUnit]
           ,[PriceCol3FreightSurcharge]
           ,[PriceCol3UnitPrice]
           ,[PriceCol3MOQUnit]
           ,[PriceCol4FreightSurcharge]
           ,[PriceCol4UnitPrice]
           ,[PriceCol4MOQUnit])
     VALUES
           (@versionId
           ,@currency
           ,@brandId
           ,@brandName
           ,@collection
           ,@c3Style
           ,@description
           ,@sizes
           ,@colours
           ,@colourDesc
           ,@coo
           ,@group
           ,@supplierStyle
           ,@col1FreightSurcharge
           ,@col1UnitPrice
           ,@col1MOQUnit
           ,@col2FreightSurcharge
           ,@col2UnitPrice
           ,@col2MOQUnit
           ,@col3FreightSurcharge
           ,@col3UnitPrice
           ,@col3MOQUnit
           ,@col4FreightSurcharge
           ,@col4UnitPrice
           ,@col4MOQUnit)", parameters);
        }

        public IEnumerable<PriceListPriceInfo> GetAllPrices(int versionId, string currency, int brandId, int pageNumber = 0, int itemsPerPage = 0)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@versionId", versionId},
                { "@currency", currency },
                { "@brandId", brandId },
            };
            var sSql = $@"SELECT * FROM C3_PricingListPrices
                       WHERE PriceVersionId = @versionId AND PriceCurrency = @currency AND PriceBrandId = @brandId
                        ORDER BY PriceCollection, PriceC3Style";

            if (itemsPerPage > 0)
            {
                sSql += $" OFFSET {(pageNumber - 1) * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY";
            }
            var ds = ExecuteQuery(sSql, parameters);

            if (DataHelper.IsEmpty(ds))
            {
                return Enumerable.Empty<PriceListPriceInfo>();
            }

            
            var results = new List<PriceListPriceInfo>();

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                results.Add(CreatePriceListPriceInfo(ds.Tables[0].Rows[i]));
            }

            return results;
        }

        private PriceListPriceInfo CreatePriceListPriceInfo(DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            return new PriceListPriceInfo
            {
                PriceVersionID = row[nameof(PriceListPriceInfo.PriceVersionID)].ToInt(),
                PriceCurrency = row[nameof(PriceListPriceInfo.PriceCurrency)].ToSafeString(),
                PriceBrandID = row[nameof(PriceListPriceInfo.PriceBrandID)].ToInt(),
                PriceBrandName = row[nameof(PriceListPriceInfo.PriceBrandName)].ToSafeString(),
                PriceCollection = row[nameof(PriceListPriceInfo.PriceCollection)].ToSafeString(),
                PriceC3Style = row[nameof(PriceListPriceInfo.PriceC3Style)].ToSafeString(),
                PriceSupplierStyle = row[nameof(PriceListPriceInfo.PriceSupplierStyle)].ToSafeString(),
                PriceDescription = row[nameof(PriceListPriceInfo.PriceDescription)].ToSafeString(),
                PriceCoo = row[nameof(PriceListPriceInfo.PriceCoo)].ToSafeString(),
                PriceGroup =  row[nameof(PriceListPriceInfo.PriceGroup)].ToSafeString(),
                PriceSizes = row[nameof(PriceListPriceInfo.PriceSizes)].ToSafeString(),
                PriceColours = row[nameof(PriceListPriceInfo.PriceColours)].ToSafeString(),
                PriceColourDesc = row[nameof(PriceListPriceInfo.PriceColourDesc)].ToSafeString(),
                PriceCol1FreightSurcharge = row[nameof(PriceListPriceInfo.PriceCol1FreightSurcharge)].ToDecimal(),
                PriceCol1UnitPrice = row[nameof(PriceListPriceInfo.PriceCol1UnitPrice)].ToDecimal(),
                PriceCol1MOQUnit = row[nameof(PriceListPriceInfo.PriceCol1MOQUnit)].ToInt(),
                PriceCol2FreightSurcharge = row[nameof(PriceListPriceInfo.PriceCol2FreightSurcharge)].ToDecimal(),
                PriceCol2UnitPrice = row[nameof(PriceListPriceInfo.PriceCol2UnitPrice)].ToDecimal(),
                PriceCol2MOQUnit = row[nameof(PriceListPriceInfo.PriceCol2MOQUnit)].ToInt(),
                PriceCol3FreightSurcharge = row[nameof(PriceListPriceInfo.PriceCol3FreightSurcharge)].ToDecimal(),
                PriceCol3UnitPrice = row[nameof(PriceListPriceInfo.PriceCol3UnitPrice)].ToDecimal(),
                PriceCol3MOQUnit = row[nameof(PriceListPriceInfo.PriceCol3MOQUnit)].ToInt(),
                PriceCol4FreightSurcharge = row[nameof(PriceListPriceInfo.PriceCol4FreightSurcharge)].ToDecimal(),
                PriceCol4UnitPrice = row[nameof(PriceListPriceInfo.PriceCol4UnitPrice)].ToDecimal(),
                PriceCol4MOQUnit = row[nameof(PriceListPriceInfo.PriceCol4MOQUnit)].ToInt(),
            };
        }

        public int GetAllPricesCount(int versionId, string currency, int brandId)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@versionId", versionId },
                { "@currency", currency },
                { "@brandId", brandId }
            };
            var sSql = $@"SELECT COUNT(*) FROM C3_PricingListPrices
                       WHERE PriceVersionId = @versionId AND PriceCurrency = @currency AND PriceBrandId = @brandId";

           
            var ds = ExecuteQuery(sSql, parameters);

            if (DataHelper.IsEmpty(ds))
            {
                return 0;
            }

            return ds.Tables[0].Rows[0][0].ToInt();
        }

        public void DeleteAll(int versionId, int brandId, string currency)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@versionId", versionId },
                { "@currency", currency },
                { "@brandId", brandId }
            };
            var sSql = $@"DELETE FROM C3_PricingListPrices
                       WHERE PriceVersionId = @versionId AND PriceCurrency = @currency AND PriceBrandId = @brandId";

            ExecuteCommand(sSql, parameters);

        }
    }
}