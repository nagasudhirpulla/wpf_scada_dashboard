using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DataPointClasses;

namespace WPFScadaDashboard.DataFetchers
{
    public interface IFetcherBase
    {
        PointResult FetchCurrentPointData(DashBoardDataPoint point);

        List<PointResult> FetchHistoricalPointData(IDashboardTimeSeriesPoint dashboardTimeSeriesPoint);
    }
}
