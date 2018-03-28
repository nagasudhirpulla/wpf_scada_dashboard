using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WPFScadaDashboard
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // https://stackoverflow.com/questions/26845815/how-to-pass-file-as-parameter-to-your-program-wpf-c-sharp
            this.Properties["FilePathArgName"] = null;
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                var fileName = args[1];
                if (File.Exists(fileName))
                {
                    /*
                    var extension = Path.GetExtension(fileName);
                    if (extension == ".sudhir")
                    {
                        this.Properties["FilePathArgName"] = fileName;
                    }
                    */
                    this.Properties["FilePathArgName"] = fileName;
                }
            }
            base.OnStartup(e);
        }
    }
}
