using System;
using NLog.Common;
using NLog.Targets;

namespace NKnife.NLogWpfViewer
{
    [Target("NLogViewer")]
    public class NLogViewerTarget : Target
    {
        public event Action<AsyncLogEventInfo> LogReceived;

        protected override void Write(NLog.Common.AsyncLogEventInfo logEvent)
        {
            base.Write(logEvent);

            if (LogReceived != null)
                LogReceived(logEvent);
        }
    }
}
