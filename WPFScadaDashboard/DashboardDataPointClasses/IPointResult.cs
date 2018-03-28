using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardDataPointClasses
{
    public interface IPointResult
    {
        double Val_ { get; set; }
        string ResultType_ { get; set; }
    }
}
