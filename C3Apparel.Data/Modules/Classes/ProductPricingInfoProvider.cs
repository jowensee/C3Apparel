
using System.Collections.Generic;
using System.Data;
using System.Linq;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Pricing;
using C3Apparel.Data.Sql;
using C3Apparel.Data.Utilities;

namespace C3Apparel.Data.Modules.Classes
{
    public class ProductPricingInfoProvider : BaseRepository, IProductPricingInfoProvider
    {
        public ProductPricingInfoProvider(IConfigurationService configurationService) : base(configurationService)
        {
        }
        public IEnumerable<DataRow> GetProductPricing(int brandId, SearchFilter filter, int pageNumber = 0, int itemsPerPage = 0)
        {
            string[] columns =
            {
                nameof(BrandInfo.BrandDisplayName),
                nameof(ProductPricingInfo.ProductPricingStatus),
                nameof(ProductPricingInfo.ProductPricingC3Style),
                nameof(ProductPricingInfo.ProducPricingDescription),
                nameof(ProductPricingInfo.ProductPricingGroup),
                nameof(ProductPricingInfo.ProductPricingColours),
                nameof(ProductPricingInfo.ProductPricingSizes),
                nameof(ProductPricingInfo.ProductPricingC3BuyPrice),
                nameof(ProductPricingInfo.ProductPricingSKUWeight),
                nameof(ProductPricingInfo.ProductPricingC3OverrideWeight)
            };

            var sFilterSql = string.Empty;
            if (filter != null && !string.IsNullOrWhiteSpace(filter.Collection))
            {
                sFilterSql = $" AND ProductPricingCollection LIKE '%{SQLHelper.SqlString(filter.Collection)}%'";
            }
            
            var sSql = $@"SELECT {string.Join(",", columns)} FROM C3_ProductPricing JOIN COM_Brand
                        ON C3_ProductPricing.ProductPricingSupplierID = COM_Brand.BrandID
                       WHERE COM_Brand.BrandID = {brandId} {sFilterSql}
                        ORDER BY ProductPricingID";

            if (itemsPerPage > 0)
            {
                sSql += $" OFFSET {(pageNumber - 1) * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY";
            }
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return Enumerable.Empty<DataRow>();
            }

            
            var results = new List<DataRow>();

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                results.Add(ds.Tables[0].Rows[i]);
            }

            return results;
            /*
             *
             *var query = _productPricingInfoProvider.GetProductPricing(whereCondition, pageNumber, itemsPerPage).Columns(
                    nameof(BrandInfo.BrandDisplayName),
                    nameof(ProductPricingInfo.ProductPricingStatus),
                    nameof(ProductPricingInfo.ProductPricingC3Style),
                    nameof(ProductPricingInfo.ProducPricingDescription),
                    nameof(ProductPricingInfo.ProductPricingGroup),
                    nameof(ProductPricingInfo.ProductPricingColours),
                    nameof(ProductPricingInfo.ProductPricingSizes),
                    nameof(ProductPricingInfo.ProductPricingC3BuyPrice),
                    nameof(ProductPricingInfo.ProductPricingSKUWeight),
                    nameof(ProductPricingInfo.ProductPricingC3OverrideWeight)).Source(
                    product => product.Join<BrandInfo>(nameof(ProductPricingInfo.ProductPricingSupplierID), nameof(BrandInfo.BrandID)))
                .WhereEquals(nameof(ProductPricingInfo.ProductPricingSupplierID), brandID);

            if (!string.IsNullOrWhiteSpace(filter?.Collection))
            {
                query = query.WhereLike(nameof(ProductPricingInfo.ProductPricingCollection), $"%{filter.Collection}%");
            }
            
            
            return query.OrderBy(nameof(ProductPricingInfo.ProductPricingID)).Page(pageNumber - 1, itemsPerPage).Select(product => CreateProductItem(product));
    
             * 
             */
        }

        public int GetProductPricingCount(int brandId, SearchFilter filter)
        {
            var sSql = $@"SELECT COUNT(*) FROM C3_ProductPricing WHERE ProductPricingSupplierID = {brandId}";

            if (filter != null && !string.IsNullOrWhiteSpace(filter.Collection))
            {
                sSql += $" AND ProductPricingCollection LIKE ' '%{SQLHelper.SqlString(filter.Collection)}%'";
            }

            var ds = ExecuteQuery(sSql);
            if (DataHelper.IsEmpty(ds))
            {
                return 0;
            }

            return ds.Tables[0].Rows[0][0].ToInt();
            /*
             * var query = _productPricingInfoProvider.GetProductPricingCount(brandID, filter?.Collectio)
                .WhereEquals(nameof(ProductPricingInfo.ProductPricingSupplierID), brandID);
            
            if (!string.IsNullOrWhiteSpace(filter?.Collection))
            {
                query = query.WhereLike(nameof(ProductPricingInfo.ProductPricingCollection), $"%{filter.Collection}%");
            }

            return query.Count();
             *
             * 
             */
        }

        public ProductPricingInfo GetProductPricingByC3Style(int brandId, string c3Style)
        {
            var sSql =
                $@"SELECT TOP 1 * FROM C3_ProductPricing WHERE ProductPricingSupplierID = {brandId} AND ProductPricingC3Style = '{c3Style.Replace("'", "''")}' ";

            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return null;
            }

            var row = ds.Tables[0].Rows[0];
            return new ProductPricingInfo
            {
                ProducPricingDescription = row[nameof(ProductPricingInfo.ProducPricingDescription)].ToSafeString(),
                ProductPricingCollection = row[nameof(ProductPricingInfo.ProductPricingCollection)].ToSafeString(),
                ProductPricingColours = row[nameof(ProductPricingInfo.ProductPricingColours)].ToSafeString(),
                ProductPricingCoo = row[nameof(ProductPricingInfo.ProductPricingCoo)].ToSafeString(),
                ProductPricingGroup = row[nameof(ProductPricingInfo.ProductPricingGroup)].ToSafeString(),
                ProductPricingSizes = row[nameof(ProductPricingInfo.ProductPricingSizes)].ToSafeString(),
                ProductPricingStatus = row[nameof(ProductPricingInfo.ProductPricingStatus)].ToSafeString(),
                ProductPricingC3Style = row[nameof(ProductPricingInfo.ProductPricingC3Style)].ToSafeString(),
                ProductPricingColourDesc = row[nameof(ProductPricingInfo.ProductPricingColourDesc)].ToSafeString(),
                ProductPricingSupplierStyle =
                    row[nameof(ProductPricingInfo.ProductPricingSupplierStyle)].ToSafeString(),
                ProductPricingC3BuyPrice = row[nameof(ProductPricingInfo.ProductPricingC3BuyPrice)].ToDecimal(),
                ProductPricingC3OverrideWeight =
                    row[nameof(ProductPricingInfo.ProductPricingC3OverrideWeight)].ToDecimal(),
                ProductPricingSKUWeight = row[nameof(ProductPricingInfo.ProductPricingSKUWeight)].ToDecimal(),
                ProductPricingSupplierID = row[nameof(ProductPricingInfo.ProductPricingSupplierID)].ToInt()
            };

        }

    }
}