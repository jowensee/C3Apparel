using C3Apparel.Data.Pricing;

namespace C3Apparel.Data.Products
{
    public class BrandPricing
    {
        public BrandItem Brand { get; }
        public ExchangeRateItem ExchangeRate { get; set; }
        public decimal ImportDuty { get; set; }
        
        public bool IsValid => Brand != null && ExchangeRate != null;

        public BrandPricing(BrandItem brand, ExchangeRateItem exchangeRate, decimal importDuty)
        {
            Brand = brand;
            ExchangeRate = exchangeRate;
            ImportDuty = importDuty;
        }
        
    }
}