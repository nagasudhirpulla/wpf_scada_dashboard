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
using System.Windows.Shapes;

namespace WPFScadaDashboard
{
    /// <summary>
    /// Interaction logic for AppSettingsWindow.xaml
    /// </summary>
    public partial class AppSettingsWindow : Window
    {
        public AppSettingsWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public bool IsScadaRandomFetch
        {
            get { return AppSettingsHelper.GetSetting<bool>("scada_fetch_random", false); }
            set { AppSettingsHelper.SetSetting("scada_fetch_random", value.ToString()); }
        }

        private void OK_Pressed_Handler(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
