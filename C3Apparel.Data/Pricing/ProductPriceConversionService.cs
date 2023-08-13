using System.Collections.Generic;
using System.Linq;
using C3Apparel.Data.Common;
using C3Apparel.Data.Products;
namespace C3Apparel.Data.Pricing
{
    public class ProductPriceConversionService : IProductPriceConversionService
    {
        private readonly IProductRepository _productRepository;
        private readonly IExchangeRateRetriever _exchangeRateRetriever;
        private readonly IBrandRepository _brandRepository;
        private readonly IPriceCalculator _priceCalculator;
        
        public ProductPriceConversionService(IProductRepository productRepository, IExchangeRateRetriever exchangeRateRetriever, IBrandRepository brandRepository, IProductSettingsRepository productSettingsRepository, IPriceCalculator priceCalculator)
        {
            _productRepository = productRepository;
            _exchangeRateRetriever = exchangeRateRetriever;
            _brandRepository = brandRepository;
            _priceCalculator = priceCalculator;
        }

        private (IEnumerable<ProductItem>, ResultItem) ConvertPrices(IEnumerable<ProductItem> products, BrandPricing brandPricing, string targetCurrency)
        {
            var resultItem = new ResultItem();
            
            if (brandPricing?.Brand == null)
            {
                resultItem.HasError = true;
                resultItem.Message = "Invalid BrandId";

                return (Enumerable.Empty<ProductItem>(), resultItem);

            }

            if (brandPricing?.ExchangeRate == null)
            {
                resultItem.HasError = true;
                resultItem.Message = "Cannot find exchange rate";

                return (Enumerable.Empty<ProductItem>(), resultItem);

            }

            var productList = products.ToList();
            
            productList.ForEach(p => ConvertItemPrices(brandPricing.Brand.BrandRegionCode,p, brandPricing.ExchangeRate.Rate, targetCurrency, brandPricing.ImportDuty));
            return (productList, resultItem);
        }
        
        private (IEnumerable<ProductItem>, ResultItem) ConvertPrices(List<PriceWeightbasedSettings> freightSettings, IEnumerable<ProductItem> products, BrandPricing brandPricing, string targetCurrency)
        {
            var resultItem = new ResultItem();
            
            if (brandPricing?.Brand == null)
            {
                resultItem.HasError = true;
                resultItem.Message = "Invalid BrandId";

                return (Enumerable.Empty<ProductItem>(), resultItem);

            }

            if (brandPricing?.ExchangeRate == null)
            {
                resultItem.HasError = true;
                resultItem.Message = "Cannot find exchange rate";

                return (Enumerable.Empty<ProductItem>(), resultItem);
            }

            var productList = products.ToList();
            
            productList.ForEach(p => ConvertItemPrices(freightSettings[0], freightSettings[1], freightSettings[2], freightSettings[3], 
                p, brandPricing.ExchangeRate.Rate, targetCurrency, brandPricing.ImportDuty));
            return (productList, resultItem);
        }
        public (IEnumerable<ProductItem>, ResultItem) GetProductsWithConvertedPrice(BrandPricing brandPricing, string targetCurrency)
        {
            var products = _productRepository.GetProducts(brandPricing.Brand.BrandID, null);
   
            return ConvertPrices(products, brandPricing, targetCurrency);
        }

        public (IEnumerable<ProductItem>, ResultItem) GetProductsWithConvertedPrice(BrandPricing brandPricing, string targetCurrency, int pageNumber, int itemPerPage)
        {
            var products = _productRepository.GetProducts(brandPricing.Brand.BrandID, null, pageNumber, itemPerPage);
   
            return ConvertPrices(products, brandPricing, targetCurrency);
        }

        public (IEnumerable<ProductItem>, ResultItem) GetProductsWithConvertedPrice(List<PriceWeightbasedSettings> freightSettings, BrandPricing brandPricing, 
            SearchFilter filter, string targetCurrency, int pageNumber, int itemPerPage)
        {
            var products = _productRepository.GetProducts(brandPricing.Brand.BrandID, filter, pageNumber, itemPerPage);
   
            return ConvertPrices(freightSettings, products, brandPricing, targetCurrency);
        }

        public (IEnumerable<ProductItem>, ResultItem) GetProductsWithConvertedPrice(List<PriceWeightbasedSettings> freightSettings, BrandPricing brandPricing,
            SearchFilter filter, string targetCurrency)
        {
            
            var products = _productRepository.GetProducts(brandPricing.Brand.BrandID, filter);
   
            return ConvertPrices(freightSettings, products, brandPricing, targetCurrency);
        }


        private void ConvertItemPrices(string regionCode, ProductItem product, decimal rate, string currency, decimal importDuty)
        {
            decimal freightSurcharge;
            decimal price;
            int moq;
            (price, moq,freightSurcharge) = _priceCalculator.CalculatePrice(WeightbasedSettings.GetRegionPriceKeyName(regionCode, WeightbasedSettings.Price1KeyName), product, rate, currency, importDuty);
            product.Price1 = price;
            product.MinimumOrderQty1 = moq;
            product.FreightSurcharge1 = freightSurcharge;
            
            (price, moq,freightSurcharge) = _priceCalculator.CalculatePrice(WeightbasedSettings.GetRegionPriceKeyName(regionCode, WeightbasedSettings.Price2KeyName), product, rate, currency, importDuty);
            product.Price2 = price;
            product.MinimumOrderQty2 = moq;
            product.FreightSurcharge2 = freightSurcharge;
            
            (price, moq,freightSurcharge) = _priceCalculator.CalculatePrice(WeightbasedSettings.GetRegionPriceKeyName(regionCode, WeightbasedSettings.Price3KeyName), product, rate, currency, importDuty);
            product.Price3 = price;
            product.MinimumOrderQty3 = moq;
            product.FreightSurcharge3 = freightSurcharge;
            
            (price, moq,freightSurcharge) = _priceCalculator.CalculatePrice(WeightbasedSettings.GetRegionPriceKeyName(regionCode, WeightbasedSettings.Price4KeyName), product, rate, currency, importDuty);
            product.Price4 = price;
            product.MinimumOrderQty4 = moq;
            product.FreightSurcharge4 = freightSurcharge;
        }
        
        private void ConvertItemPrices(PriceWeightbasedSettings settings1, 
            PriceWeightbasedSettings settings2,
            PriceWeightbasedSettings settings3,
            PriceWeightbasedSettings settings4, ProductItem product, decimal rate, string currency, decimal importDuty)
        {
            decimal freightSurcharge;
            decimal price;
            int moq;
            (price, moq,freightSurcharge) = _priceCalculator.CalculatePrice(settings1, product, rate, currency, importDuty);
            product.Price1 = price;
            product.MinimumOrderQty1 = moq;
            product.FreightSurcharge1 = freightSurcharge;
            
            (price, moq,freightSurcharge) = _priceCalculator.CalculatePrice(settings2, product, rate, currency, importDuty);
            product.Price2 = price;
            product.MinimumOrderQty2 = moq;
            product.FreightSurcharge2 = freightSurcharge;
            
            (price, moq,freightSurcharge) = _priceCalculator.CalculatePrice(settings3, product, rate, currency, importDuty);
            product.Price3 = price;
            product.MinimumOrderQty3 = moq;
            product.FreightSurcharge3 = freightSurcharge;
            
            (price, moq,freightSurcharge) = _priceCalculator.CalculatePrice(settings4, product, rate, currency, importDuty);
            product.Price4 = price;
            product.MinimumOrderQty4 = moq;
            product.FreightSurcharge4 = freightSurcharge;
        }
    }
}