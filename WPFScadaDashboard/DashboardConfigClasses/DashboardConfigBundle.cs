using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.JSONConverters;

namespace WPFScadaDashboard.DashboardConfigClasses
{
    class DashboardConfigBundle
    {
        public DashboardConfig DashboardConfig_ { get; set; }
        [JsonConverter(typeof(DashboardCellConfigConverter))]
        public List<IDashboardCellConfig> DashboardCellConfigs_ { get; set; }
    }
}
