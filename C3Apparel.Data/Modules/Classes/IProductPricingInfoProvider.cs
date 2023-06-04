
using System.Collections.Generic;
using System.Data;
using C3Apparel.Data.Pricing;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IProductPricingInfoProvider 
    {
        IEnumerable<DataRow> GetProductPricing(int brandId, SearchFilter filter, int pageNumber = 0, int itemsPerPage = 0);
        int GetProductPricingCount(int brandId, SearchFilter filter);

        ProductPricingInfo GetProductPricingByC3Style(int brandId, string c3Style);
    }
}