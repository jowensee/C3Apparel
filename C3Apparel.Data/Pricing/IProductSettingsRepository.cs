namespace C3Apparel.Data.Pricing
{
    public interface IProductSettingsRepository
    {
        PriceGlobalSettings GetPriceGlobalSettings();
        AllPriceWeightBasedSettings GetAllWeightBasedPriceSettings();
    }
}