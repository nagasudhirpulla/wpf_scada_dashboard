using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WPFScadaDashboard.DashboardConfigClasses;
using WPFScadaDashboard.Dialogs;

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

        public const string ConsoleMessageTypeStr = "console";
        public const string DeleteCellMessageTypeStr = "delete_cell";

        public string DashBoardFileName_ { get; set; } = null;
        ConsoleContent dc = new ConsoleContent();

        public AutoFetchConfig AutoFetchConfig_ = new AutoFetchConfig();

        private DispatcherTimer FetchTimer_;

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
            FetchTimer_ = new DispatcherTimer();
            UpdateFetcherInterval();
            FetchTimer_.Tick += Fetch_Timer_Tick;
        }

        public void AddLinesToConsole(string str) { dc.AddItemsToConsole(str); }

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
                // add deficit columns
                for (int i = 0; i < colDeficit; i++)
                {
                    CellsContainer.ColumnDefinitions.Add(GetNewColDefinition());
                }

            }
            else if (colDeficit < 0)
            {
                // delete excess columns
                for (int i = 0; i < -colDeficit; i++)
                {
                    CellsContainer.ColumnDefinitions.RemoveAt(0);
                }
            }
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
                linePlotCellUC.Changed += new EventHandler<EventArgs>(Changed);
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

        public void DeleteDashboardCellAt(int cellIndex)
        {
            if (cellIndex >= 0 && cellIndex < CellsContainer.Children.Count)
            {
                CellsContainer.Children.RemoveAt(cellIndex);
            }
            SyncRowColDefinitionsWithCells();
        }

        public void DeleteDashboardCell(UIElement cellUC)
        {
            CellsContainer.Children.Remove(cellUC);
            SyncRowColDefinitionsWithCells();
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

        private void Changed(object sender, EventArgs eArgs)
        {
            if (sender is ICellUC fc)
            {
                if (eArgs is DashBoardEventArgs e)
                {
                    if (e != null)
                    {
                        if (e.MessageType_ == ConsoleMessageTypeStr)
                        {
                            // Create a console entry
                            dc.AddItemsToConsole($"({e.SenderName_}) {e.MessageStr_}");
                        }
                        else if (e.MessageType_ == DeleteCellMessageTypeStr)
                        {
                            // get the filename
                            if (MessageBox.Show("Are you sure to delete this cell?", "Delete Cell", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                string cellName = fc.GetDashboardCellConfig().Name_;
                                // Delete the particular UI Element or Dashbaord Cell
                                DeleteDashboardCell((UIElement)fc);
                                dc.AddItemsToConsole($"{cellName} is deleted");
                            }
                        }
                    }
                }
                else if (eArgs is CellPosChangeReqArgs cellPosChangeArgs)
                {
                    if (cellPosChangeArgs != null)
                    {
                        CellPosChangeWindow cellPosChangeWindow = new CellPosChangeWindow(fc.GetDashboardCellConfig());
                        if (cellPosChangeWindow.ShowDialog() == true)
                        {
                            // change the cell position
                            fc.GetDashboardCellConfig().CellPosition_ = cellPosChangeWindow.posConfigVM.cellConfig.CellPosition_;
                            fc.GetDashboardCellConfig().CellHeight_ = cellPosChangeWindow.posConfigVM.cellConfig.CellHeight_;
                            fc.GetDashboardCellConfig().CellWidth_ = cellPosChangeWindow.posConfigVM.cellConfig.CellWidth_;
                            fc.GetDashboardCellConfig().HeightMode_ = cellPosChangeWindow.posConfigVM.cellConfig.HeightMode_;
                            fc.GetDashboardCellConfig().WidthMode_ = cellPosChangeWindow.posConfigVM.cellConfig.WidthMode_;
                            fc.GetDashboardCellConfig().MinHeight_ = cellPosChangeWindow.posConfigVM.cellConfig.MinHeight_;
                            fc.GetDashboardCellConfig().MinWidth_ = cellPosChangeWindow.posConfigVM.cellConfig.MinWidth_;
                            fc.GetDashboardCellConfig().HorizontalAlignment_ = cellPosChangeWindow.posConfigVM.cellConfig.HorizontalAlignment_;
                            fc.GetDashboardCellConfig().VerticalAlignment_ = cellPosChangeWindow.posConfigVM.cellConfig.VerticalAlignment_;
                            SyncRowColDefinitionsWithCells();
                            fc.UpdateCellPosition();
                        }
                    }
                }
            }
        }

        private void OpenCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OpenDashBoard();
        }

        private void OpenDashBoard()
        {
            // MessageBox.Show("You clicked 'Open...'");
            OpenFileDialog openFileDialog = new OpenFileDialog();
            // openFileDialog.Multiselect = true;
            openFileDialog.Filter = "dash files (*.dash)|*.dash|All files (*.*)|*.*";
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
                    SaveDashBoardAs();
                }
            }
        }

        private void SaveAsCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            // https://stackoverflow.com/questions/4682915/defining-menuitem-shortcuts
            SaveDashBoardAs();
        }

        private void SaveDashBoardAs()
        {
            string filename = DashBoardFileName_;
            if (filename == null)
            {
                filename = $"dashboard_template_{DateTime.Now.ToString("dd.MM.yyyy_HH.mm.ss")}.dash";
            }
            DashboardConfigBundle configBundle = GetDashBoardConfigBundle();
            string jsonText = JsonConvert.SerializeObject(configBundle, Formatting.Indented);
            SaveFileDialog savefileDialog = new SaveFileDialog
            {
                // set a default file name
                FileName = filename,
                // set filters - this can be done in properties as well
                Filter = "dash Files (*.dash)|*.dash|All files (*.*)|*.*"
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
            RefreshAllCells();
        }

        private void FetchStopBtn_Click(object sender, RoutedEventArgs e)
        {
            StopFetching();
            dc.AddItemsToConsole("Stopped Fetching");
        }

        private void StopFetching()
        {
            // stop the fetch timer if active
            if (FetchTimer_.IsEnabled)
            {
                FetchTimer_.Stop();
            }
        }

        public void UpdateFetcherInterval()
        {
            FetchTimer_.Interval = TimeSpan.FromSeconds(AutoFetchConfig_.TimePeriod_.HoursOffset_ * 60 * 60 + AutoFetchConfig_.TimePeriod_.MinsOffset_ * 60 + AutoFetchConfig_.TimePeriod_.SecsOffset_);
            // todo change this in app config also
        }

        private void AutoFetchStart_Click(object sender, RoutedEventArgs e)
        {
            StopFetching();
            FetchTimer_.Start();
        }

        private void RefreshAllCells()
        {
            // Fetch all cells data
            for (int i = 0; i < CellsContainer.Children.Count; i++)
            {
                // trigger the fetch data action in all cells
                //DashboardCellPosition cellPosition = ((ICellUC)(CellsContainer.Children[i])).GetDashboardCellConfig().CellPosition_;
                if (CellsContainer.Children[i] is ICellUC cellUC)
                {
                    cellUC.RefreshCell();
                }
            }
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AppSettingsWindow();
            if (dialog.ShowDialog() == true)
            {
                // do something
            }
        }

        public void AutoFetchConfigBtn_Click(object sender, RoutedEventArgs e)
        {
            FetchConfigWindow fetchConfigWindow = new FetchConfigWindow();
            fetchConfigWindow.ShowDialog();
            if (fetchConfigWindow.DialogResult == true)
            {
                AutoFetchConfig_ = fetchConfigWindow.fetchConfigVM.AutoFetchConfig;
                // change the fetcher timer interval
                UpdateFetcherInterval();
                dc.AddItemsToConsole("Auto Fetch config changes saved...");
            }
        }


        private void Fetch_Timer_Tick(object sender, EventArgs e)
        {
            RefreshAllCells();
        }

        private void AddTimeSeriesPlotCell_Click(object sender, RoutedEventArgs e)
        {
            LinePlotCellConfig linePlotCellConfig = new LinePlotCellConfig();
            AddDashBoardCell(linePlotCellConfig);
        }

        private void ClearConsole_Click(object sender, RoutedEventArgs e)
        {
            ClearConsole();
        }

        private void ClearConsole()
        {
            dc.ClearConsole();
        }

        private void ChangeConsoleHeight_Click(object sender, RoutedEventArgs e)
        {
            ConsoleConfigWindow consoleConfigWindow = new ConsoleConfigWindow(DashboardConfig_.ConsoleHeight_);
            if (consoleConfigWindow.ShowDialog() == true)
            {
                // change the cell position
                DashboardConfig_.ConsoleHeight_ = consoleConfigWindow.ConsoleConfigVM_.ConsoleHeight_;
                OnPropertyChanged("DashboardConfig_");
            }

        }

        private void Rename_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            // https://stackoverflow.com/questions/2796470/wpf-create-a-dialog-prompt
            var dialog = new SimpleStringInputDialog("Dashboard Title", "Enter the Dashboard Title", DashboardConfig_.DashboardName_);
            if (dialog.ShowDialog() == true)
            {
                DashboardConfig_.DashboardName_ = dialog.ResponseText;
                OnPropertyChanged("WindowTitleString");
                AddLinesToConsole($"DashBoard Title changed to {DashboardConfig_.DashboardName_}");
            }
        }
    }
}

/*
 We use css like grid system using Grid view
 Each view will express its height and width in terms of min width, min height, cell colspan, cell rowspan, cell alignment, cell absolute width/height if required
 */
