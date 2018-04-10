﻿using System;
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
        public CellPosChangeWindow(DashboardCellPosition cellPosition)
        {
            InitializeComponent();            
            posConfigVM = new PosConfigVM(cellPosition);
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

        public string RowIndex { get { return cellPosition.RowIndex_.ToString(); } set { cellPosition.RowIndex_ = int.Parse(value); } }
        public string ColumnIndex { get { return cellPosition.ColIndex_.ToString(); } set { cellPosition.ColIndex_ = int.Parse(value); } }
        public string RowSpan { get { return cellPosition.RowSpan_.ToString(); } set { cellPosition.RowSpan_ = int.Parse(value); } }
        public string ColumnSpan { get { return cellPosition.ColSpan_.ToString(); } set { cellPosition.ColSpan_ = int.Parse(value); } }

        public DashboardCellPosition cellPosition;

        public PosConfigVM(DashboardCellPosition cellPosition)
        {
            this.cellPosition = cellPosition;
            NotifyPropertyChanged("RowIndex");
            NotifyPropertyChanged("ColumnIndex");
            NotifyPropertyChanged("RowSpan");
            NotifyPropertyChanged("ColumnSpan");
        }
    }
}