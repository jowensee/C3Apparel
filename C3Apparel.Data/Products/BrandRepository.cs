using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Modules.Classes;
using C3Apparel.Data.Pricing;

namespace C3Apparel.Data.Products
{
    public class BrandRepository:IBrandRepository
    {
        private readonly IBrandInfoProvider _brandInfoProvider;
        private readonly IExchangeRateRetriever _exchangeRateRetriever;
        private readonly IProductSettingsRepository _productSettingsRepository;
        public BrandRepository(IBrandInfoProvider brandInfoProvider, IExchangeRateRetriever exchangeRateRetriever, IProductSettingsRepository productSettingsRepository)
        {
            _brandInfoProvider = brandInfoProvider;
            _exchangeRateRetriever = exchangeRateRetriever;
            _productSettingsRepository = productSettingsRepository;
        }
        public IEnumerable<BrandItem> GetAllBrands()
        {
            return _brandInfoProvider.GetAllBrands()
                .Select(b => new BrandItem(b.BrandID, b.BrandDisplayName, 
                    b.BrandCurrency,
                    b.BrandPricingDisclaimerTextAU,
                    b.BrandPricingDisclaimerTextNZ));

        }
        
        public BrandItem GetBrand(int brandId)
        {
             var brand = _brandInfoProvider.GetBrand(brandId);

             if (brand == null)
             {
                 return null;
             }

             return new BrandItem(brand.BrandID, brand.BrandDisplayName,
                 brand.BrandCurrency,
                 brand.BrandPricingDisclaimerTextAU,
                 brand.BrandPricingDisclaimerTextNZ);

        }

        public BrandPricing GetBrandPricingInfo(int brandId, string targetCurrency)
        {
            var brand = GetBrand(brandId);
            
            //TODO:: exchangerate and import duty can be from inquiry input
            var importDuty = _productSettingsRepository.GetPriceGlobalSettings()?.GetImportDuty(targetCurrency) ?? 0;
            
            return new BrandPricing(brand, _exchangeRateRetriever.GetExchangeRate(brand.BrandCurrency, targetCurrency), importDuty);

        }
    }
}