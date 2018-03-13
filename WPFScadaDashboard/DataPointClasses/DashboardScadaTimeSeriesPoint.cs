using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DataPointClasses
{
    class DashboardScadaTimeSeriesPoint : DashboardTimeSeriesPoint
    {
        // The Scada Point of the TimeSeries Data Configuration
        public DashboardScadaPoint ScadaPoint_ { get; set; }

        // History fetch Startegy
        public string HistoryFetchStrategy_ { get; set; } = "snap";

        // Periodicity of history fetch
        public int FetchPeriodSecs_ { get; set; } = 60;

        public DashboardScadaTimeSeriesPoint(DashboardScadaPoint ScadaPoint, DateTime StartTime, DateTime EndTime) : base(StartTime, EndTime)
        {
            ScadaPoint_ = ScadaPoint;
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
