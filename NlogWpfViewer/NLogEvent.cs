using System;
using NLog;

namespace NLogWpfViewer
{
    public class NLogEvent : EventArgs
    {
        public LogEventInfo EventInfo;

        public NLogEvent(LogEventInfo LogEventInfo)
        {
            // TODO: Complete member initialization
            EventInfo = LogEventInfo;
        }


        public static implicit operator LogEventInfo(NLogEvent e)
        {
            return e.EventInfo;
        }

        public static implicit operator NLogEvent(LogEventInfo e)
        {
            return new NLogEvent(e);
        }
    }
}