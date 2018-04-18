using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardDataPointClasses
{
    public interface IDashboardTimeSeriesPoint
    {
        string TimeSeriesType_ { get; set; }

        DateTime GetStartTime();
        DateTime StartTime { get; }

        DateTime GetEndTime();
        DateTime EndTime { get; }

        IDataPoint GetDataPoint();
        IDataPoint DataPoint { get; set; }
    }
}
