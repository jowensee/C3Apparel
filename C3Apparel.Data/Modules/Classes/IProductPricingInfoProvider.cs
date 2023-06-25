
using System.Collections.Generic;
using System.Data;
using C3Apparel.Data.Modules.Filters;
using C3Apparel.Data.Pricing;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IProductPricingInfoProvider 
    {
        IEnumerable<DataRow> GetProductPricing(int brandId, SearchFilter filter, int pageNumber = 0, int itemsPerPage = 0);
        int GetProductPricingCount(int brandId, SearchFilter filter);

        ProductPricingInfo GetProductPricingByC3Style(int brandId, string c3Style);
        void InsertProductPricing(ProductPricingInfo productPricing);
        void UpdateProductPricing(ProductPricingInfo productPricing);
        void Delete(int id);
        void Delete(int brandId, string pricingC3Style);
        IEnumerable<ProductPricingInfo> GetAllProductPricings(ProductPricingFilter filter, int pageNumber, int itemsPerPage);
        int GetAllProductPricingsCount(ProductPricingFilter filter);
        ProductPricingInfo GetProductPricing(int productPricingId);
        void DeleteAll(int brandId);
    }
}