using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DataPointClasses
{
    class DashboardTimeSeriesPoint
    {
        public DateTime StartTime_ { get; set; }
        public DateTime EndTime_ { get; set; }

        public DashboardTimeSeriesPoint(DateTime StartTime, DateTime EndTime)
        {
            StartTime_ = StartTime;
            EndTime_ = EndTime;
        }
    }
}
