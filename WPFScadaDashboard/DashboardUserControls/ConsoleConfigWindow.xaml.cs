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

namespace WPFScadaDashboard.DashboardUserControls
{
    /// <summary>
    /// Interaction logic for ConsoleConfigWindow.xaml
    /// </summary>
    public partial class ConsoleConfigWindow : Window
    {
        public ConsoleConfigVM ConsoleConfigVM_;
        public ConsoleConfigWindow(int consoleHeight)
        {
            InitializeComponent();
            ConsoleConfigVM_ = new ConsoleConfigVM(consoleHeight);
            ConsoleConfigForm.DataContext = ConsoleConfigVM_;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Update Console Configuration ?", "Console Configuration", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
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

    public class ConsoleConfigVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string ConsoleHeightStr { get { return ConsoleHeight_.ToString(); } set { ConsoleHeight_ = int.Parse(value); } }

        public int ConsoleHeight_;

        public ConsoleConfigVM(int consoleHeight)
        {
            this.ConsoleHeight_ = consoleHeight;
            NotifyPropertyChanged("ConsoleHeightStr");
        }
    }
}
