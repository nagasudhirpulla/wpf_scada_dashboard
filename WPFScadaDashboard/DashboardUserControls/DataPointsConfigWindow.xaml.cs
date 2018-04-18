using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WPFScadaDashboard.DashboardDataPointClasses;

namespace WPFScadaDashboard.DashboardUserControls
{
    /// <summary>
    /// Interaction logic for DataPointsConfigWindow.xaml
    /// </summary>
    public partial class DataPointsConfigWindow : Window
    {
        public DataPointsConfigVM dataPointsVM;
        public DataPointsConfigWindow(LinePlotCellConfig dashboardCellConfig)
        {
            InitializeComponent();
            dataPointsVM = new DataPointsConfigVM(dashboardCellConfig);
            DataPointsConfigForm.DataContext = dataPointsVM;
            lbTimeSeriesPoints.ItemsSource = dataPointsVM.dashboardTimeSeriesPoints;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Update Data Points ?", "Cell Data Points Configuration", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
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

        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/b1a0c663-5665-433d-bcad-76fabe90b406/select-listboxitem-on-buttonclick-in-listboxitem?forum=wpf
        private DependencyObject FindParentTreeItem(DependencyObject CurrentControl, Type ParentType)
        {
            bool notfound = true;
            while (notfound)
            {
                DependencyObject parent = VisualTreeHelper.GetParent(CurrentControl);
                string ParentTypeName = ParentType.Name;
                //Compare current type name with what we want
                if (parent == null)
                {
                    System.Diagnostics.Debugger.Break();
                    notfound = false;
                    continue;
                }
                if (parent.GetType().Name == ParentTypeName)
                {
                    return parent;
                }
                //we haven't found it so walk up the tree.
                CurrentControl = parent;
            }
            return null;
        }

        private void LbDeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Remove Data Point ?", "Cell Data Points Configuration", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            else
            {
                //a button on list view has been clicked
                Button button = sender as Button;
                //walk up the tree to find the ListboxItem
                DependencyObject tvi = FindParentTreeItem(button, typeof(ListBoxItem));
                //if not null cast the Dependancy object to type of Listbox item.
                if (tvi != null)
                {
                    ListBoxItem lbi = tvi as ListBoxItem;
                    // Delete the object from Observable Collection
                    IDashboardTimeSeriesPoint timeSeriesPoint = (IDashboardTimeSeriesPoint)lbi.DataContext;
                    dataPointsVM.dashboardTimeSeriesPoints.Remove(timeSeriesPoint);
                }
            }
        }
    }

    public class DataPointsConfigVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<IDashboardTimeSeriesPoint> dashboardTimeSeriesPoints;

        public DataPointsConfigVM(LinePlotCellConfig dashboardCellConfig)
        {
            this.dashboardTimeSeriesPoints = new ObservableCollection<IDashboardTimeSeriesPoint>(dashboardCellConfig.TimeSeriesPoints_);
        }


    }
}
