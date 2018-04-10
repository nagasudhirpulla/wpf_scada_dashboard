using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DashboardConfigClasses;

namespace WPFScadaDashboard.DashboardUserControls
{
    interface ICellUC
    {
        IDashboardCellConfig GetDashboardCellConfig();
        void UpdateCellPosition();
        void DeleteCell();
    }
}
