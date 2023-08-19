using System.Collections.Generic;
using C3Apparel.Data.Sql;

namespace C3Apparel.Data.Modules.Classes
{
    public class EventLogInfoProvider : BaseRepository, IEventLogInfoProvider
    {
        
        public EventLogInfoProvider(IConfigurationService configurationService) : base(configurationService)
        {
        }
        public void InsertEvent(EventLogInfo eventLog)
        {
            var parameters = new Dictionary<string, object>
            {
                { "@eventCode", eventLog.EventCode },
                { "@eventDescription", eventLog.EventDescription },
                { "@eventSource", eventLog.EventSource }
                
            };
            
            ExecuteCommand($@"INSERT INTO C3_EventLog (EventCode, EventDescription, EventSource) VALUES   
                     (@eventCode, @eventDescription, @eventSource)", parameters);
        }

    }
}