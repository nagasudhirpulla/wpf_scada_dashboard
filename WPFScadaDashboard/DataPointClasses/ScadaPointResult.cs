using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DataPointClasses
{
    class ScadaPointResult : PointResult
    {
        // Data Quality of the result
        public string DataQuality_ { get; set; }

        // Result TimeStamp
        public DateTime ResultTime_ { get; set; }

        public ScadaPointResult(double val, string DataQuality, DateTime ResultTime) : base(val)
        {
            DataQuality_ = DataQuality;
            ResultTime_ = ResultTime;
        }
    }
}
