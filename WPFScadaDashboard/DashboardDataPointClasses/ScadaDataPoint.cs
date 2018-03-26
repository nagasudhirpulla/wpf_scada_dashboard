using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardDataPointClasses
{
    public class ScadaDataPoint : IDataPoint
    {
        public string Id_ { get; set; }

        public string Name_ { get; set; }

        // Extended Id of the point
        public string ExtendedId_ { get; set; } = null;

        public string PointType_ { get; set; } = "Scada";

        public ScadaDataPoint(string Id)
        {
            Id_ = Id;
            Name_ = Id;
        }

        public ScadaDataPoint(string Id, string Name)
        {
            Id_ = Id;
            Name_ = Name;
        }

        public ScadaDataPoint(string Id, string Name, string ExtendedId)
        {
            Id_ = Id;
            Name_ = Name;
            ExtendedId_ = ExtendedId;
        }

        // Implementing Interface
        public string GetPointType()
        {
            return PointType_;
        }
    }
}
