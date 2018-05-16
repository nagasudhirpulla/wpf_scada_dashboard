using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFScadaDashboard.DashboardConfigClasses
{
    public class DashboardCellPosition
    {
        public int ColIndex_ { get; set; }
        public int RowIndex_ { get; set; }
        public int ColSpan_ { get; set; } = 1;
        public int RowSpan_ { get; set; } = 1;

        public DashboardCellPosition()
        {            
            RowIndex_ = 0;
            ColIndex_ = 0;
        }

        public DashboardCellPosition(int RowIndex, int ColIndex)
        {            
            RowIndex_ = RowIndex;
            ColIndex_ = ColIndex;
        }

        public DashboardCellPosition(int RowIndex, int ColIndex, int RowSpan, int ColSpan)
        {
            RowIndex_ = RowIndex;
            ColIndex_ = ColIndex;
            RowSpan_ = RowSpan;
            ColSpan_ = ColSpan;
        }

        public DashboardCellPosition(DashboardCellPosition cellPosition)
        {
            RowIndex_ = cellPosition.RowIndex_;
            ColIndex_ = cellPosition.ColIndex_;
            ColSpan_ = cellPosition.ColSpan_;
            RowSpan_ = cellPosition.RowSpan_;
        }
    }
}
