using C3Apparel.Data.Products;

namespace C3Apparel.Data.Pricing
{
    public interface IPriceCalculator
    {
        (decimal price, int moq, decimal freightSurcharge) CalculatePrice( string priceCodeName, ProductItem productItem, decimal exchangeRate, string targetCurrency, decimal importDuty);
        
        (decimal price, int moq, decimal freightSurcharge) CalculatePrice(PriceWeightbasedSettings settings , ProductItem productItem, decimal exchangeRate, string targetCurrency, decimal importDuty);
    }
}