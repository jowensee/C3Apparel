using System;

namespace C3Apparel.Data.Modules.Classes
{

    public class ExchangeRateInfo
    {

        public int ExchangeRateID
        {
            get;
            set;
        }

        public string ExchangeRateSourceCurrency
        {
            get;
            set;
        }

        public decimal ExchangeRateAUDValue
        {
            get;
            set;
        }

        public decimal ExchangeRateNZDValue
        {
            get;
            set;
        }

        public DateTime ExchangeRateValidFrom
        {
            get;
            set;
        }

        public DateTime ExchangeRateValidTo
        {
            get;
            set;
        }

        public Guid ExchangeRateGuid
        {
            get;
            set;
        }

        public DateTime ExchangeRateLastModified
        {
            get;
            set;
        }


    }
}