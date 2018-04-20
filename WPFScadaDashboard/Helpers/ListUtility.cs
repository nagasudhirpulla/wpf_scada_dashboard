using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WPFScadaDashboard.DashboardDataPointClasses;

namespace WPFScadaDashboard.Helpers
{
    public class ListUtility
    {
        // https://social.msdn.microsoft.com/Forums/vstudio/en-US/b1a0c663-5665-433d-bcad-76fabe90b406/select-listboxitem-on-buttonclick-in-listboxitem?forum=wpf
        public static DependencyObject FindParentTreeItem(DependencyObject CurrentControl, Type ParentType)
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

        public static DateTime GetDateTime(DateTime timeAbsolute_, VariableTime timeVariable_, string timeMode_, string dateMode_, int dateOffset_)
        {
            if (timeMode_ == DashboardScadaTimeSeriesPoint.AbsoluteMode)
            {
                if (dateMode_ == DashboardScadaTimeSeriesPoint.VariableMode)
                {
                    // stub
                    // return variable date but with time of the StartTimeAbsolute_ component
                    DateTime date = DateTime.Now.AddDays(-1 * dateOffset_);
                    return new DateTime(date.Year, date.Month, date.Day, timeAbsolute_.Hour, timeAbsolute_.Minute, timeAbsolute_.Second);
                }
                return timeAbsolute_;

            }
            else
            {
                return DateTime.Now.AddHours(-1 * timeVariable_.HoursOffset_).AddMinutes(-1 * timeVariable_.MinsOffset_).AddSeconds(-1 * timeVariable_.SecsOffset_);
            }
        }
    }
}
