using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using WPFScadaDashboard.DashboardConfigClasses;

namespace WPFScadaDashboard.DashboardUserControls
{
    /// <summary>
    /// Interaction logic for CellPosChangeWindow.xaml
    /// </summary>
    public partial class CellPosChangeWindow : Window
    {
        public PosConfigVM posConfigVM;
        public CellPosChangeWindow(IDashboardCellConfig cellConfig)
        {
            InitializeComponent();            
            posConfigVM = new PosConfigVM(cellConfig);
            CellPosChangeForm.DataContext = posConfigVM;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Update Cell Position ?", "Cell Position Configuration", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            else
            {
                DialogResult = true;
            }
        }
                
        private bool AreAllValidNumericChars(string str)
        {
            foreach (char c in str)
            {
                if (!Char.IsNumber(c)) return false;
            }

            return true;
        }

        // https://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !AreAllValidNumericChars(e.Text);
            base.OnPreviewTextInput(e);
        }
    }

    public class PosConfigVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string RowIndex { get { return cellConfig.CellPosition_.RowIndex_.ToString(); } set { cellConfig.CellPosition_.RowIndex_ = int.Parse(value); } }
        public string ColumnIndex { get { return cellConfig.CellPosition_.ColIndex_.ToString(); } set { cellConfig.CellPosition_.ColIndex_ = int.Parse(value); } }
        public string RowSpan { get { return cellConfig.CellPosition_.RowSpan_.ToString(); } set { cellConfig.CellPosition_.RowSpan_ = int.Parse(value); } }
        public string ColumnSpan { get { return cellConfig.CellPosition_.ColSpan_.ToString(); } set { cellConfig.CellPosition_.ColSpan_ = int.Parse(value); } }

        public IDashboardCellConfig cellConfig;

        public PosConfigVM(IDashboardCellConfig cellPosition)
        {
            this.cellConfig = cellPosition;
            NotifyPropertyChanged("RowIndex");
            NotifyPropertyChanged("ColumnIndex");
            NotifyPropertyChanged("RowSpan");
            NotifyPropertyChanged("ColumnSpan");
        }
    }
}
