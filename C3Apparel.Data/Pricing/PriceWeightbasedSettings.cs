namespace C3Apparel.Data.Pricing
{
    public class PriceWeightbasedSettings
    {
        public string KeyName { get; }
        public decimal WeightInKg { get; }
        public decimal MarginInDecimal { get; }
        
        public decimal AUFreightPerKg { get; }
        public decimal NZFreightPerKg { get; }

        public decimal AUFreightSurcharge { get; }
        public decimal NZFreightSurcharge { get; }
        public string ColumnHeader1 { get; }
        public string ColumnHeader2 { get; }
        
        public decimal FreightCost(string currency)
        {
            if (currency == CurrencyConstants.AUD)
            {
                return AUFreightPerKg;
            }
            
            if (currency == CurrencyConstants.NZD)
            {
                return NZFreightPerKg;
            }

            return 0;
        }

        public decimal TotalFreightCost(string currency)
        {
            if (currency == CurrencyConstants.AUD)
            {
                return WeightInKg * AUFreightPerKg;
            }
            
            if (currency == CurrencyConstants.NZD)
            {
                return WeightInKg * NZFreightPerKg;
            }

            return 0;
        }

        public decimal FreightSurcharge(string currency)
        {
            if (currency == CurrencyConstants.AUD)
            {
                return AUFreightSurcharge;
            }
            
            if (currency == CurrencyConstants.NZD)
            {
                return NZFreightSurcharge;
            }

            return 0;
        }

        public PriceWeightbasedSettings(string keyName, decimal weightInKg, decimal marginInDecimal, decimal auFreightPerKg, decimal nzFreightPerKg, decimal auFreightSurcharge, decimal nzFreightSurcharge, string columnHeader1, string columnHeader2)
        {
            KeyName = keyName;
            WeightInKg = weightInKg;
            MarginInDecimal = marginInDecimal;
            AUFreightPerKg = auFreightPerKg;
            NZFreightPerKg = nzFreightPerKg;
            AUFreightSurcharge = auFreightSurcharge;
            NZFreightSurcharge = nzFreightSurcharge;
            ColumnHeader1 = columnHeader1;
            ColumnHeader2 = columnHeader2;
        }
    }
}