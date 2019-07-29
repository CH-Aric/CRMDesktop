using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace CRMDesktop
{
    public static class Navigator
    {
        public static Frame MainFrame;
        public static NavigationService Navi = NavigationService.GetNavigationService(new DependencyObject());
    }

}
