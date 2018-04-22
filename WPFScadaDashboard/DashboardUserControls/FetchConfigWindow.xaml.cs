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
    /// Interaction logic for FetchConfigWindow.xaml
    /// </summary>
    public partial class FetchConfigWindow : Window
    {
        public FetchConfigVM fetchConfigVM;
        public FetchConfigWindow()
        {
            InitializeComponent();
            fetchConfigVM = new FetchConfigVM();
            AutoFetchConfigForm.DataContext = fetchConfigVM;
        }

        public void SetAutoFetchConfig(AutoFetchConfig autoFetchConfig)
        {
            fetchConfigVM.AutoFetchConfig = autoFetchConfig;
        }

        private void UpdateChanges_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Update Auto Fetch Configuration ?", "Update Configurtion", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
            }
            else
            {
                DialogResult = true;
            }
        }

    }

    public class FetchConfigVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private AutoFetchConfig autoFetchConfig;

        public AutoFetchConfig AutoFetchConfig
        {
            get { return autoFetchConfig; }
            set
            {
                autoFetchConfig = value;
                NotifyPropertyChanged("FetchWindowHrs");
                NotifyPropertyChanged("FetchWindowMins");
                NotifyPropertyChanged("FetchWindowSecs");
            }
        }

        public FetchConfigVM()
        {
            this.autoFetchConfig = new AutoFetchConfig();
        }

        public string FetchWindowHrs
        {
            get { return autoFetchConfig.TimePeriod_.HoursOffset_.ToString(); }
            set
            {
                int intVal = autoFetchConfig.TimePeriod_.HoursOffset_;
                if (int.TryParse(value, out intVal))
                {
                    autoFetchConfig.TimePeriod_.HoursOffset_ = intVal;
                }
            }
        }

        public string FetchWindowMins
        {
            get { return autoFetchConfig.TimePeriod_.MinsOffset_.ToString(); }
            set
            {
                int intVal = autoFetchConfig.TimePeriod_.MinsOffset_;
                if (int.TryParse(value, out intVal))
                {
                    autoFetchConfig.TimePeriod_.MinsOffset_ = intVal;
                }
            }
        }

        public string FetchWindowSecs
        {
            get { return autoFetchConfig.TimePeriod_.SecsOffset_.ToString(); }
            set
            {
                int intVal = autoFetchConfig.TimePeriod_.SecsOffset_;
                if (int.TryParse(value, out intVal))
                {
                    autoFetchConfig.TimePeriod_.SecsOffset_ = intVal;
                }
            }
        }
    }
}
