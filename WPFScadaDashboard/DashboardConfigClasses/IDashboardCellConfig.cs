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
        string Name_ { get; set; }

        // Type of DataViz like linePlot, MapPlot etc. This might be useful for deserialization and serialization
        string VizType_ { get; set; }

        List<IDataPoint> GetCellDataPoints();

        DashboardCellPosition CellPosition_ { get; set; }

        double CellWidth_ { get; set; }

        double CellHeight_ { get; set; }

        double MinWidth_ { get; set; }

        double MinHeight_ { get; set; }

        string WidthMode_ { get; set; }

        string HeightMode_ { get; set; }

        string HorizontalAlignment_ { get; set; }

        string VerticalAlignment_ { get; set; }

        string BackgroundColorString_ { get; set; }

        string ForegroundColorString_ { get; set; }
    }
}
