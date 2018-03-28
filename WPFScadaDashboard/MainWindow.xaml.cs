using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using WPFScadaDashboard.DashboardConfigClasses;
using WPFScadaDashboard.DashboardDataPointClasses;
using WPFScadaDashboard.DashboardUserControls;

namespace WPFScadaDashboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        DashboardUC DashboardUC_;
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            // load a dashboard in the view space            
            DashboardConfig dashboardConfig = new DashboardConfig("Dashboard");
            DashboardUC_ = new DashboardUC(dashboardConfig);
            MainContainer.Content = DashboardUC_;
            //this.Title = DashboardUC_.DashboardConfig_.DashboardName_;
            DashboardUC_.PropertyChanged += DashboardUC__PropertyChanged;
            //AddSeedCells();
            String fileNameStr = (String)((App)Application.Current).Properties["FilePathArgName"];
            DashboardUC_.OpenFileName(fileNameStr);
        }

        private void DashboardUC__PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("DashboardUC_");
            OnPropertyChanged("WindowTitleString");
        }

        public string WindowTitleString { get { return DashboardUC_.DashboardConfig_.DashboardName_; } }

        public void AddSeedCells()
        {
            LinePlotCellConfig linePlotCellConfig = new LinePlotCellConfig
            {
                Name_ = "First Cell Name",
                CellPosition_ = new DashboardCellPosition(0, 0, 1, 2),
                TimeSeriesPoints_ = new List<IDashboardTimeSeriesPoint> { new DashboardScadaTimeSeriesPoint(new ScadaDataPoint("123"), DateTime.Now.AddHours(-10), DateTime.Now) }
            };
            LinePlotCellConfig linePlotCellConfig2 = new LinePlotCellConfig
            {
                Name_ = "Second Cell Name",
                CellPosition_ = new DashboardCellPosition(1, 0)
            };
            LinePlotCellConfig linePlotCellConfig3 = new LinePlotCellConfig
            {
                Name_ = "Third Cell Name",
                CellPosition_ = new DashboardCellPosition(1, 1)
            };
            DashboardUC_.AddDashBoardCell(linePlotCellConfig);
            DashboardUC_.AddDashBoardCell(linePlotCellConfig2);
            DashboardUC_.AddDashBoardCell(linePlotCellConfig3);
        }
    }
}
