using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashBoardAnatomy
{
    class DashboardCell
    {
        // Name of the Cell
        public string Name_ { get; set; } = "Cell_Name";

        // Position of the Cell
        public DashboardCellPosition CellPosition_ = new DashboardCellPosition();

        // Type of DataViz
        private string VizType_;

        public string GetVizType()
        {
            return VizType_;
        }

        // todo add list of data points for the cell to visualize

        // Constructor
        public DashboardCell()
        {
            VizType_ = "Generic";
        }
    }
}
