using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DataPointClasses;

namespace WPFScadaDashboard.DataFetchers
{
    class ScadaFetcher : IFetcherBase
    {
        public PointResult FetchCurrentPointData(DashBoardDataPoint point)
        {
            return null;
        }

        public List<PointResult> FetchHistoricalPointData(IDashboardTimeSeriesPoint dashboardTimeSeriesPoint)
        {
            return null;
        }
    }
}
