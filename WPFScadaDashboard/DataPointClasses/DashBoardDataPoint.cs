using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DataPointClasses
{
    public class DashBoardDataPoint
    {
        // This is a generic dashboard DataPoint Class which other specific data point types are going to implement
        public string Name_ { get; set; }
        public string Id_ { get; set; }

        // Specifies the point type for serialization purposes
        public virtual string PointType_ { get; } = "Generic";

        // Constructor. We are not allowing to have null Id
        public DashBoardDataPoint(string Id)
        {
            Id_ = Id;
            // Keeping name = Id since we dont have a name supplied
            Name_ = Id_;
        }

        // Constructor. We are not allowing to have null Id and Name Attributes
        public DashBoardDataPoint(string Id, string Name)
        {
            Name_ = Name;
            Id_ = Id;
        }
    }
}
