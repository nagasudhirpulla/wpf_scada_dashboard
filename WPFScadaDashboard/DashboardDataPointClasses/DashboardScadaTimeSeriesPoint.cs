using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardDataPointClasses
{
    public class DashboardScadaTimeSeriesPoint : IDashboardTimeSeriesPoint
    {
        public DateTime StartTime_ { get; set; }
        public DateTime EndTime_ { get; set; }

        // The Scada Point of the TimeSeries Data Configuration
        public ScadaDataPoint ScadaPoint_ { get; set; }

        // History fetch Startegy
        public string HistoryFetchStrategy_ { get; set; } = "snap";

        // Periodicity of history fetch
        public int FetchPeriodSecs_ { get; set; } = 60;

        public DashboardScadaTimeSeriesPoint(ScadaDataPoint ScadaPoint, DateTime StartTime, DateTime EndTime)
        {
            StartTime_ = StartTime;
            EndTime_ = EndTime;
            ScadaPoint_ = ScadaPoint;
        }

        // Implementing Interface
        public IDataPoint GetDataPoint()
        {
            return ScadaPoint_;
        }

        // Implementing Interface
        public DateTime GetStartTime()
        {
            return StartTime_;
        }

        // Implementing Interface
        public DateTime GetEndTime()
        {
            return EndTime_;
        }

        public void SetFetchPeriodMins(int mins)
        {
            FetchPeriodSecs_ = mins * 60;
        }

        public void SetFetchPeriodHours(int hours)
        {
            FetchPeriodSecs_ = hours * 60 * 60;
        }

        public void SetFetchPeriodDays(int days)
        {
            FetchPeriodSecs_ = days * 24 * 60 * 60;
        }        
    }
}
