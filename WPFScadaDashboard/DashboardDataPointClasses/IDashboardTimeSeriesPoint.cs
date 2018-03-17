using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardDataPointClasses
{
    public interface IDashboardTimeSeriesPoint
    {
        DateTime GetStartTime();

        DateTime GetEndTime();

        IDataPoint GetDataPoint();
    }
}
