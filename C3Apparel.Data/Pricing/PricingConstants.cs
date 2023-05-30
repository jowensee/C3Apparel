namespace C3Apparel.Data.Pricing
{
    public static class CurrencyConstants
    {
        public const string AUD = "AUD";
        public const string NZD = "NZD";
        public const string USD = "USD";
        public const string EURO = "EURO";
    }
    
    
    public static class WeightbasedSettings
    {
        public const string Price1KeyName = "Price1";
        public const string Price2KeyName = "Price2";
        public const string Price3KeyName = "Price3";
        public const string Price4KeyName = "Price4";

        public static string GetRegionPriceKeyName(string regionCode, string priceKeyName)
        {
            return $"{regionCode}.{priceKeyName}";
        }
    }
    
    public static class Region
    {
        public const string CODE_US = "US";
        public const string CODE_EUROPE = "EURO";
    }
    
    public static class AccountConstants
    {
        public const string ROLE_AU = "AURole";
        public const string ROLE_NZ = "NZRole";
    }
}