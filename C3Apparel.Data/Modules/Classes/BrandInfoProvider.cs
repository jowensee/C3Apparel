using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Data.Sql;
using C3Apparel.Data.Utilities;

namespace C3Apparel.Data.Modules.Classes
{
    public class BrandInfoProvider : BaseRepository, IBrandInfoProvider
    {
        public const string BASE_SQL = @"SELECT [BrandID]
                          ,[BrandDisplayName]
                          ,[BrandName]
                          ,[BrandDescription]
                          ,[BrandHomepage]
                          ,[BrandEnabled]
                          ,[BrandGuid]
                          ,[BrandLastModified]
                          ,[BrandCurrency]
                          ,[BrandPricingDisclaimerTextAU]
                          ,[BrandPricingDisclaimerTextNZ]
                          ,[BrandBusinessName]
                          ,[BrandFocus]
                          ,[BrandPriceListPublishedDate] 
                            FROM COM_BRAND";
        public BrandInfoProvider(IConfigurationService configurationService) : base(configurationService)
        {
        }
        public IEnumerable<BrandInfo> GetAllBrands(bool includeDisabled = false)
        {
            var sSql = BASE_SQL;

            if (!includeDisabled)
            {
                sSql += " WHERE BrandEnabled=1";
            }

            sSql += " ORDER BY BrandDisplayName";
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return Enumerable.Empty<BrandInfo>();
            }

            var brands = new List<BrandInfo>();
            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var row = ds.Tables[0].Rows[i];
                
                brands.Add(CreateBrandInfo(row));
            }
            
            return brands;
        }

        private BrandInfo CreateBrandInfo(DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            return new BrandInfo
            {
                BrandCurrency = row[nameof(BrandInfo.BrandCurrency)].ToSafeString(),
                BrandDisplayName = row[nameof(BrandInfo.BrandDisplayName)].ToSafeString(),
                BrandName = row[nameof(BrandInfo.BrandName)].ToSafeString(),
                BrandPricingDisclaimerTextAU = row[nameof(BrandInfo.BrandPricingDisclaimerTextAU)].ToSafeString(),
                BrandPricingDisclaimerTextNZ = row[nameof(BrandInfo.BrandPricingDisclaimerTextNZ)].ToSafeString(),
                BrandID = row[nameof(BrandInfo.BrandID)].ToInt(),
                BrandFocus = row[nameof(BrandInfo.BrandFocus)].ToSafeString(),
                BrandBusinessName = row[nameof(BrandInfo.BrandBusinessName)].ToSafeString(),
                BrandPriceListPublishedDate = row[nameof(BrandInfo.BrandPriceListPublishedDate)].ToDateTime(),
                BrandEnabled = row[nameof(BrandInfo.BrandEnabled)].ToBool(),
                BrandHomepage =  row[nameof(BrandInfo.BrandHomepage)].ToSafeString(),
                BrandDescription = row[nameof(BrandInfo.BrandDescription)].ToSafeString(),
            };
        }
        public BrandInfo GetBrand(int brandId)
        {
            var sSql = $"{BASE_SQL} WHERE BrandID={brandId}";

         
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return null;
            }

            return CreateBrandInfo(ds.Tables[0].Rows[0]);
        }

        public IEnumerable<BrandInfo> GetBrandsWithPricing()
        {
            var whereClause = $@"WHERE BrandID IN (
                SELECT DISTINCT ProductPricingSupplierID FROM C3_ProductPricing 
            )";
            var sSql = $@"{BASE_SQL} {whereClause} ORDER BY BrandDisplayName" ;

            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return Enumerable.Empty<BrandInfo>();
            }

            var brands = new List<BrandInfo>();
            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                var row = ds.Tables[0].Rows[i];
                
                brands.Add(CreateBrandInfo(row));
            }
            
            return brands;
        }

        public IEnumerable<BrandInfo> GetAllBrands(BrandFilter filter, int pageNumber = 0, int itemsPerPage = 0)
        {
            var sFilterSql = new StringBuilder();
            if (filter != null )
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                {
                    sFilterSql.Append(
                        $" AND {nameof(BrandInfo.BrandDisplayName)} LIKE '%{SQLHelper.SqlString(filter.Name)}%'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.Focus))
                {
                    sFilterSql.Append(
                        $" AND {nameof(BrandInfo.BrandFocus)} = '{SQLHelper.SqlString(filter.Focus)}'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.Currency))
                {
                    sFilterSql.Append(
                        $" AND {nameof(BrandInfo.BrandCurrency)} = '{SQLHelper.SqlString(filter.Currency)}'");
                }
            }
            
            var sSql = $@"SELECT * FROM COM_Brand
                       WHERE 1 = 1 {sFilterSql}
                        ORDER BY BrandDisplayName";

            if (itemsPerPage > 0)
            {
                sSql += $" OFFSET {(pageNumber - 1) * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY";
            }
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return Enumerable.Empty<BrandInfo>();
            }

            
            var results = new List<BrandInfo>();

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                results.Add(CreateBrandInfo(ds.Tables[0].Rows[i]));
            }

            return results;
           
        }
   
        public int GetAllBrandsCount(BrandFilter filter)
        {
            var sFilterSql = new StringBuilder();
            if (filter != null )
            {
                if (!string.IsNullOrWhiteSpace(filter.Name))
                {
                    sFilterSql.Append(
                        $" AND {nameof(BrandInfo.BrandDisplayName)} LIKE '%{SQLHelper.SqlString(filter.Name)}%'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.Focus))
                {
                    sFilterSql.Append(
                        $" AND {nameof(BrandInfo.BrandFocus)} = '{SQLHelper.SqlString(filter.Focus)}'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.Currency))
                {
                    sFilterSql.Append(
                        $" AND {nameof(BrandInfo.BrandCurrency)} = '{SQLHelper.SqlString(filter.Currency)}'");
                }
            }
            
            var sSql = $@"SELECT COUNT(*) FROM COM_Brand
                       WHERE 1 = 1 {sFilterSql}";

           
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return 0;
            }

            return ds.Tables[0].Rows[0][0].ToInt();

        }

        public void Delete(int id)
        {
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            ExecuteCommand("DELETE FROM COM_Brand WHERE BrandID = @Id", parameters);
        }

        public void InsertBrand(BrandInfo brand)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@displayName", brand.BrandDisplayName },
                { "@codeName", brand.BrandName },
                { "@currency", brand.BrandCurrency },
                { "@enabled", brand.BrandEnabled },
                { "@focus", brand.BrandFocus },
                { "@website", brand.BrandHomepage },
                { "@description", brand.BrandDescription },
                { "@buinessName", brand.BrandBusinessName },
                { "@disclaimerAU", brand.BrandPricingDisclaimerTextAU },
                { "@disclaimerNZ", brand.BrandPricingDisclaimerTextNZ }
                
            };
            
            ExecuteCommand($@"INSERT INTO COM_Brand (BrandDisplayName, BrandName, BrandDescription, BrandCurrency,BrandEnabled,
                             BrandFocus, BrandHomepage,BrandBusinessName,
                       BrandPricingDisclaimerTextAU, BrandPricingDisclaimerTextNZ) VALUES   
                     (@displayName, @codeName, @description, @currency, @enabled, @focus, @website, @buinessName, @disclaimerAU, @disclaimerNZ)", parameters);
        }

        public void UpdateBrand(BrandInfo brand)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@Id", brand.BrandID },
                { "@displayName", brand.BrandDisplayName },
                { "@codeName", brand.BrandName },
                { "@currency", brand.BrandCurrency },
                { "@enabled", brand.BrandEnabled },
                { "@focus", brand.BrandFocus },
                { "@website", brand.BrandHomepage },
                { "@description", brand.BrandDescription.DBNullIfNull() },
                { "@businessName", brand.BrandBusinessName.DBNullIfNull() },
                { "@disclaimerAU", brand.BrandPricingDisclaimerTextAU },
                { "@disclaimerNZ", brand.BrandPricingDisclaimerTextNZ },
                
            };

            var publishedDateUpdate = "BrandPriceListPublishedDate = NULL";
            if (brand.BrandPriceListPublishedDate > DateTime.MinValue)
            {
                publishedDateUpdate = "BrandPriceListPublishedDate = @publishedDate";
                parameters.Add("@publishedDate", brand.BrandPriceListPublishedDate);
            }
            
            ExecuteCommand($@"UPDATE COM_Brand SET BrandDisplayName=@displayName, BrandName = @codeName, BrandDescription=@description, BrandCurrency=@currency,
                                BrandEnabled=@enabled, BrandFocus= @focus, BrandHomepage=@website,BrandBusinessName=@businessName,
                                    BrandPricingDisclaimerTextAU=@disclaimerAU, BrandPricingDisclaimerTextNZ=@disclaimerNZ,
                                   {publishedDateUpdate}
                                    WHERE BrandID=@Id", parameters);
            
        }
    }
}