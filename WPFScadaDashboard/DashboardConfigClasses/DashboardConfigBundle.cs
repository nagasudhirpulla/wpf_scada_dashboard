using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardConfigClasses
{
    class DashboardConfigBundle
    {
        public DashboardConfig DashboardConfig_ { get; set; }
        public List<IDashboardCellConfig> DashboardCellConfigs_ { get; set; }
    }
}
