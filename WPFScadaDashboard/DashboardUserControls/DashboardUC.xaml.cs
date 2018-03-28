using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPFScadaDashboard.DashboardConfigClasses;

namespace WPFScadaDashboard.DashboardUserControls
{
    /// <summary>
    /// Interaction logic for DashboardUC.xaml
    /// </summary>
    public partial class DashboardUC : UserControl, INotifyPropertyChanged
    {
        // Declare the event
        public event PropertyChangedEventHandler PropertyChanged;
        // Create the OnPropertyChanged method to raise the event
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public static string ConsoleMessageTypeStr = "console";

        public string DashBoardFileName_ { get; set; } = null;
        ConsoleContent dc = new ConsoleContent();

        private DashboardConfig dashboardConfig = new DashboardConfig();
        public DashboardConfig DashboardConfig_
        {
            get { return dashboardConfig; }
            set
            {
                dashboardConfig = value;
                OnPropertyChanged("DashboardConfig_");
            }
        }

        private void DoInitialWireUp()
        {
            InitializeComponent();
            consoleItems.ItemsSource = dc.ConsoleOutput;
            dc.AddItemsToConsole("Hello User!");
            DataContext = this;
        }

        public DashboardUC()
        {
            DoInitialWireUp();
        }

        public DashboardUC(DashboardConfig dashboardConfig)
        {
            DashboardConfig_ = dashboardConfig;
            DoInitialWireUp();
        }

        private RowDefinition GetNewRowDefinition()
        {
            RowDefinition rowDef = new RowDefinition();
            // Add default Row Definition settings here if desired
            return rowDef;
        }

        private ColumnDefinition GetNewColDefinition()
        {
            ColumnDefinition colDef = new ColumnDefinition();
            // Add default Row Definition settings here if desired
            return colDef;
        }

        // not used but kept as utility function
        public DashboardCellPosition GetMaxRowColumnIndex()
        {
            DashboardCellPosition maxIndexPos = new DashboardCellPosition(0, 0);
            for (int i = 0; i < CellsContainer.Children.Count; i++)
            {
                DashboardCellPosition cellPosition = ((ICellUC)(CellsContainer.Children[i])).GetDashboardCellConfig().CellPosition_;
                if (cellPosition.ColIndex_ > maxIndexPos.ColIndex_)
                {
                    maxIndexPos.ColIndex_ = cellPosition.ColIndex_;
                }
                if (cellPosition.RowIndex_ > maxIndexPos.RowIndex_)
                {
                    maxIndexPos.RowIndex_ = cellPosition.RowIndex_;
                }
            }
            return maxIndexPos;
        }

        public void SyncRowColDefinitionsWithCells()
        {
            List<DashboardCellPosition> cellPositions = new List<DashboardCellPosition>();

            // iterate through each cell in the cells container
            for (int cell_iter = 0; cell_iter < CellsContainer.Children.Count; cell_iter++)
            {
                ICellUC cellUC = (ICellUC)CellsContainer.Children[cell_iter];
                IDashboardCellConfig dashboardCellConfig = cellUC.GetDashboardCellConfig();
                DashboardCellPosition cellPosition = dashboardCellConfig.CellPosition_;
                // manipulate cell position to avoid cell position conflicts by pushing the cell position down by one row
                if (cellPositions.Exists(x => x.RowIndex_ == cellPosition.RowIndex_ && x.ColIndex_ == cellPosition.ColIndex_))
                {
                    // get the maximum row Index
                    int maxRowIndex = (from pos in cellPositions select pos.RowIndex_).Max();
                    //modify the rowIndex to avoid duplicates
                    cellPosition.RowIndex_ = maxRowIndex + 1;
                    cellUC.GetDashboardCellConfig().CellPosition_ = cellPosition;
                }
                cellPositions.Add(cellPosition);
            }

            int maxRows = 0;
            int maxCols = 0;
            if (cellPositions.Count > 0)
            {
                maxRows = (from pos in cellPositions select pos.RowIndex_).Max() + 1;
                maxCols = (from pos in cellPositions select pos.ColIndex_).Max() + 1;
            }

            // Add adequate number of row and column definitions
            // Find rows deficit
            int rowDeficit = maxRows - CellsContainer.RowDefinitions.Count();
            int colDeficit = maxCols - CellsContainer.ColumnDefinitions.Count();
            // Do addition or deletion of row definitions for Row Grids
            if (rowDeficit > 0)
            {
                // add deficit rows
                for (int i = 0; i < rowDeficit; i++)
                {
                    CellsContainer.RowDefinitions.Add(GetNewRowDefinition());
                }

            }
            else if (rowDeficit < 0)
            {
                // delete excess rows
                for (int i = 0; i < -rowDeficit; i++)
                {
                    CellsContainer.RowDefinitions.RemoveAt(0);
                }
            }

            if (colDeficit > 0)
            {
                // add deficit rows
                for (int i = 0; i < colDeficit; i++)
                {
                    CellsContainer.ColumnDefinitions.Add(GetNewColDefinition());
                }

            }
            else if (colDeficit < 0)
            {
                // delete excess rows
                for (int i = 0; i < -colDeficit; i++)
                {
                    CellsContainer.ColumnDefinitions.RemoveAt(0);
                }
            }
            // todo do addition or deletion of col definitions
        }

        public void AddDashBoardCell(IDashboardCellConfig dashboardCellConfig)
        {
            if (dashboardCellConfig == null)
            {
                return;
            }
            if (dashboardCellConfig.GetType().FullName == typeof(LinePlotCellConfig).FullName)
            {
                // Add a line plot User Control to the Cell Container
                LinePlotCellUC linePlotCellUC = new LinePlotCellUC((LinePlotCellConfig)dashboardCellConfig);
                linePlotCellUC.Changed += new EventHandler<DashBoardEventArgs>(Changed);
                CellsContainer.Children.Add(linePlotCellUC);
            }
            SyncRowColDefinitionsWithCells();
        }

        public void AddDashBoardCells(List<IDashboardCellConfig> dashboardCellConfigs)
        {
            for (int i = 0; i < dashboardCellConfigs.Count; i++)
            {
                AddDashBoardCell(dashboardCellConfigs.ElementAt(i));
            }
        }

        public void ClearDashboardCells()
        {
            CellsContainer.Children.Clear();
            SyncRowColDefinitionsWithCells();
        }

        public List<IDashboardCellConfig> GetDashboardCellConfigs()
        {
            List<IDashboardCellConfig> dashboardCellConfigs = new List<IDashboardCellConfig>();
            for (int i = 0; i < CellsContainer.Children.Count; i++)
            {
                dashboardCellConfigs.Add(((ICellUC)CellsContainer.Children[i]).GetDashboardCellConfig());
            }
            return dashboardCellConfigs;
        }

        private void Changed(object sender, DashBoardEventArgs e)
        {
            if (sender is ICellUC fc)
            {
                if (e != null)
                {
                    if (e.MessageType_ == ConsoleMessageTypeStr)
                    {
                        // Create a console entry
                        dc.AddItemsToConsole($"({e.SenderName_}) {e.MessageStr_}");
                    }
                }
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            // MessageBox.Show("You clicked 'Open...'");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // openFileDialog.Multiselect = true;
            openFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer);
            if (openFileDialog.ShowDialog() == true)
            {
                string filename = openFileDialog.FileNames[0];
                OpenFileName(filename);
                if (filename != null)
                {
                    DashBoardFileName_ = filename;
                }
            }
        }

        public void OpenFileName(string str)
        {
            if (str != null)
            {
                DashboardConfigBundle configBundle = JsonConvert.DeserializeObject<DashboardConfigBundle>(File.ReadAllText(str));
                //dc.AddItemsToConsole(JsonConvert.SerializeObject(configBundle, Formatting.Indented));
                DashboardConfig_ = configBundle.DashboardConfig_;
                List<IDashboardCellConfig> dashboardCellConfigs = configBundle.DashboardCellConfigs_;
                ClearDashboardCells();
                AddDashBoardCells(dashboardCellConfigs);
                dc.AddItemsToConsole($"Dashboard \"{DashboardConfig_.DashboardName_}\" loaded");
            }
        }

        private DashboardConfigBundle GetDashBoardConfigBundle()
        {
            DashboardConfigBundle configBundle = new DashboardConfigBundle() { DashboardConfig_ = DashboardConfig_, DashboardCellConfigs_ = GetDashboardCellConfigs() };
            return configBundle;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            // get the filename
            if (MessageBox.Show("Save this Dashboard?", "Save", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                string filename = DashBoardFileName_;
                if (filename != null)
                {
                    DashboardConfigBundle configBundle = GetDashBoardConfigBundle();
                    JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { DefaultValueHandling = DefaultValueHandling.Ignore };
                    string jsonText = JsonConvert.SerializeObject(configBundle, Formatting.Indented, jsonSerializerSettings);
                    File.WriteAllText(filename, jsonText);
                    dc.AddItemsToConsole("Saved the updated Dashboard!!!");
                }
                else
                {
                    // open save as window
                    SaveAs_Click(this, null);
                }
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            string filename = DashBoardFileName_;
            if (filename == null)
            {
                filename = String.Format("dashboard_template_{0}.json", DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss"));
            }
            DashboardConfigBundle configBundle = GetDashBoardConfigBundle();
            string jsonText = JsonConvert.SerializeObject(configBundle, Formatting.Indented);
            SaveFileDialog savefileDialog = new SaveFileDialog
            {
                // set a default file name
                FileName = filename,
                // set filters - this can be done in properties as well
                Filter = "JSON Files (*.json)|*.json|All files (*.*)|*.*"
            };
            if (savefileDialog.ShowDialog() == true)
            {
                File.WriteAllText(savefileDialog.FileName, jsonText);
                dc.AddItemsToConsole("Saved the updated template file!!!");
                if (savefileDialog.FileName != null)
                {
                    DashBoardFileName_ = savefileDialog.FileName;
                }
            }
        }

        private void NewWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void About_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FetchBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

/*
 We use css like grid system using Grid view
 Each view will express its height and width in terms of min width, min height, cell colspan, cell rowspan, cell alignment, cell absolute width/height if required
 */
