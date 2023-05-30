
using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IInquirySettingsInfoProvider
    {
        IEnumerable<InquirySettingsInfo> Get(string where);
    }
}