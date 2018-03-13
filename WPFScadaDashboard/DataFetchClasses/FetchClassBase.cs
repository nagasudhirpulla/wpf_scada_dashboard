using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFScadaDashboard.DataPointClasses;

namespace WPFScadaDashboard.DataFetchClasses
{
    abstract class FetchClassBase
    {
        public abstract List<double> FetchPointData(DashBoardDataPoint point);
        
    }
}
