using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardDataPointClasses
{
    public class DashboardScadaTimeSeriesPoint : IDashboardTimeSeriesPoint
    {
        public const string timeSeriesType = "ScadaTimeSeriesPoint";
        public const string AbsoluteMode = "absolute";
        public const string VariableMode = "variable";
        public const string AbsoluteDateMode = "absolute_date";
        public const string VariableDateMode = "variable_date";
        public const string FetchStrategySnap = "snap";
        public const string FetchStrategyAverage = "average";
        public const string FetchStrategyMax = "max";
        public const string FetchStrategyMin = "min";
        public const string FetchStrategyRaw = "raw";


        public string TimeSeriesType_ { get { return timeSeriesType; } set { } }
        public DateTime StartTimeAbsolute_ { get; set; }
        public DateTime EndTimeAbsolute_ { get; set; }
        public string StartDateMode_ { get; set; } = AbsoluteDateMode;
        public int StartDateOffset_ { get; set; } = 0;
        public string StartTimeMode_ { get; set; } = AbsoluteMode;
        public string EndDateMode_ { get; set; } = AbsoluteDateMode;
        public int EndDateOffset_ { get; set; } = 0;
        public string EndTimeMode_ { get; set; } = AbsoluteMode;
        public VariableTime StartTimeVariable_ { get; set; } = new VariableTime();
        public VariableTime EndTimeVariable_ { get; set; } = new VariableTime();
        public VariableTime FetchTime_ { get; set; } = new VariableTime(0, 0, 60);
        public string ColorString_ { get; set; } = "";

        // The Scada Point of the TimeSeries Data Configuration
        public ScadaDataPoint ScadaPoint_ { get; set; }

        // History fetch Startegy
        public string HistoryFetchStrategy_ { get; set; } = FetchStrategySnap;

        // Periodicity of history fetch
        public int FetchPeriodSecs_ { get { return 3600 * FetchTime_.HoursOffset_ + 60 * FetchTime_.MinsOffset_ + FetchTime_.SecsOffset_; } }

        public DashboardScadaTimeSeriesPoint(ScadaDataPoint ScadaPoint, DateTime StartTime, DateTime EndTime)
        {
            StartTimeAbsolute_ = StartTime;
            EndTimeAbsolute_ = EndTime;
            ScadaPoint_ = ScadaPoint;
        }

        public DashboardScadaTimeSeriesPoint(DashboardScadaTimeSeriesPoint pnt)
        {
            StartTimeAbsolute_ = pnt.StartTimeAbsolute_;
            StartTimeVariable_ = pnt.StartTimeVariable_;
            StartTimeMode_ = pnt.StartTimeMode_;
            StartDateMode_ = pnt.StartDateMode_;
            StartDateOffset_ = pnt.StartDateOffset_;
            EndTimeAbsolute_ = pnt.EndTimeAbsolute_;
            EndTimeVariable_ = pnt.EndTimeVariable_;
            EndTimeMode_ = pnt.EndTimeMode_;
            EndDateMode_ = pnt.EndDateMode_;
            EndDateOffset_ = pnt.EndDateOffset_;
            FetchTime_ = pnt.FetchTime_;
            HistoryFetchStrategy_ = pnt.HistoryFetchStrategy_;
            ColorString_ = pnt.ColorString_;
            ScadaPoint_ = new ScadaDataPoint(pnt.ScadaPoint_);
        }

        // created just for the sake of the deserialization
        public DashboardScadaTimeSeriesPoint()
        {
            StartTimeAbsolute_ = DateTime.Now;
            EndTimeAbsolute_ = DateTime.Now;
            ScadaPoint_ = new ScadaDataPoint("");
        }

        public IDataPoint GetDataPoint()
        {
            return ScadaPoint_;
        }

        public IDataPoint DataPoint
        {
            get
            {
                return ScadaPoint_;
            }
            set
            {
                if (value is ScadaDataPoint scadaDataPoint)
                {
                    ScadaPoint_ = scadaDataPoint;
                }
            }
        }

        public DateTime GetStartTime()
        {
            return Helpers.ListUtility.GetDateTime(StartTimeAbsolute_, StartTimeVariable_, StartTimeMode_, StartDateMode_, StartDateOffset_);
        }

        public DateTime StartTime { get { return GetStartTime(); } }

        public DateTime GetEndTime()
        {
            return Helpers.ListUtility.GetDateTime(EndTimeAbsolute_, EndTimeVariable_, EndTimeMode_, EndDateMode_, EndDateOffset_);
        }

        public DateTime EndTime { get { return GetEndTime(); } }
    }
}
