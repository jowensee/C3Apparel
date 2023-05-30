
using System;

namespace C3Apparel.Data.Modules.Classes
{
    public class ImportDutyInfo
    {

        public int ImportDutyID
        {
            get;
            set;
        }

        public decimal ImportDutyAustralia
        {
            get;
            set;
        }

        public decimal ImportDutyNewZealand
        {
            get;
            set;
        }

        public Guid ImportDutyGuid
        {
            get;
            set;
        }

        public DateTime ImportDutyLastModified
        {
            get;
            set;
        }

    }
}