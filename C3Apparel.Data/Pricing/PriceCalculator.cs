using System;
using C3Apparel.Data.Extensions;
using C3Apparel.Data.Products;

namespace C3Apparel.Data.Pricing
{
    public class PriceCalculator : IPriceCalculator
    {
        
        private readonly IProductSettingsRepository _productSettingsRepository;
        private readonly PriceGlobalSettings _priceGlobalSettings;
        private readonly AllPriceWeightBasedSettings _weightbasedSettings;
        
        public PriceCalculator(IProductSettingsRepository productSettingsRepository)
        {
            _productSettingsRepository = productSettingsRepository;
            _priceGlobalSettings = _productSettingsRepository.GetPriceGlobalSettings();
            _weightbasedSettings = _productSettingsRepository.GetAllWeightBasedPriceSettings();
        }

        public (decimal price, int moq, decimal freightSurcharge) CalculatePrice( string priceCodeName, ProductItem productItem, decimal exchangeRate, 
            string targetCurrency, decimal importDuty)
        {
            var weightBasedSettings =
                _weightbasedSettings.GetWeightBasedPriceSettings(priceCodeName);

            if (weightBasedSettings == null)
            {
                return (0, 0, 0);
            }

            var moq = 1;
            var computedBasicBuyPrice = (productItem.C3BuyPrice / exchangeRate);
            var freightCost = (productItem.C3OverrideWeight * weightBasedSettings.FreightCost(targetCurrency));
            var importDutyCost = (importDuty * computedBasicBuyPrice);
            decimal landedCost = computedBasicBuyPrice + importDutyCost + freightCost;
            
            if (!priceCodeName.Contains(WeightbasedSettings.Price1KeyName))
            {
                moq = (int)Math.Floor(weightBasedSettings.WeightInKg / productItem.C3OverrideWeight);
            }

            var unitPrice = landedCost * (1+weightBasedSettings.MarginInDecimal);
            return (unitPrice , moq, weightBasedSettings.FreightSurcharge(targetCurrency));
        }

        public (decimal price, int moq, decimal freightSurcharge) CalculatePrice(PriceWeightbasedSettings weightBasedSettings, ProductItem productItem,
            decimal exchangeRate, string targetCurrency, decimal importDuty)
        {
            

            if (weightBasedSettings == null)
            {
                return (0, 0, 0);
            }

            var moq = 1;
            var computedBasicBuyPrice = (productItem.C3BuyPrice / exchangeRate);
            var freightCost = (productItem.C3OverrideWeight * weightBasedSettings.FreightCost(targetCurrency));
            
            var importDutyCost = (importDuty * computedBasicBuyPrice);
            decimal landedCost = computedBasicBuyPrice + importDutyCost + freightCost;
            
            if (!weightBasedSettings.KeyName.Contains(WeightbasedSettings.Price1KeyName))
            {
                moq = (int)Math.Floor(weightBasedSettings.WeightInKg / productItem.C3OverrideWeight);
            }

            var unitPrice = landedCost * (1+weightBasedSettings.MarginInDecimal);
            return (unitPrice , moq, weightBasedSettings.FreightSurcharge(targetCurrency));
        }
    }
}