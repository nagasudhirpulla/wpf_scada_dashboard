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
using WPFScadaDashboard.DashboardUserControls;

namespace WPFScadaDashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DashboardUC DashboardUC_;
        public MainWindow()
        {
            InitializeComponent();
            // load a dashboard in the view space            
            DashboardConfig dashboardConfig = new DashboardConfig("Example Dashboard");
            DashboardUC_ = new DashboardUC(dashboardConfig);
            MainContainer.Content = DashboardUC_;
            this.Title = DashboardUC_.DashboardConfig_.DashboardName_;
            AddSeedCells();
        }

        public void AddSeedCells()
        {
            LinePlotCellConfig linePlotCellConfig = new LinePlotCellConfig
            {
                Name_ = "First Cell Name",
                WidthMode_ = LinePlotCellConfig.VariableWidthMode,
                Height_ = 300
            };
            LinePlotCellConfig linePlotCellConfig2 = new LinePlotCellConfig
            {
                Name_ = "Second Cell Name",
                CellPosition_ = new DashboardCellPosition(1, 0),
                WidthMode_ = LinePlotCellConfig.VariableWidthMode,
                Height_ = 150,
                VerticalAlignment_ = "Top"
            };
            LinePlotCellConfig linePlotCellConfig3 = new LinePlotCellConfig
            {
                Name_ = "Third Cell Name",
                CellPosition_ = new DashboardCellPosition(1, 1),
                WidthMode_ = LinePlotCellConfig.VariableWidthMode,
                Height_ = 250,
                VerticalAlignment_ = "Top"
            };
            DashboardUC_.AddDashBoardCells(linePlotCellConfig);
            DashboardUC_.AddDashBoardCells(linePlotCellConfig2);
            DashboardUC_.AddDashBoardCells(linePlotCellConfig3);
        }
    }
}
