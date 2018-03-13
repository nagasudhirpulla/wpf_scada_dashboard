using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashBoardAnatomy
{
    class DashboardCellPosition
    {
        public int XIndex_ { get; set; }
        public int YIndex_ { get; set; }

        public DashboardCellPosition()
        {
            XIndex_ = 0;
            YIndex_ = 0;
        }

        public DashboardCellPosition(int xIndex, int yIndex)
        {
            XIndex_ = xIndex;
            YIndex_ = yIndex;
        }
    }
}
