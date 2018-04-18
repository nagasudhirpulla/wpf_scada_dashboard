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

        public IDashboardCellConfig cellConfig;

        public PosConfigVM(IDashboardCellConfig cellPosition)
        {
            this.cellConfig = cellPosition;
            NotifyPropertyChanged("RowIndex");
            NotifyPropertyChanged("ColumnIndex");
            NotifyPropertyChanged("RowSpan");
            NotifyPropertyChanged("ColumnSpan");
            NotifyPropertyChanged("CellWidthMode");
            NotifyPropertyChanged("CellHeightMode");
            NotifyPropertyChanged("CellWidth");
            NotifyPropertyChanged("CellHeight");
            NotifyPropertyChanged("CellMinWidth");
            NotifyPropertyChanged("CellMinHeight");
            NotifyPropertyChanged("CellHorAlignMode");
            NotifyPropertyChanged("CellVertAlignMode");
        }

        public List<string> WidthModes { get; set; } = new List<string> { LinePlotCellConfig.AbsoluteWidthMode, LinePlotCellConfig.VariableWidthMode };
        public List<string> HeightModes { get; set; } = new List<string> { LinePlotCellConfig.AbsoluteHeightMode, LinePlotCellConfig.VariableHeightMode };

        public List<string> VertAlignModes { get; set; } = new List<string> { LinePlotCellConfig.AlignmentModeStretch, LinePlotCellConfig.AlignmentModeTop, LinePlotCellConfig.AlignmentModeBottom, LinePlotCellConfig.AlignmentModeCenter };
        public List<string> HorAlignModes { get; set; } = new List<string> { LinePlotCellConfig.AlignmentModeStretch, LinePlotCellConfig.AlignmentModeLeft, LinePlotCellConfig.AlignmentModeRight, LinePlotCellConfig.AlignmentModeCenter };

        public int CellWidthMode { get { return WidthModes.IndexOf(cellConfig.WidthMode_); } set { cellConfig.WidthMode_ = WidthModes.ElementAt(value); NotifyPropertyChanged("CellWidthModeStr"); } }
        public string CellWidthModeStr
        {
            get { return cellConfig.WidthMode_; }
            set
            {
                int modeInt = WidthModes.IndexOf(value);
                if (modeInt != -1)
                {
                    cellConfig.WidthMode_ = value;
                }
            }
        }

        public int CellHeightMode { get { return HeightModes.IndexOf(cellConfig.HeightMode_); } set { cellConfig.HeightMode_ = HeightModes.ElementAt(value); NotifyPropertyChanged("CellHeightModeStr"); } }
        public string CellHeightModeStr
        {
            get { return cellConfig.HeightMode_; }
            set
            {
                int modeInt = HeightModes.IndexOf(value);
                if (modeInt != -1)
                {
                    cellConfig.HeightMode_ = value;
                }
            }
        }

        public int CellHorAlignMode { get { return HorAlignModes.IndexOf(cellConfig.HorizontalAlignment_); } set { cellConfig.HorizontalAlignment_ = HorAlignModes.ElementAt(value); NotifyPropertyChanged("CellHorAlignModeStr"); } }
        public string CellHorAlignModeStr
        {
            get { return cellConfig.WidthMode_; }
            set
            {
                int modeInt = HorAlignModes.IndexOf(value);
                if (modeInt != -1)
                {
                    cellConfig.HorizontalAlignment_ = value;
                }
            }
        }

        public int CellVertAlignMode { get { return VertAlignModes.IndexOf(cellConfig.VerticalAlignment_); } set { cellConfig.VerticalAlignment_ = VertAlignModes.ElementAt(value); NotifyPropertyChanged("CellVertAlignModeStr"); } }
        public string CellVertAlignModeStr
        {
            get { return cellConfig.VerticalAlignment_; }
            set
            {
                int modeInt = VertAlignModes.IndexOf(value);
                if (modeInt != -1)
                {
                    cellConfig.VerticalAlignment_ = value;
                }
            }
        }

        public string CellWidth { get { return cellConfig.CellWidth_.ToString(); } set { cellConfig.CellWidth_ = int.Parse(value); } }
        public string CellHeight { get { return cellConfig.CellHeight_.ToString(); } set { cellConfig.CellHeight_ = int.Parse(value); } }
        public string CellMinWidth { get { return cellConfig.MinWidth_.ToString(); } set { cellConfig.MinWidth_ = int.Parse(value); } }
        public string CellMinHeight { get { return cellConfig.MinHeight_.ToString(); } set { cellConfig.MinHeight_ = int.Parse(value); } }
        public string RowIndex { get { return cellConfig.CellPosition_.RowIndex_.ToString(); } set { cellConfig.CellPosition_.RowIndex_ = int.Parse(value); } }
        public string ColumnIndex { get { return cellConfig.CellPosition_.ColIndex_.ToString(); } set { cellConfig.CellPosition_.ColIndex_ = int.Parse(value); } }
        public string RowSpan { get { return cellConfig.CellPosition_.RowSpan_.ToString(); } set { cellConfig.CellPosition_.RowSpan_ = int.Parse(value); } }
        public string ColumnSpan { get { return cellConfig.CellPosition_.ColSpan_.ToString(); } set { cellConfig.CellPosition_.ColSpan_ = int.Parse(value); } }

    }
}
