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

        public string TimeSeriesType_ { get { return timeSeriesType; } set { } }
        public DateTime StartTimeAbsolute_ { get; set; }
        public DateTime EndTimeAbsolute_ { get; set; }
        public string StartTimeMode_ { get; set; } = AbsoluteMode;
        public string EndTimeMode_ { get; set; } = AbsoluteMode;
        public VariableTime StartTimeVariable_ { get; set; } = new VariableTime();
        public VariableTime EndTimeVariable_ { get; set; } = new VariableTime();
        public VariableTime FetchTime_ { get; set; } = new VariableTime(0, 0, 60);

        // The Scada Point of the TimeSeries Data Configuration
        public ScadaDataPoint ScadaPoint_ { get; set; }

        // History fetch Startegy
        public string HistoryFetchStrategy_ { get; set; } = "snap";

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
            EndTimeAbsolute_ = pnt.EndTimeAbsolute_;
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
                    //stub
                    ScadaPoint_ = scadaDataPoint;
                }
            }
        }

        public DateTime GetStartTime()
        {
            if (StartTimeMode_ == AbsoluteMode)
            {
                return StartTimeAbsolute_;
            }
            else
            {
                return DateTime.Now.AddHours(-1 * StartTimeVariable_.HoursOffset_).AddMinutes(-1 * StartTimeVariable_.MinsOffset_).AddSeconds(-1 * StartTimeVariable_.SecsOffset_);
            }
        }

        public DateTime StartTime { get { return GetStartTime(); } }

        public DateTime GetEndTime()
        {
            if (EndTimeMode_ == AbsoluteMode)
            {
                return EndTimeAbsolute_;
            }
            else
            {
                return DateTime.Now.AddHours(-1 * EndTimeVariable_.HoursOffset_).AddMinutes(-1 * EndTimeVariable_.MinsOffset_).AddSeconds(-1 * EndTimeVariable_.SecsOffset_);
            }
        }

        public DateTime EndTime { get { return GetEndTime(); } }
    }
}
