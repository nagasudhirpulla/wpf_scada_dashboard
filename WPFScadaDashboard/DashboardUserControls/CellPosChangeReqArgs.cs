using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DashboardConfigClasses;

namespace WPFScadaDashboard.DashboardUserControls
{
    public class CellPosChangeReqArgs : EventArgs
    {
        // currently no properties required
        public DashboardCellPosition DashboardCellPosition_ { get; set; }

        public CellPosChangeReqArgs(DashboardCellPosition dashboardCellPosition_)
        {
            DashboardCellPosition_ = dashboardCellPosition_;
        }

        public CellPosChangeReqArgs()
        {
        }
    }
}
