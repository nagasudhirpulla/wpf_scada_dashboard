using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using WPFScadaDashboard.DashboardDataPointClasses;

namespace WPFScadaDashboard.DashboardUserControls
{
    /// <summary>
    /// Interaction logic for ScadaTimeSeriesPointEditWindow.xaml
    /// </summary>
    public partial class ScadaTimeSeriesPointEditWindow : Window
    {
        public ScadaTimeSeriesPointVM scadaTimeSeriesPointVM;
        public ScadaTimeSeriesPointEditWindow(DashboardScadaTimeSeriesPoint pnt)
        {
            InitializeComponent();
            scadaTimeSeriesPointVM = new ScadaTimeSeriesPointVM(pnt);
            ScadaPointEditForm.DataContext = scadaTimeSeriesPointVM;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Update Data Point ?", "Scada Timeseries Data Point Configuration", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            else
            {
                DialogResult = true;
            }                        
        }

        // https://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Helpers.NumericTextValidation.AreAllValidNumericChars(e.Text);
            base.OnPreviewTextInput(e);
        }
    }

    public class ScadaTimeSeriesPointVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DashboardScadaTimeSeriesPoint ScadaTimeSeriesPoint { get; set; }

        public ScadaTimeSeriesPointVM(DashboardScadaTimeSeriesPoint scadaTimeSeriesPoint)
        {
            this.ScadaTimeSeriesPoint = scadaTimeSeriesPoint;
        }
        public List<string> HourStrings { get; set; } = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" };
        public List<string> MinuteStrings { get; set; } = new List<string> { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23", "24", "25", "26", "27", "28", "29", "30", "31", "32", "33", "34", "35", "36", "37", "38", "39", "40", "41", "42", "43", "44", "45", "46", "47", "48", "49", "50", "51", "52", "53", "54", "55", "56", "57", "58", "59" };

        public List<string> DateLimitsModes { get; set; } = new List<string> { DashboardScadaTimeSeriesPoint.VariableMode, DashboardScadaTimeSeriesPoint.AbsoluteMode };

        public int StartDateMode { get { return DateLimitsModes.IndexOf(ScadaTimeSeriesPoint.StartTimeMode_); } set { ScadaTimeSeriesPoint.StartTimeMode_ = DateLimitsModes.ElementAt(value); NotifyPropertyChanged("StartDateModeStr"); } }
        public string StartDateModeStr
        {
            get { return ScadaTimeSeriesPoint.StartTimeMode_; }
            set
            {
                int modeInt = DateLimitsModes.IndexOf(value);
                if (modeInt != -1)
                {
                    ScadaTimeSeriesPoint.StartTimeMode_ = value;
                }
            }
        }

        public DateTime StartDate { get { return ScadaTimeSeriesPoint.StartTimeAbsolute_; } set { ScadaTimeSeriesPoint.StartTimeAbsolute_ = new DateTime(value.Year, value.Month, value.Day, ScadaTimeSeriesPoint.StartTimeAbsolute_.Hour, ScadaTimeSeriesPoint.StartTimeAbsolute_.Minute, ScadaTimeSeriesPoint.StartTimeAbsolute_.Second); } }
        public int StartTimeHoursIndex { get { return ScadaTimeSeriesPoint.StartTimeAbsolute_.Hour; } set { ScadaTimeSeriesPoint.StartTimeAbsolute_ = new DateTime(ScadaTimeSeriesPoint.StartTimeAbsolute_.Year, ScadaTimeSeriesPoint.StartTimeAbsolute_.Month, ScadaTimeSeriesPoint.StartTimeAbsolute_.Day, value, ScadaTimeSeriesPoint.StartTimeAbsolute_.Minute, ScadaTimeSeriesPoint.StartTimeAbsolute_.Second); } }
        public int StartTimeMinsIndex { get { return ScadaTimeSeriesPoint.StartTimeAbsolute_.Minute; } set { ScadaTimeSeriesPoint.StartTimeAbsolute_ = new DateTime(ScadaTimeSeriesPoint.StartTimeAbsolute_.Year, ScadaTimeSeriesPoint.StartTimeAbsolute_.Month, ScadaTimeSeriesPoint.StartTimeAbsolute_.Day, ScadaTimeSeriesPoint.StartTimeAbsolute_.Hour, value, ScadaTimeSeriesPoint.StartTimeAbsolute_.Second); } }
        public int StartTimeSecsIndex { get { return ScadaTimeSeriesPoint.StartTimeAbsolute_.Second; } set { ScadaTimeSeriesPoint.StartTimeAbsolute_ = new DateTime(ScadaTimeSeriesPoint.StartTimeAbsolute_.Year, ScadaTimeSeriesPoint.StartTimeAbsolute_.Month, ScadaTimeSeriesPoint.StartTimeAbsolute_.Day, ScadaTimeSeriesPoint.StartTimeAbsolute_.Hour, ScadaTimeSeriesPoint.StartTimeAbsolute_.Minute, value); } }

        public string StartHoursVariable
        {
            get { return ScadaTimeSeriesPoint.StartTimeVariable_.HoursOffset_.ToString(); }
            set
            {
                int intVal = ScadaTimeSeriesPoint.StartTimeVariable_.HoursOffset_;
                if (int.TryParse(value, out intVal))
                {
                    ScadaTimeSeriesPoint.StartTimeVariable_.HoursOffset_ = intVal;
                }
            }
        }

        public string StartMinsVariable
        {
            get { return ScadaTimeSeriesPoint.StartTimeVariable_.MinsOffset_.ToString(); }
            set
            {
                int intVal = ScadaTimeSeriesPoint.StartTimeVariable_.MinsOffset_;
                if (int.TryParse(value, out intVal))
                {
                    ScadaTimeSeriesPoint.StartTimeVariable_.MinsOffset_ = intVal;
                }
            }
        }

        public string StartSecsVariable
        {
            get { return ScadaTimeSeriesPoint.StartTimeVariable_.SecsOffset_.ToString(); }
            set
            {
                int intVal = ScadaTimeSeriesPoint.StartTimeVariable_.SecsOffset_;
                if (int.TryParse(value, out intVal))
                {
                    ScadaTimeSeriesPoint.StartTimeVariable_.SecsOffset_ = intVal;
                }
            }
        }

        public string EndHoursVariable
        {
            get { return ScadaTimeSeriesPoint.EndTimeVariable_.HoursOffset_.ToString(); }
            set
            {
                int intVal = ScadaTimeSeriesPoint.EndTimeVariable_.HoursOffset_;
                if (int.TryParse(value, out intVal))
                {
                    ScadaTimeSeriesPoint.EndTimeVariable_.HoursOffset_ = intVal;
                }
            }
        }

        public string EndMinsVariable
        {
            get { return ScadaTimeSeriesPoint.EndTimeVariable_.MinsOffset_.ToString(); }
            set
            {
                int intVal = ScadaTimeSeriesPoint.EndTimeVariable_.MinsOffset_;
                if (int.TryParse(value, out intVal))
                {
                    ScadaTimeSeriesPoint.EndTimeVariable_.MinsOffset_ = intVal;
                }
            }
        }

        public string EndSecsVariable
        {
            get { return ScadaTimeSeriesPoint.EndTimeVariable_.SecsOffset_.ToString(); }
            set
            {
                int intVal = ScadaTimeSeriesPoint.EndTimeVariable_.SecsOffset_;
                if (int.TryParse(value, out intVal))
                {
                    ScadaTimeSeriesPoint.EndTimeVariable_.SecsOffset_ = intVal;
                }
            }
        }

        public int EndDateMode { get { return DateLimitsModes.IndexOf(ScadaTimeSeriesPoint.EndTimeMode_); } set { ScadaTimeSeriesPoint.EndTimeMode_ = DateLimitsModes.ElementAt(value); NotifyPropertyChanged("EndDateModeStr"); } }
        public string EndDateModeStr
        {
            get { return ScadaTimeSeriesPoint.EndTimeMode_; }
            set
            {
                int modeInt = DateLimitsModes.IndexOf(value);
                if (modeInt != -1)
                {
                    ScadaTimeSeriesPoint.EndTimeMode_ = value;
                }
            }
        }
        public DateTime EndDate { get { return ScadaTimeSeriesPoint.EndTimeAbsolute_; } set { ScadaTimeSeriesPoint.EndTimeAbsolute_ = new DateTime(value.Year, value.Month, value.Day, ScadaTimeSeriesPoint.EndTimeAbsolute_.Hour, ScadaTimeSeriesPoint.EndTimeAbsolute_.Minute, ScadaTimeSeriesPoint.EndTimeAbsolute_.Second); } }
        public int EndTimeHoursIndex { get { return ScadaTimeSeriesPoint.EndTimeAbsolute_.Hour; } set { ScadaTimeSeriesPoint.EndTimeAbsolute_ = new DateTime(ScadaTimeSeriesPoint.EndTimeAbsolute_.Year, ScadaTimeSeriesPoint.EndTimeAbsolute_.Month, ScadaTimeSeriesPoint.EndTimeAbsolute_.Day, value, ScadaTimeSeriesPoint.EndTimeAbsolute_.Minute, ScadaTimeSeriesPoint.EndTimeAbsolute_.Second); } }
        public int EndTimeMinsIndex { get { return ScadaTimeSeriesPoint.EndTimeAbsolute_.Minute; } set { ScadaTimeSeriesPoint.EndTimeAbsolute_ = new DateTime(ScadaTimeSeriesPoint.EndTimeAbsolute_.Year, ScadaTimeSeriesPoint.EndTimeAbsolute_.Month, ScadaTimeSeriesPoint.EndTimeAbsolute_.Day, ScadaTimeSeriesPoint.EndTimeAbsolute_.Hour, value, ScadaTimeSeriesPoint.EndTimeAbsolute_.Second); } }
        public int EndTimeSecsIndex { get { return ScadaTimeSeriesPoint.EndTimeAbsolute_.Second; } set { ScadaTimeSeriesPoint.EndTimeAbsolute_ = new DateTime(ScadaTimeSeriesPoint.EndTimeAbsolute_.Year, ScadaTimeSeriesPoint.EndTimeAbsolute_.Month, ScadaTimeSeriesPoint.EndTimeAbsolute_.Day, ScadaTimeSeriesPoint.EndTimeAbsolute_.Hour, ScadaTimeSeriesPoint.EndTimeAbsolute_.Minute, value); } }
        public string FetchWindowHrs
        {
            get { return ScadaTimeSeriesPoint.FetchTime_.HoursOffset_.ToString(); }
            set
            {
                int intVal = ScadaTimeSeriesPoint.FetchTime_.HoursOffset_;
                if (int.TryParse(value, out intVal))
                {
                    ScadaTimeSeriesPoint.FetchTime_.HoursOffset_ = intVal;
                }
            }
        }

        public string FetchWindowMins
        {
            get { return ScadaTimeSeriesPoint.FetchTime_.MinsOffset_.ToString(); }
            set
            {
                int intVal = ScadaTimeSeriesPoint.FetchTime_.MinsOffset_;
                if (int.TryParse(value, out intVal))
                {
                    ScadaTimeSeriesPoint.FetchTime_.MinsOffset_ = intVal;
                }
            }
        }

        public string FetchWindowSecs
        {
            get { return ScadaTimeSeriesPoint.FetchTime_.SecsOffset_.ToString(); }
            set
            {
                int intVal = ScadaTimeSeriesPoint.FetchTime_.SecsOffset_;
                if (int.TryParse(value, out intVal))
                {
                    ScadaTimeSeriesPoint.FetchTime_.SecsOffset_ = intVal;
                }
            }
        }
    }

    public class IsVariableDateVisibleConverter : IValueConverter
    {
        public IsVariableDateVisibleConverter() { }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string modeString = (string)value;
            if (modeString == "variable")
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
                return "variable";
            else
                return "absolute";
        }
    }

    public class IsAbsoluteDateVisibleConverter : IValueConverter
    {
        public IsAbsoluteDateVisibleConverter() { }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string modeString = (string)value;
            if (modeString == "absolute")
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility visibility = (Visibility)value;

            if (visibility == Visibility.Visible)
                return "absolute";
            else
                return "variable";
        }
    }
}
