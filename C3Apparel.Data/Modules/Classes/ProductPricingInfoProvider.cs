
using System.Collections.Generic;
using System.Data;

namespace C3Apparel.Data.Modules.Classes
{
    public class ProductPricingInfoProvider : IProductPricingInfoProvider
    {
        public IEnumerable<DataRow> GetProductPricing(int brandId, string filterCollection, int pageNumber = 0, int itemsPerPage = 0)
        {
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
            throw new System.NotImplementedException();
        }

        public int GetProductPricingCount(int brandId, string filterCollection)
        {
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
            throw new System.NotImplementedException();
        }

        public ProductPricingInfo GetProductPricingByC3Style(int brandId, string c3Style)
        {
            /*
             * product = productProvider.GetProductPricing().WhereEquals(nameof(ProductPricingInfo.ProductPricingSupplierID), brandID)
                            .WhereEquals(nameof(ProductPricingInfo.ProductPricingC3Style), pricing.C3Style)
                            .FirstOrDefault();

             * 
             */
            throw new System.NotImplementedException();
        }
    }
}