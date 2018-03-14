using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DataPointClasses
{
    public class ScadaPointResult : PointResult
    {
        // Data Quality of the result
        public string DataQuality_ { get; set; }

        // Result TimeStamp
        public DateTime ResultTime_ { get; set; }

        // Result units
        public string Units_ { get; set; }

        public ScadaPointResult(double val, string DataQuality, DateTime ResultTime) : base(val)
        {
            DataQuality_ = DataQuality;
            ResultTime_ = ResultTime;
        }

        public ScadaPointResult(double val, string DataQuality, DateTime ResultTime, string Units) : base(val)
        {
            DataQuality_ = DataQuality;
            ResultTime_ = ResultTime;
            Units_ = Units;
        }
    }
}
