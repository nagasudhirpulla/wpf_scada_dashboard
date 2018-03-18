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

        // Set Position of the Cell
        void SetCellPosition(DashboardCellPosition dashboardCellPosition);

        // Get Type of DataViz like linePlot, MapPlot etc. This might be useful for deserialization and serialization
        string GetVizType();

        List<IDataPoint> GetCellDataPoints();

        double GetCellWidth();

        double GetCellHeight();

        string GetWidthMode();

        string GetHeightMode();

        void SetCellWidth(double width);

        void SetCellHeight(double height);

        void SetWidthMode(string mode);

        void SetHeightMode(string mode);

    }
}
