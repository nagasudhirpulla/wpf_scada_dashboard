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
using WPFScadaDashboard.DashboardDataPointClasses;

namespace WPFScadaDashboard.DashboardUserControls
{
    /// <summary>
    /// Interaction logic for ScadaTimeSeriesPointEditWindow.xaml
    /// </summary>
    public partial class ScadaTimeSeriesPointEditWindow : Window
    {
        public ScadaTimeSeriesPointVM scadaTimeSeriesPointVM;
        public ScadaTimeSeriesPointEditWindow(DashboardScadaTimeSeriesPoint pnt)
        {
            InitializeComponent();
            scadaTimeSeriesPointVM = new ScadaTimeSeriesPointVM(pnt);
            ScadaPointEditForm.DataContext = scadaTimeSeriesPointVM;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Update Data Point ?", "Scada Timeseries Data Point Configuration", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
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
    }

    public class ScadaTimeSeriesPointVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DashboardScadaTimeSeriesPoint ScadaTimeSeriesPoint { get; set; }

        public ScadaTimeSeriesPointVM(DashboardScadaTimeSeriesPoint scadaTimeSeriesPoint)
        {
            this.ScadaTimeSeriesPoint = scadaTimeSeriesPoint;
        }
    }
}
