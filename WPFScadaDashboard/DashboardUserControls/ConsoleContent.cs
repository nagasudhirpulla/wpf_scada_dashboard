using System;
using System.Collections.ObjectModel;

namespace WPFScadaDashboard.DashboardUserControls
{
    public class ConsoleContent
    {
        ObservableCollection<string> consoleOutput_ = new ObservableCollection<string>();

        public ObservableCollection<string> ConsoleOutput
        {
            get
            {
                return consoleOutput_;
            }
            set
            {
                consoleOutput_ = value;
            }
        }

        public void AddItemsToConsole(string str)
        {
            consoleOutput_.Add(DateTime.Now.ToString("HH:mm:ss") + " " + str);
        }

        public void ClearConsole()
        {
            consoleOutput_.Clear();
        }
    }
}
