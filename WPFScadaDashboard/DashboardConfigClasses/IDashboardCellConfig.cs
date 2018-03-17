using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DashboardDataPointClasses;

namespace WPFScadaDashboard.DashboardConfigClasses
{
    public interface IDashboardCellConfig
    {
        // Get Name of the Cell
        string GetName();

        // Get Position of the Cell
        DashboardCellPosition GetCellPosition();

        // Get Type of DataViz like linePlot, MapPlot etc. This might be useful for deserialization and serialization
        string GetVizType();

        List<IDataPoint> GetCellDataPoints();

    }
}
