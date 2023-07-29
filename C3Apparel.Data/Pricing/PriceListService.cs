using System.Collections.Generic;
using C3Apparel.Data.Common;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Products;

namespace C3Apparel.Data.Pricing
{
    public class PriceListService : IPriceListService
    {
        private readonly IPriceListPriceInfoProvider _priceListPriceInfoProvider;
        private readonly IBrandRepository _brandRepository;
        private readonly IProductPricingService _productPricingService;
        
        public PriceListService(IPriceListPriceInfoProvider priceListPriceInfoProvider, IBrandRepository brandRepository, IProductPricingService productPricingService)
        {
            _priceListPriceInfoProvider = priceListPriceInfoProvider;
            _brandRepository = brandRepository;
            _productPricingService = productPricingService;
        }
        
        public string SavePriceListToPriceListTable(int versionId, string currency, int brandId)
        {
            
            var brandPricing = _brandRepository.GetBrandPricingInfo(brandId, currency);
            if (!brandPricing.IsValid)
            {
                return "Brand not found";
            }
            
            IEnumerable<ProductItem> products;
            ResultItem result;
            
            (products, result) = _productPricingService.GetProductsWithConvertedPrice(brandPricing, currency);

            if (result.HasError)
            {
                return result.Message;
            }

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

            return string.Empty;
        }
    }
}