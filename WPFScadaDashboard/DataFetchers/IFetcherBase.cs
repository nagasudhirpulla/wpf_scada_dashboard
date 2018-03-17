using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DashboardDataPointClasses;

namespace WPFScadaDashboard.DataFetchers
{
    public interface IFetcherBase
    {
        IPointResult FetchCurrentPointData(IDataPoint point);

        List<IPointResult> FetchHistoricalPointData(IDashboardTimeSeriesPoint dashboardTimeSeriesPoint);
    }
}
