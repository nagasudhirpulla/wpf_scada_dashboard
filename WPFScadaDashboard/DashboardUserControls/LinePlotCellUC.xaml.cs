using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFScadaDashboard.DashboardConfigClasses;

namespace WPFScadaDashboard.DashboardUserControls
{
    /// <summary>
    /// Interaction logic for LinePlotCellUC.xaml
    /// </summary>
    public partial class LinePlotCellUC : UserControl, ICellUC
    {
        public LinePlotCellConfig LinePlotCellConfig_;

        private void DoInitialWireUp()
        {
            InitializeComponent();
            DataContext = this;
        }

        // constructor
        public LinePlotCellUC()
        {
            DoInitialWireUp();
        }

        // constructor
        public LinePlotCellUC(LinePlotCellConfig linePlotCellConfig)
        {
            LinePlotCellConfig_ = linePlotCellConfig;
            DoInitialWireUp();
        }

        // Implementing Interface
        public IDashboardCellConfig GetDashboardCellConfig()
        {
            return LinePlotCellConfig_;
        }

        public string CellTitle { get { return LinePlotCellConfig_.Name_; } set { LinePlotCellConfig_.Name_ = value; } }

        public int RowIndex { get { return LinePlotCellConfig_.CellPosition_.RowIndex_; } }

        public int ColumnIndex { get { return LinePlotCellConfig_.CellPosition_.ColIndex_; } }

        public string CellHeight
        {
            get
            {
                string starChar = "";
                if (LinePlotCellConfig_.GetHeightMode() == LinePlotCellConfig.VariableHeightMode)
                {
                    starChar = "*";
                }
                return LinePlotCellConfig_.Height_.ToString() + starChar;
            }
        }

        public string CellWidth
        {
            get
            {
                string starChar = "";
                if (LinePlotCellConfig_.GetWidthMode() == LinePlotCellConfig.VariableWidthMode)
                {
                    starChar = "*";
                }
                return LinePlotCellConfig_.Width_.ToString() + starChar;
            }
        }

        public double CellMinHeight { get { return LinePlotCellConfig_.MinHeight_; } set { LinePlotCellConfig_.MinHeight_ = value; } }

        public double CellMinWidth { get { return LinePlotCellConfig_.MinWidth_; } set { LinePlotCellConfig_.MinWidth_ = value; } }
    }
}
