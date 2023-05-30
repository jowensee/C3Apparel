

using System;

namespace C3Apparel.Data.Modules.Classes
{

    public partial class InquirySettingsInfo
    {

        public int InquirySettingsID
        {
            get;
            set;
        }
        public string InquirySettingsName
        {
            get;
            set;
        }

        public string InquirySettingsJsonString
        {
            get;
            set;
        }

        public Guid InquirySettingsGuid
        {
            get;
            set;
        }

        public DateTime InquirySettingsModifiedWhen
        {
            get;
            set;
        }

    }
}