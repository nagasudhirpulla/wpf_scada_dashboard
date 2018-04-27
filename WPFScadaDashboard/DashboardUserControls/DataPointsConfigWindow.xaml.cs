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

        // https://stackoverflow.com/questions/5511/numeric-data-entry-in-wpf
        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Helpers.NumericTextValidation.AreAllValidNumericChars(e.Text);
            base.OnPreviewTextInput(e);
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
                DependencyObject tvi = Helpers.ListUtility.FindParentTreeItem(button, typeof(ListBoxItem));
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

        private void LbEditBtn_Click(object sender, RoutedEventArgs e)
        {
            //a button on list view has been clicked
            Button button = sender as Button;
            //walk up the tree to find the ListboxItem
            DependencyObject tvi = Helpers.ListUtility.FindParentTreeItem(button, typeof(ListBoxItem));
            //if not null cast the Dependancy object to type of Listbox item.
            if (tvi != null)
            {
                ListBoxItem lbi = tvi as ListBoxItem;
                IDashboardTimeSeriesPoint timeSeriesPoint = (IDashboardTimeSeriesPoint)lbi.DataContext;
                // Open edit window for this point
                if (timeSeriesPoint is DashboardScadaTimeSeriesPoint scadaTimeSeriesPoint)
                {
                    ScadaTimeSeriesPointEditWindow scadaTimeSeriesPointEditWindow = new ScadaTimeSeriesPointEditWindow(new DashboardScadaTimeSeriesPoint(scadaTimeSeriesPoint));
                    scadaTimeSeriesPointEditWindow.ShowDialog();
                    if (scadaTimeSeriesPointEditWindow.DialogResult == true)
                    {
                        // update the point
                        int pointIndex = dataPointsVM.dashboardTimeSeriesPoints.IndexOf(timeSeriesPoint);
                        if (pointIndex >= 0)
                        {
                            dataPointsVM.dashboardTimeSeriesPoints[pointIndex] = scadaTimeSeriesPointEditWindow.scadaTimeSeriesPointVM.ScadaTimeSeriesPoint;
                            ICollectionView view = CollectionViewSource.GetDefaultView(dataPointsVM.dashboardTimeSeriesPoints);
                            view.Refresh();                            
                        }
                    }
                }
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            // show relavent edit window
            if (TimeSeriesPointTypesComboBox.SelectedIndex > -1 && TimeSeriesPointTypesComboBox.SelectedIndex <= dataPointsVM.PointTypes.Count && dataPointsVM.PointTypes[TimeSeriesPointTypesComboBox.SelectedIndex] == DashboardScadaTimeSeriesPoint.timeSeriesType)
            {
                // show scada point edit window with relavent initialisation
                DateTime startTime = DateTime.Now;
                DateTime endTime = startTime;
                ScadaDataPoint pnt = new ScadaDataPoint("");
                if (dataPointsVM.dashboardTimeSeriesPoints.Count > 0)
                {
                    startTime = dataPointsVM.dashboardTimeSeriesPoints.ElementAt(0).StartTime;
                    endTime = dataPointsVM.dashboardTimeSeriesPoints.ElementAt(0).EndTime;
                }
                DashboardScadaTimeSeriesPoint scadaTimeSeriesPoint = new DashboardScadaTimeSeriesPoint(pnt, startTime, endTime);
                ScadaTimeSeriesPointEditWindow scadaTimeSeriesPointEditWindow = new ScadaTimeSeriesPointEditWindow(scadaTimeSeriesPoint);
                scadaTimeSeriesPointEditWindow.ShowDialog();
                if (scadaTimeSeriesPointEditWindow.DialogResult == true)
                {
                    // update the point
                    dataPointsVM.dashboardTimeSeriesPoints.Add(scadaTimeSeriesPointEditWindow.scadaTimeSeriesPointVM.ScadaTimeSeriesPoint);
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

        public List<string> PointTypes { get; set; } = new List<string> { DashboardScadaTimeSeriesPoint.timeSeriesType };

        public DataPointsConfigVM(LinePlotCellConfig dashboardCellConfig)
        {
            this.dashboardTimeSeriesPoints = new ObservableCollection<IDashboardTimeSeriesPoint>(dashboardCellConfig.TimeSeriesPoints_);
        }
    }
}
