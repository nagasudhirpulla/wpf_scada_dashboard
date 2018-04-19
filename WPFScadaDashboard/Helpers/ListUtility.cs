using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

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
    }
}
