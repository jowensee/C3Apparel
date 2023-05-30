
using System.Collections.Generic;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IPriceSettingsInfoProvider
    {
        IEnumerable<PriceSettingsInfo> Get();
    }
}