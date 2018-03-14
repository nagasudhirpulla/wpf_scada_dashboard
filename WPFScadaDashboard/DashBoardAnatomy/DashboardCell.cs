using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashBoardAnatomy
{
    public class DashboardCell
    {
        // Name of the Cell
        public string Name_ { get; set; } = "Cell_Name";

        // Position of the Cell
        public DashboardCellPosition CellPosition_ = new DashboardCellPosition();

        // Type of DataViz
        public virtual string VizType_ { get; private set; } = "Generic";

        // todo add list of data points for the cell to visualize

        // Constructor
        public DashboardCell()
        {
            
        }
    }
}
