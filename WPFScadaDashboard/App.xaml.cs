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
            /*
            // https://stackoverflow.com/questions/26845815/how-to-pass-file-as-parameter-to-your-program-wpf-c-sharp
            this.Properties["FilePathArgName"] = null;
            var args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                var fileName = args[1];
                if (File.Exists(fileName))
                {
                    //
                    //var extension = Path.GetExtension(fileName);
                    //if (extension == ".sudhir")
                    //{
                    //    this.Properties["FilePathArgName"] = fileName;
                    //}
                    //
                    this.Properties["FilePathArgName"] = fileName;
                }
            }
            */
            //https://blogs.msdn.microsoft.com/avip/2008/10/26/wpf-supporting-command-line-arguments-and-file-extensions/
            // Check if this was launched by double-clicking a doc. If so, use that as the
            // startup file name.
            if (AppDomain.CurrentDomain.SetupInformation
                .ActivationArguments != null && AppDomain.CurrentDomain.SetupInformation
                .ActivationArguments.ActivationData != null
            && AppDomain.CurrentDomain.SetupInformation
                .ActivationArguments.ActivationData.Length > 0)
            {
                string fname = "No filename given";
                try
                {
                    fname = AppDomain.CurrentDomain.SetupInformation
                            .ActivationArguments.ActivationData[0];

                    // It comes in as a URI; this helps to convert it to a path.
                    Uri uri = new Uri(fname);
                    fname = uri.LocalPath;

                    this.Properties["FilePathArgName"] = fname;

                }
                catch (Exception ex)
                {
                    // For some reason, this couldn't be read as a URI.
                    // Do what you must...
                }
            }
            base.OnStartup(e);
        }
    }
}
