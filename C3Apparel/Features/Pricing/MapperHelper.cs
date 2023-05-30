using System.Collections.Generic;
using C3Apparel.Data.Pricing;
using C3Apparel.Web.Features.Content.API.Requests;

namespace C3Apparel.Web.Features.Pricing;

public static class MapperHelper
{
    public static List<PriceWeightbasedSettings> Map(PricingSettings settings, string regionCode)
    {
        var priceSettings = new List<PriceWeightbasedSettings>();
        if (regionCode == Region.CODE_US)
        {
            for (var i = 1; i <= settings.UsFreightSettings.Count; i++)
            {
                var setting = settings.UsFreightSettings[i-1];
                priceSettings.Add(new PriceWeightbasedSettings($"{regionCode}.Price{i}", setting.WeightInKg, (decimal) setting.MarginInDecimal,
                    setting.AuFreightPerKg, setting.NzFreightPerKg, setting.AuFreightSurcharge, setting.NzFreightSurcharge, $"Price Column {i}", "Air Express"));
            }
        }
        
        if (regionCode == Region.CODE_EUROPE)
        {
            for (var i = 1; i <= settings.EuroFreightSettings.Count; i++)
            {
                var setting = settings.EuroFreightSettings[i-1];
                priceSettings.Add(new PriceWeightbasedSettings($"{regionCode}.Price{i}", setting.WeightInKg, (decimal) setting.MarginInDecimal,
                    setting.AuFreightPerKg, setting.NzFreightPerKg, setting.AuFreightSurcharge, setting.NzFreightSurcharge, $"Price Column {i}", "Air Express"));
            }
        }

        return priceSettings;
    }
}