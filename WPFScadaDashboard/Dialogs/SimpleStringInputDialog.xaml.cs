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

namespace WPFScadaDashboard.Dialogs
{
    /// <summary>
    /// Interaction logic for SimpleStringInputDialog.xaml
    /// </summary>
    public partial class SimpleStringInputDialog : Window
    {
        // https://stackoverflow.com/questions/2796470/wpf-create-a-dialog-prompt
        public SimpleStringInputDialog(string promptTitleBarText, string promptText, string initialInput)
        {
            InitializeComponent();
            PromptTitleBarText = promptTitleBarText;
            PromptText = promptText;
            ResponseText = initialInput;
            DataContext = this;
        }

        public string PromptTitleBarText { get; set; } = "Dialog Title";
        public string PromptText { get; set; } = "Prompt Text";

        public string ResponseText
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
