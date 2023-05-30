

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

    }
}