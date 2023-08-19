using System.Collections.Generic;
using System.Data;
using C3Apparel.Data.Modules.Filters;

namespace C3Apparel.Data.Modules.Classes
{
    public interface IEventLogInfoProvider
    {
        void InsertEvent(EventLogInfo eventLog);
    }
}