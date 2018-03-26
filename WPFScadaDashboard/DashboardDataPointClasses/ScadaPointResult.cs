using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardDataPointClasses
{
    public class ScadaPointResult : IPointResult
    {
        public double Val_ { get; set; }

        // Data Quality of the result
        public string DataQuality_ { get; set; }

        // Result TimeStamp
        public DateTime ResultTime_ { get; set; }

        // Result units
        public string Units_ { get; set; }

        public ScadaPointResult(double val, string DataQuality, DateTime ResultTime)
        {
            Val_ = val;
            DataQuality_ = DataQuality;
            ResultTime_ = ResultTime;
        }

        public ScadaPointResult(double val, string DataQuality, DateTime ResultTime, string Units)
        {
            Val_ = val;
            DataQuality_ = DataQuality;
            ResultTime_ = ResultTime;
            Units_ = Units;
        }        
    }
}
