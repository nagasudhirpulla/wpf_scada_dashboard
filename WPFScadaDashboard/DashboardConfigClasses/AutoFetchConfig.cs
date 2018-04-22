using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DashboardDataPointClasses;

namespace WPFScadaDashboard.DashboardConfigClasses
{
    public class AutoFetchConfig
    {
        public VariableTime TimePeriod_ { get; set; }

        public AutoFetchConfig()
        {
            TimePeriod_ = new VariableTime(0, 0, 5);
        }

        public AutoFetchConfig(VariableTime timePeriod_)
        {
            TimePeriod_ = timePeriod_;
        }
    }
}
