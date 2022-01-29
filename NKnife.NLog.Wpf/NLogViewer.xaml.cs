using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NLog;
using NLog.Common;

namespace NKnife.NLog.WPF
{
    /// <summary>
    /// NLogViewer.xaml 的交互逻辑
    /// </summary>
    public partial class NLogViewer : UserControl
    {
        public NLogViewer()
        {
            IsTargetConfigured = false;
            LogEntries = new ObservableCollection<LogEventViewModel>();

            InitializeComponent();

            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                foreach (var target in LogManager.Configuration.AllTargets.Where(t => t is NLogViewerTarget).Cast<NLogViewerTarget>())
                {
                    IsTargetConfigured = true;
                    target.LogReceived += LogReceived;
                }
            }
        }

        public ObservableCollection<LogEventViewModel> LogEntries { get; set; }

        public bool IsTargetConfigured { get; set; }

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

        [Description("Name of Time column.")]
        [Category("Data")]
        [TypeConverter(typeof(StringConverter))]
        public string TimeHeader { get; set; } = "Time";

        [Description("Name of Logger column.")]
        [Category("Data")]
        [TypeConverter(typeof(StringConverter))]
        public object LoggerNameHeader { get; set; } = "Logger";

        [Description("Name of Level column.")]
        [Category("Data")]
        [TypeConverter(typeof(StringConverter))]
        public object LevelHeader { get; set; } = "Level";

        [Description("Name of Message column.")]
        [Category("Data")]
        [TypeConverter(typeof(StringConverter))]
        public object MessageHeader { get; set; } = "Message";

        [Description("Name of Exception column.")]
        [Category("Data")]
        [TypeConverter(typeof(StringConverter))]
        public object ExceptionHeader { get; set; } = "Exception";

        public event EventHandler ItemAdded;

        protected virtual void LogReceived(AsyncLogEventInfo log)
        {
            var vm = new LogEventViewModel(log.LogEvent);

            Dispatcher.BeginInvoke(new Action(() =>
            {
                if (MaxRowCount > 0 && LogEntries.Count >= MaxRowCount)
                    LogEntries.RemoveAt(0);
                LogEntries.Add(vm);
                if (AutoScrollToLast) ScrollToLast();
                if (ItemAdded != null)
                    ItemAdded(this, (NLogEvent)log.LogEvent);
            }));
        }

        public virtual void Clear()
        {
            LogEntries.Clear();
        }

        public virtual void ScrollToFirst()
        {
            if (_LogView_.Items.Count <= 0) return;
            _LogView_.SelectedIndex = 0;
            ScrollToItem(_LogView_.SelectedItem);
        }

        public virtual void ScrollToLast()
        {
            if (_LogView_.Items.Count <= 0) return;
            _LogView_.SelectedIndex = _LogView_.Items.Count - 1;
            ScrollToItem(_LogView_.SelectedItem);
        }

        private void ScrollToItem(object item)
        {
            _LogView_.ScrollIntoView(item);
        }
    }
}
