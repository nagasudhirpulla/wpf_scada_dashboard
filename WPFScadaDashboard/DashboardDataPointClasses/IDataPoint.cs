using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardDataPointClasses
{
    public interface IDataPoint
    {
        // This is a generic dashboard DataPoint Class which other specific data point types are going to implement

        string Name_ { get; set; }
        string Id_ { get; set; }
        
        // Specifies the point type for serialization purposes
        string GetPointType();
    }
}
