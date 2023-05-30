
using System.Collections.Generic;
using System.Data;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IProductPricingInfoProvider 
    {
        IEnumerable<DataRow> GetProductPricing(int brandId, string filterCollection, int pageNumber = 0, int itemsPerPage = 0);
        int GetProductPricingCount(int brandId, string filterCollection);

        ProductPricingInfo GetProductPricingByC3Style(int brandId, string c3Style);
    }
}