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
    /// Interaction logic for DashboardUC.xaml
    /// </summary>
    public partial class DashboardUC : UserControl
    {
        public static string ConsoleMessageTypeStr = "console";

        ConsoleContent dc = new ConsoleContent();
        public DashboardConfig DashboardConfig_ { get; set; } = new DashboardConfig();

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
                DashboardCellPosition cellPosition = ((ICellUC)(CellsContainer.Children[i])).GetDashboardCellConfig().GetCellPosition();
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
                DashboardCellPosition cellPosition = dashboardCellConfig.GetCellPosition();
                // manipulate cell position to avoid cell position conflicts by pushing the cell position down by one row
                if (cellPositions.Exists(x => x.RowIndex_ == cellPosition.RowIndex_ && x.ColIndex_ == cellPosition.ColIndex_))
                {
                    // get the maximum row Index
                    int maxRowIndex = (from pos in cellPositions select pos.RowIndex_).Max();
                    //modify the rowIndex to avoid duplicates
                    cellPosition.RowIndex_ = maxRowIndex + 1;
                    cellUC.GetDashboardCellConfig().SetCellPosition(cellPosition);
                }
                cellPositions.Add(cellPosition);
            }

            int maxRows = (from pos in cellPositions select pos.RowIndex_).Max() + 1;
            int maxCols = (from pos in cellPositions select pos.ColIndex_).Max() + 1;

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

        public void AddDashBoardCells(IDashboardCellConfig dashboardCellConfig)
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

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {

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
