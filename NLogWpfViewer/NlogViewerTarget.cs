using System;
using NLog.Common;
using NLog.Targets;

namespace NLogWpfViewer
{
    [Target("NLogViewer")]
    public class NLogViewerTarget : Target
    {
        public event Action<AsyncLogEventInfo> LogReceived;

        protected override void Write(AsyncLogEventInfo logEvent)
        {
            base.Write(logEvent);

            if (LogReceived != null)
                LogReceived(logEvent);
        }
    }
}