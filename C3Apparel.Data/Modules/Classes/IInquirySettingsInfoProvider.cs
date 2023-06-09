
using System;
using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IInquirySettingsInfoProvider
    {
        InquirySettingsInfo GetSettingsByGuid(Guid guid);
        void Save(InquirySettingsInfo settings);
    }
}