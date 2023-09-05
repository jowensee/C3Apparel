
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Modules.Filters;
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
                nameof(BrandInfo.BrandID),
                nameof(BrandInfo.BrandDisplayName),
                nameof(ProductPricingInfo.ProductPricingStatus),
                nameof(ProductPricingInfo.ProductPricingC3Style),
                nameof(ProductPricingInfo.ProductPricingCollection),
                nameof(ProductPricingInfo.ProductPricingDescription),
                nameof(ProductPricingInfo.ProductPricingGroup),
                nameof(ProductPricingInfo.ProductPricingColours),
                nameof(ProductPricingInfo.ProductPricingColourDesc),                
                nameof(ProductPricingInfo.ProductPricingCoo),
                nameof(ProductPricingInfo.ProductPricingSupplierStyle),
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
                    nameof(ProductPricingInfo.ProductPricingDescription),
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
                ProductPricingID = row[nameof(ProductPricingInfo.ProductPricingID)].ToInt(),
                ProductPricingDescription = row[nameof(ProductPricingInfo.ProductPricingDescription)].ToSafeString(),
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

        public void InsertProductPricing(ProductPricingInfo productPricing)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ProductPricingDescription", productPricing.ProductPricingDescription },
                { "@ProductPricingC3Style", productPricing.ProductPricingC3Style },
                { "@ProductPricingSupplierStyle", productPricing.ProductPricingSupplierStyle },
                { "@ProductPricingSupplierID", productPricing.ProductPricingSupplierID },
                { "@ProductPricingCollection", productPricing.ProductPricingCollection },
                { "@ProductPricingColours", productPricing.ProductPricingColours },
                { "@ProductPricingSizes", productPricing.ProductPricingSizes },
                { "@ProductPricingColourDesc", productPricing.ProductPricingColourDesc },
                { "@ProductPricingGroup", productPricing.ProductPricingGroup },
                { "@ProductPricingCoo", productPricing.ProductPricingCoo },
                { "@ProductPricingC3BuyPrice", productPricing.ProductPricingC3BuyPrice },
                { "@ProductPricingSKUWeight", productPricing.ProductPricingSKUWeight },
                { "@ProductPricingC3OverrideWeight", productPricing.ProductPricingC3OverrideWeight },
                { "@ProductPricingStatus", productPricing.ProductPricingStatus }
                
            };
            
            ExecuteCommand($@"INSERT INTO C3_ProductPricing (ProductPricingDescription, ProductPricingC3Style, ProductPricingSupplierStyle,ProductPricingSupplierID,
                             ProductPricingCollection, ProductPricingSizes, ProductPricingColours,ProductPricingColourDesc, ProductPricingGroup, ProductPricingCoo, 
                       ProductPricingC3BuyPrice, ProductPricingSKUWeight, ProductPricingC3OverrideWeight,ProductPricingStatus,
                       ProductPricingLastModified, ProductPricingGuid) VALUES   
                     (@ProductPricingDescription, @ProductPricingC3Style, @ProductPricingSupplierStyle, @ProductPricingSupplierID, 
                      @ProductPricingCollection, @ProductPricingSizes, @ProductPricingColours, @ProductPricingColourDesc, @ProductPricingGroup, @ProductPricingCoo,
                      @ProductPricingC3BuyPrice,@ProductPricingSKUWeight,@ProductPricingC3OverrideWeight,@ProductPricingStatus,
                      GETDATE(), NewID())", parameters);
        }

        public void UpdateProductPricing(ProductPricingInfo productPricing)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@ProductPricingID", productPricing.ProductPricingID },
                { "@ProductPricingDescription", productPricing.ProductPricingDescription },
                { "@ProductPricingC3Style", productPricing.ProductPricingC3Style },
                { "@ProductPricingSupplierStyle", productPricing.ProductPricingSupplierStyle },
                { "@ProductPricingSupplierID", productPricing.ProductPricingSupplierID },
                { "@ProductPricingCollection", productPricing.ProductPricingCollection },
                { "@ProductPricingSizes", productPricing.ProductPricingSizes },
                { "@ProductPricingColours", productPricing.ProductPricingColours },
                { "@ProductPricingColourDesc", productPricing.ProductPricingColourDesc },
                { "@ProductPricingGroup", productPricing.ProductPricingGroup },
                { "@ProductPricingCoo", productPricing.ProductPricingCoo },
                { "@ProductPricingC3BuyPrice", productPricing.ProductPricingC3BuyPrice },
                { "@ProductPricingSKUWeight", productPricing.ProductPricingSKUWeight },
                { "@ProductPricingC3OverrideWeight", productPricing.ProductPricingC3OverrideWeight },
                { "@ProductPricingStatus", productPricing.ProductPricingStatus }
                
            };
            
            ExecuteCommand($@"UPDATE C3_ProductPricing SET
                                    ProductPricingDescription = @ProductPricingDescription, 
                                    ProductPricingC3Style = @ProductPricingC3Style, 
                                    ProductPricingSupplierStyle = @ProductPricingSupplierStyle,
                                    ProductPricingSupplierID = @ProductPricingSupplierID,                             
                                    ProductPricingCollection = @ProductPricingCollection, 
                                    ProductPricingSizes = @ProductPricingSizes,
                                    ProductPricingColours = @ProductPricingColours,
                                    ProductPricingColourDesc = @ProductPricingColourDesc, 
                                    ProductPricingGroup = @ProductPricingGroup, 
                                    ProductPricingCoo = @ProductPricingCoo,                        
                                    ProductPricingC3BuyPrice = @ProductPricingC3BuyPrice, 
                                    ProductPricingSKUWeight = @ProductPricingSKUWeight, 
                                    ProductPricingC3OverrideWeight = @ProductPricingC3OverrideWeight,
                                    ProductPricingStatus = @ProductPricingStatus,
                                    ProductPricingLastModified = GETDATE()
                                    WHERE ProductPricingID = @ProductPricingID", parameters);
        }

        public void Delete(int id)
        {
            var parameters = new Dictionary<string, object> { { "@Id", id } };
            ExecuteCommand("DELETE FROM C3_ProductPricing WHERE ProductPricingID = @Id", parameters);
        }

        public void Delete(int brandId, string pricingC3Style)
        {
            var parameters = new Dictionary<string, object> { { "@Id", brandId },
            {
                "@style", pricingC3Style
            } };
            ExecuteCommand($@"DELETE FROM C3_ProductPricing WHERE {nameof(ProductPricingInfo.ProductPricingSupplierID)}=@Id
                                                                AND {nameof(ProductPricingInfo.ProductPricingC3Style)}=@style", parameters);
        }

        private string GetFilter(ProductPricingFilter filter)
        {
            var sFilterSql = new StringBuilder();
            if (filter != null )
            {
                if (!string.IsNullOrWhiteSpace(filter.C3Style))
                {
                    sFilterSql.Append(
                        $" AND {nameof(ProductPricingInfo.ProductPricingC3Style)} LIKE '%{SQLHelper.SqlString(filter.C3Style)}%'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.Collection))
                {
                    sFilterSql.Append(
                        $" AND {nameof(ProductPricingInfo.ProductPricingCollection)} LIKE '%{SQLHelper.SqlString(filter.Collection)}%'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.Colour))
                {
                    sFilterSql.Append(
                        $" AND {nameof(ProductPricingInfo.ProductPricingColours)} LIKE '%{SQLHelper.SqlString(filter.Colour)}%'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.Description))
                {
                    sFilterSql.Append(
                        $" AND {nameof(ProductPricingInfo.ProductPricingDescription)} LIKE '%{SQLHelper.SqlString(filter.Description)}%'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.Sizes))
                {
                    sFilterSql.Append(
                        $" AND {nameof(ProductPricingInfo.ProductPricingSizes)} LIKE '%{SQLHelper.SqlString(filter.Sizes)}%'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.ProductGroup))
                {
                    sFilterSql.Append(
                        $" AND {nameof(ProductPricingInfo.ProductPricingGroup)} LIKE '%{SQLHelper.SqlString(filter.ProductGroup)}%'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.SupplierStyle))
                {
                    sFilterSql.Append(
                        $" AND {nameof(ProductPricingInfo.ProductPricingSupplierStyle)} LIKE '%{SQLHelper.SqlString(filter.SupplierStyle)}%'");
                }
                
                if (!string.IsNullOrWhiteSpace(filter.COO))
                {
                    sFilterSql.Append(
                        $" AND {nameof(ProductPricingInfo.ProductPricingCoo)} LIKE '%{SQLHelper.SqlString(filter.COO)}%'");
                }
                
                if (filter.Supplier > 0)
                {
                    sFilterSql.Append(
                        $" AND {nameof(ProductPricingInfo.ProductPricingSupplierID)} = {filter.Supplier}");
                }
                
            }

            return sFilterSql.ToString();
        }
        public IEnumerable<ProductPricingInfo> GetAllProductPricings(ProductPricingFilter filter, int pageNumber, int itemsPerPage)
        {
            var sFilterSql = GetFilter(filter);
            
            var sSql = $@"SELECT * FROM C3_ProductPricing
                       WHERE 1 = 1 {sFilterSql}
                        ORDER BY ProductPricingCollection, REPLICATE('0',20-LEN(ProductPricingC3Style)) + ProductPricingC3Style";

            if (itemsPerPage > 0)
            {
                sSql += $" OFFSET {(pageNumber - 1) * itemsPerPage} ROWS FETCH NEXT {itemsPerPage} ROWS ONLY";
            }
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return Enumerable.Empty<ProductPricingInfo>();
            }

            
            var results = new List<ProductPricingInfo>();

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                results.Add(CreateProductPricingInfo(ds.Tables[0].Rows[i]));
            }

            return results;

        }

        private ProductPricingInfo CreateProductPricingInfo(DataRow row)
        {
            if (row == null)
            {
                return null;
            }
            return new ProductPricingInfo
            {
                ProductPricingID =  row[nameof(ProductPricingInfo.ProductPricingID)].ToInt(),
                ProductPricingDescription = row[nameof(ProductPricingInfo.ProductPricingDescription)].ToSafeString(),
                ProductPricingC3Style = row[nameof(ProductPricingInfo.ProductPricingC3Style)].ToSafeString(),
                ProductPricingSupplierStyle = row[nameof(ProductPricingInfo.ProductPricingSupplierStyle)].ToSafeString(),
                ProductPricingSupplierID = row[nameof(ProductPricingInfo.ProductPricingSupplierID)].ToInt(),
                ProductPricingCollection = row[nameof(ProductPricingInfo.ProductPricingCollection)].ToSafeString(),
                ProductPricingColours = row[nameof(ProductPricingInfo.ProductPricingColours)].ToSafeString(),
                ProductPricingColourDesc = row[nameof(ProductPricingInfo.ProductPricingColourDesc)].ToSafeString(),
                ProductPricingGroup = row[nameof(ProductPricingInfo.ProductPricingGroup)].ToSafeString(),
                ProductPricingCoo = row[nameof(ProductPricingInfo.ProductPricingCoo)].ToSafeString(),
                ProductPricingC3BuyPrice =  row[nameof(ProductPricingInfo.ProductPricingC3BuyPrice)].ToDecimal(),
                ProductPricingSKUWeight = row[nameof(ProductPricingInfo.ProductPricingSKUWeight)].ToDecimal(),
                ProductPricingC3OverrideWeight =  row[nameof(ProductPricingInfo.ProductPricingC3OverrideWeight)].ToDecimal(),
                ProductPricingStatus = row[nameof(ProductPricingInfo.ProductPricingStatus)].ToSafeString(),
                ProductPricingSizes = row[nameof(ProductPricingInfo.ProductPricingSizes)].ToSafeString(),
                
            };
        }

        public int GetAllProductPricingsCount(ProductPricingFilter filter)
        {
            var sFilterSql = GetFilter(filter);
            
            var sSql = $@"SELECT COUNT(*) FROM C3_ProductPricing
                       WHERE 1 = 1 {sFilterSql}";

           
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return 0;
            }

            return ds.Tables[0].Rows[0][0].ToInt();
        }

        public ProductPricingInfo GetProductPricing(int productPricingId)
        {
            var sSql = $"SELECT * FROM C3_ProductPricing WHERE ProductPricingID={productPricingId}";

         
            var ds = ExecuteQuery(sSql);

            if (DataHelper.IsEmpty(ds))
            {
                return null;
            }

            return CreateProductPricingInfo(ds.Tables[0].Rows[0]);
        }

        public void DeleteAll(int brandId)
        {
            var parameters = new Dictionary<string, object> { { "@Id", brandId } };
            ExecuteCommand($"DELETE FROM C3_ProductPricing WHERE {nameof(ProductPricingInfo.ProductPricingSupplierID)} = @Id", parameters);
        }
    }
}