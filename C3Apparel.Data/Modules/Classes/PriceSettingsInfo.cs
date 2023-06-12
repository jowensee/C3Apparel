

using System;

namespace C3Apparel.Data.Modules.Classes
{
    public partial class PriceSettingsInfo
    {

        public int PriceSettingsID
        {
            get;
            set;
        }
        
        public string PriceSettingsName
        {
            get;
            set;
        }

        public string PriceSettingsCodeName
        {
            get;
            set;
        }

        public decimal Weight
        {
            get;
            set;
        }

        public decimal C3MarginPercent
        {
            get;
            set;
        }

        public decimal AUFreightPerKg
        {
            get;
            set;
        }

        public decimal NZFreightPerKg
        {
            get;
            set;
        }

        public decimal AUFreightSurcharge
        {
            get;
            set;
        }

        public decimal NZFreightSurcharge
        {
            get;
            set;
        }

        public string ColumnHeaderText1
        {
            get;
            set;
        }

        public string ColumnHeaderText2
        {
            get;
            set;
        }

        public Guid PriceSettingsGuid
        {
            get;
            set;
        }

        public DateTime PriceSettingsLastModified
        {
            get;
            set;
        }

        public static PriceSettingsInfo Create(string codeName, decimal weightInKg, decimal marginInDecimal, 
            decimal auFreightPerKg, decimal nzFreightPerKg, decimal auFreightSurcharge, decimal nzFreightSurcharge,
            string columnHeader1, string colunmHeader2)
        {
            return new PriceSettingsInfo
            {
                PriceSettingsCodeName = codeName, Weight = weightInKg, C3MarginPercent = marginInDecimal,
                AUFreightPerKg = auFreightPerKg, NZFreightPerKg = nzFreightPerKg,
                AUFreightSurcharge = auFreightSurcharge,
                NZFreightSurcharge = nzFreightSurcharge,
                ColumnHeaderText1 = columnHeader1,
                ColumnHeaderText2 = colunmHeader2
            };
        }
    }
}