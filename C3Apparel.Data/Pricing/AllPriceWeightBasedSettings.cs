using System.Collections.Generic;

namespace C3Apparel.Data.Pricing
{
    public class AllPriceWeightBasedSettings
    {
        public Dictionary<string, PriceWeightbasedSettings> AllPriceWeightbasedSettings;

        public AllPriceWeightBasedSettings(Dictionary<string, PriceWeightbasedSettings> allPriceWeightbasedSettings)
        {
            AllPriceWeightbasedSettings = allPriceWeightbasedSettings;
        }


        public PriceWeightbasedSettings GetWeightBasedPriceSettings(string keyName)
        {

            if (AllPriceWeightbasedSettings == null || !AllPriceWeightbasedSettings.ContainsKey(keyName))
            {
                return null;
            }

            return AllPriceWeightbasedSettings[keyName];
        }
    }
}