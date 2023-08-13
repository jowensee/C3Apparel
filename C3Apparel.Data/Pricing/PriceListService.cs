using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Common;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Products;

namespace C3Apparel.Data.Pricing
{
    public class PriceListService : IPriceListService
    {
        private readonly IPriceListPriceInfoProvider _priceListPriceInfoProvider;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductPriceConversionService _productPriceConversionService;
        
        public PriceListService(IPriceListPriceInfoProvider priceListPriceInfoProvider, IBrandRepository brandRepository, IProductPriceConversionService productPriceConversionService)
        {
            _priceListPriceInfoProvider = priceListPriceInfoProvider;
            _brandRepository = brandRepository;
            _productPriceConversionService = productPriceConversionService;
        }
        
        public (string, int) SavePriceListToPriceListTable(int versionId, string currency, int brandId)
        {
            
            var brandPricing = _brandRepository.GetBrandPricingInfo(brandId, currency);
            if (!brandPricing.IsValid)
            {
                return ("Brand not found", 0);
            }
            
            IEnumerable<ProductItem> products;
            ResultItem result;
            
            (products, result) = _productPriceConversionService.GetProductsWithConvertedPrice(brandPricing, currency);

            if (result.HasError)
            {
                return (result.Message, 0);
            }

            _priceListPriceInfoProvider.DeleteAll(1,brandId, currency);

            foreach (var product in products)
            {
                _priceListPriceInfoProvider.Insert(new PriceListPriceInfo
                {
                    PriceVersionID = versionId,
                    PriceBrandID = product.BrandID,
                    PriceBrandName = product.BrandName,
                    PriceC3Style = product.ProductCode,
                    PriceDescription = product.ProductName,
                    PriceCollection = product.Collection,
                    PriceColours = product.ProductColours,
                    PriceCurrency = currency,
                    PriceSizes = product.ProductSizes,
                    PriceColourDesc = product.ProductColourDesc,
                    PriceCoo = product.ProductCoo,
                    PriceGroup = product.ProductGroup,
                    PriceSupplierStyle = product.ProductSupplierStyle,
                    PriceCol1FreightSurcharge = product.FreightSurcharge1,
                    PriceCol2FreightSurcharge = product.FreightSurcharge2,
                    PriceCol3FreightSurcharge = product.FreightSurcharge3,
                    PriceCol4FreightSurcharge = product.FreightSurcharge4,
                    PriceCol1MOQUnit = product.MinimumOrderQty1,
                    PriceCol2MOQUnit = product.MinimumOrderQty2,
                    PriceCol3MOQUnit = product.MinimumOrderQty3,
                    PriceCol4MOQUnit = product.MinimumOrderQty4,
                    PriceCol1UnitPrice = product.Price1,
                    PriceCol2UnitPrice = product.Price2,
                    PriceCol3UnitPrice = product.Price3,
                    PriceCol4UnitPrice = product.Price4
                });
            }

            return (string.Empty, products.Count());
        }
    }
}