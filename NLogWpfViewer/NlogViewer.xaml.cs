using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using NLog;
using NLog.Common;

namespace NLogWpfViewer
{
    /// <summary>
    ///     Interaction logic for NLogViewer.xaml
    /// </summary>
    public partial class NLogViewer : UserControl
    {
        public NLogViewer()
        {
            IsTargetConfigured = false;
            LogEntries = new ObservableCollection<LogEventViewModel>();

            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
                foreach (var target in LogManager.Configuration.AllTargets.Where(t => t is NLogViewerTarget).Cast<NLogViewerTarget>())
                {
                    IsTargetConfigured = true;
                    target.LogReceived += LogReceived;
                }
        }

        public ListView LogView => logView;
        public ObservableCollection<LogEventViewModel> LogEntries { get; }
        public bool IsTargetConfigured { get; }

        [Description("Width of time column in pixels")]
        [Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double TimeWidth { get; set; } = 120;

        [Description("Width of Logger column in pixels, or auto if not specified")]
        [Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double LoggerNameWidth { get; set; } = 50;

        [Description("Width of Level column in pixels")]
        [Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double LevelWidth { get; set; } = 50;

        [Description("Width of Message column in pixels")]
        [Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double MessageWidth { get; set; } = 200;

        [Description("Width of Exception column in pixels")]
        [Category("Data")]
        [TypeConverter(typeof(LengthConverter))]
        public double ExceptionWidth { get; set; } = 75;

        [Description("The maximum number of row count. The oldest log gets deleted. Set to 0 for unlimited count.")]
        [Category("Data")]
        [TypeConverter(typeof(Int32Converter))]
        public int MaxRowCount { get; set; } = 50;

        [Description("Automatically scrolls to the last log item in the viewer. Default is true.")]
        [Category("Data")]
        [TypeConverter(typeof(BooleanConverter))]
        public bool AutoScrollToLast { get; set; } = true;

        public event EventHandler ItemAdded = delegate { };

        protected void LogReceived(AsyncLogEventInfo log)
        {
            var vm = new LogEventViewModel(log.LogEvent);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (MaxRowCount > 0 && LogEntries.Count >= MaxRowCount)
                    LogEntries.RemoveAt(0);
                LogEntries.Add(vm);
                if (AutoScrollToLast) ScrollToLast();
                ItemAdded(this, (NLogEvent)log.LogEvent);
            }));
        }

        public void Clear()
        {
            LogEntries.Clear();
        }

        public void ScrollToFirst()
        {
            if (LogView.Items.Count <= 0) return;
            LogView.SelectedIndex = 0;
            ScrollToItem(LogView.SelectedItem);
        }

        public void ScrollToLast()
        {
            if (LogView.Items.Count <= 0) return;
            LogView.SelectedIndex = LogView.Items.Count - 1;
            ScrollToItem(LogView.SelectedItem);
        }

        private void ScrollToItem(object item)
        {
            LogView.ScrollIntoView(item);
        }
    }
}