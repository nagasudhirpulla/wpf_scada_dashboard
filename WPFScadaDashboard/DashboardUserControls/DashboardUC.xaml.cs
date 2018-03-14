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
using WPFScadaDashboard.DashBoardAnatomy;

namespace WPFScadaDashboard.DashboardUserControls
{
    /// <summary>
    /// Interaction logic for DashboardUC.xaml
    /// </summary>
    public partial class DashboardUC : UserControl
    {
        ConsoleContent dc = new ConsoleContent();
        public DashboardUC()
        {
            InitializeComponent();
            consoleItems.ItemsSource = dc.ConsoleOutput;
            dc.AddItemsToConsole("Hello User!");            
            DataContext = this;
        }

        // List of cells
        public List<DashboardCell> DashboardCells_ { get; private set; } = new List<DashboardCell>();

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

        public DashboardCellPosition GetMaxRowColumnIndex()
        {
            DashboardCellPosition maxIndexPos = new DashboardCellPosition(0, 0);
            for (int i = 0; i < DashboardCells_.Count; i++)
            {
                if (DashboardCells_[i].CellPosition_.ColIndex_ > maxIndexPos.ColIndex_)
                {
                    maxIndexPos.ColIndex_ = DashboardCells_[i].CellPosition_.ColIndex_;
                }
                if (DashboardCells_[i].CellPosition_.RowIndex_ > maxIndexPos.RowIndex_)
                {
                    maxIndexPos.RowIndex_ = DashboardCells_[i].CellPosition_.RowIndex_;
                }
            }
            return maxIndexPos;
        }

        public void SyncRowColDefinitionsWithCells()
        {
            // Add or delete the row and column definitions

            // Get the present Row Column definitions count
            int presentRowCount = CellsContainer.RowDefinitions.Count;
            int presentColCount = CellsContainer.ColumnDefinitions.Count;

            // get the required Max Row Columns Count
            DashboardCellPosition maxRowColIndex = GetMaxRowColumnIndex();
            if (maxRowColIndex.RowIndex_ + 1 != presentRowCount)
            {
                if (presentRowCount > maxRowColIndex.RowIndex_ + 1)
                {
                    // remove excess row definitions
                    int numRowDefsToRemove = presentRowCount - maxRowColIndex.RowIndex_ - 1;
                    for (int i = 0; i < numRowDefsToRemove; i++)
                    {
                        CellsContainer.RowDefinitions.RemoveAt(0);
                    }
                }
                else
                {
                    // add new row definitions if short
                    int numRowDefsToAdd = maxRowColIndex.RowIndex_ + 1 - presentRowCount;
                    for (int i = 0; i < numRowDefsToAdd; i++)
                    {
                        CellsContainer.RowDefinitions.Add(GetNewRowDefinition());
                    }
                }
            }

            if (maxRowColIndex.ColIndex_ + 1 != presentColCount)
            {
                if (presentColCount > maxRowColIndex.ColIndex_ + 1)
                {
                    // remove excess column definitions
                    int numColDefsToRemove = presentColCount - maxRowColIndex.ColIndex_ - 1;
                    for (int i = 0; i < numColDefsToRemove; i++)
                    {
                        CellsContainer.ColumnDefinitions.RemoveAt(0);
                    }
                }
                else
                {
                    // add new column definitions if short
                    int numColDefsToAdd = maxRowColIndex.ColIndex_ + 1 - presentColCount;
                    for (int i = 0; i < numColDefsToAdd; i++)
                    {
                        CellsContainer.ColumnDefinitions.Add(GetNewColDefinition());
                    }
                }
            }
        }

        public void SetDashBoardCells(List<DashboardCell> cells)
        {
            DashboardCells_ = cells;
            SyncRowColDefinitionsWithCells();
        }
    }
}
