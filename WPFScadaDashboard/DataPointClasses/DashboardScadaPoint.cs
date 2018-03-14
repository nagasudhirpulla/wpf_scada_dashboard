using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DataPointClasses
{
    public class DashboardScadaPoint : DashBoardDataPoint
    {
        // Extended Id of the point
        public string ExtendedId_ { get; set; } = null;

        // override the PointType_ to Scada
        public override string PointType_ { get; } = "Scada";

        public DashboardScadaPoint(string Id, string Name, string ExtendedId) : base(Id, Name)
        {
            ExtendedId_ = ExtendedId;
        }

        public DashboardScadaPoint(string Id) : base(Id)
        {

        }

        public DashboardScadaPoint(string Id, string Name) : base(Id, Name)
        {

        }
    }
}
