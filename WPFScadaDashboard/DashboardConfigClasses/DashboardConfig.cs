using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardConfigClasses
{
    public class DashboardConfig
    {
        // Name of the dashboard
        public string DashboardName_ { get; set; } = "Dashboard_Name";

        public DashboardConfig(string dashboardName_)
        {
            DashboardName_ = dashboardName_;
        }

        public DashboardConfig()
        {
        }
    }
}
