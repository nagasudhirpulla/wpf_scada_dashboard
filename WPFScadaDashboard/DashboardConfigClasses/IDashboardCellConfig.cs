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
        // Name of the Cell
        string GetName();
        void SetName(String name);

        // Position of the Cell
        DashboardCellPosition GetCellPosition();
        void SetCellPosition(DashboardCellPosition dashboardCellPosition);

        // Get Type of DataViz like linePlot, MapPlot etc. This might be useful for deserialization and serialization
        string GetVizType();

        List<IDataPoint> GetCellDataPoints();

        double GetCellWidth();
        void SetCellWidth(double width);

        double GetCellHeight();
        void SetCellHeight(double height);

        string GetWidthMode();
        void SetWidthMode(string mode);

        string GetHeightMode();
        void SetHeightMode(string mode);

        string GetHorizontalAlignment();
        void SetHorizontalAlignment(string alignmentString);

        string GetVerticalAlignment();
        void SetVerticalAlignment(string alignmentString);
    }
}
