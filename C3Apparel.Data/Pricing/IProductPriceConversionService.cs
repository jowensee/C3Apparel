using System.Collections.Generic;
using C3Apparel.Data.Common;
using C3Apparel.Data.Products;

namespace C3Apparel.Data.Pricing
{
    public interface IProductPriceConversionService
    {
        (IEnumerable<ProductItem>, ResultItem) GetProductsWithConvertedPrice(BrandPricing brandPricing, string targetCurrency);
        (IEnumerable<ProductItem>, ResultItem) GetProductsWithConvertedPrice(BrandPricing brandPricing, string targetCurrency, int pageNumber, int itemPerPage);
        (IEnumerable<ProductItem>, ResultItem) GetProductsWithConvertedPrice(List<PriceWeightbasedSettings> freightSettings, BrandPricing brandPricing, SearchFilter filter, string targetCurrency, int pageNumber, int itemPerPage);
        (IEnumerable<ProductItem>, ResultItem) GetProductsWithConvertedPrice(List<PriceWeightbasedSettings> freightSettings, BrandPricing brandPricing, SearchFilter filter, string targetCurrency);

    }
}