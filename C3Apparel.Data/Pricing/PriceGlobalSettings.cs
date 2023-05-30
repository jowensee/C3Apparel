namespace C3Apparel.Data.Pricing
{
    public class PriceGlobalSettings
    {
        public decimal AUImportDutyInDecimal { get; }
        public decimal NZImportDutyInDecimal { get; }


        public decimal GetImportDuty(string currency)
        {
            if (currency == CurrencyConstants.AUD)
            {
                return AUImportDutyInDecimal;
            }

            
            if (currency == CurrencyConstants.NZD)
            {
                return NZImportDutyInDecimal;
            }
            
            return 0;
        }
        public PriceGlobalSettings(decimal auImportDutyInDecimal, decimal nzImportDutyInDecimal)
        {
            AUImportDutyInDecimal = auImportDutyInDecimal;
            NZImportDutyInDecimal = nzImportDutyInDecimal;
        }
    }
}