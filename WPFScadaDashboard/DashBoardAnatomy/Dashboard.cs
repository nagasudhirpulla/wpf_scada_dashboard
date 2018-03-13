using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashBoardAnatomy
{
    class Dashboard
    {
        // Name of the dashboard
        public string DashboardName_ { get; set; } = "Dashboard_Name";
        // List of cells
        public List<DashboardCell> DashboardCells_ { get; set; } = new List<DashboardCell>();
    }
}
