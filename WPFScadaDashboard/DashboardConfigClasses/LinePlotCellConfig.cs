using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DashboardDataPointClasses;

namespace WPFScadaDashboard.DashboardConfigClasses
{
    class LinePlotCellConfig : IDashboardCellConfig
    {
        public const string CellType_ = "Time_Series_Line_Plot";

        // Overriding default Name
        public string Name_ { get; set; } = "Line Plot Cell";

        // Cell Position
        public DashboardCellPosition CellPosition_ { get; set; } = new DashboardCellPosition();

        // Data Points of the plots that are used for fetching data for line plots
        public List<IDashboardTimeSeriesPoint> TimeSeriesPoints_ { get; set; } = new List<IDashboardTimeSeriesPoint>();

        // constructor
        public LinePlotCellConfig()
        {
        }

        // Implementing Interface
        public string GetName()
        {
            return Name_;
        }

        // Implementing Interface
        public DashboardCellPosition GetCellPosition()
        {
            return CellPosition_;
        }

        // Implementing Interface
        public string GetVizType()
        {
            return CellType_;
        }

        // Implementing Interface
        public List<IDataPoint> GetCellDataPoints()
        {
            throw new NotImplementedException();
        }
    }
}
