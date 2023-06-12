using System.Linq;
using C3Apparel.Data.Modules.Classes;

namespace C3Apparel.Data.Pricing
{
    public class ProductSettingsRepository : IProductSettingsRepository
    {
        private readonly IPriceSettingsInfoProvider _priceSettingsInfoProvider;
        private readonly IImportDutyInfoProvider _importDutyInfoProvider;
        public ProductSettingsRepository(IPriceSettingsInfoProvider priceSettingsInfoProvider, IImportDutyInfoProvider importDutyInfoProvider)
        {
            _priceSettingsInfoProvider = priceSettingsInfoProvider;
            _importDutyInfoProvider = importDutyInfoProvider;
        }
        
        public PriceGlobalSettings GetPriceGlobalSettings()
        {
            var importDuty = _importDutyInfoProvider.Get();

            if (importDuty == null)
            {
                return new PriceGlobalSettings(0, 0);
            }
            return new PriceGlobalSettings(
                importDuty.ImportDutyAustralia * (decimal) .01,
                importDuty.ImportDutyNewZealand * (decimal) .01
            );
        }

        public AllPriceWeightBasedSettings GetAllWeightBasedPriceSettings()
        {
            return new AllPriceWeightBasedSettings(_priceSettingsInfoProvider.Get().ToDictionary(a=>a.PriceSettingsCodeName, a=> 
                new PriceWeightbasedSettings(a.PriceSettingsCodeName,a.Weight, a.C3MarginPercent * (decimal) 0.01, 
                    a.AUFreightPerKg, a.NZFreightPerKg, a.AUFreightSurcharge, a.NZFreightSurcharge,
                    a.ColumnHeaderText1, a.ColumnHeaderText2)));
        }
    }
}