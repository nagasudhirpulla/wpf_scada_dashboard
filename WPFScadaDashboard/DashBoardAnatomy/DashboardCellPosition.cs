using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashBoardAnatomy
{
    public class DashboardCellPosition
    {
        public int ColIndex_ { get; set; }
        public int RowIndex_ { get; set; }

        public DashboardCellPosition()
        {
            ColIndex_ = 0;
            RowIndex_ = 0;
        }

        public DashboardCellPosition(int ColIndex, int RowIndex)
        {
            ColIndex_ = ColIndex;
            RowIndex_ = RowIndex;
        }
    }
}
