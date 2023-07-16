using System;
using C3Apparel.Data.Pricing;

namespace C3Apparel.Data.Helpers
{
    public static class CountryHelper
    {
        public static string GetCountryCurrencyCode(string countryCode)
        {
            if (countryCode.Equals(CountryConstants.AU, StringComparison.CurrentCultureIgnoreCase))
            {
                return CurrencyConstants.AUD;
            }
            
            if (countryCode.Equals(CountryConstants.NZ, StringComparison.CurrentCultureIgnoreCase))
            {
                return CurrencyConstants.NZD;
            }

            return string.Empty;
        }
        
        public static string GetCountryName(string countryCode)
        {
            if (countryCode.Equals(CountryConstants.AU, StringComparison.CurrentCultureIgnoreCase))
            {
                return "Australia";
            }
            
            if (countryCode.Equals(CountryConstants.NZ, StringComparison.CurrentCultureIgnoreCase))
            {
                return "New Zealand";
            }

            return string.Empty;
        }
    }
}