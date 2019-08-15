using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRMDesktop.Pages
{
    /// <summary>
    /// Interaction logic for Login_Page.xaml
    /// </summary>
    public partial class Login_Page : Page
    {
        public Login_Page()
        {
            if (DatabaseFunctions.client.attemptSavedLoginAsync().GetAwaiter().GetResult())
            {
                Page p = new Toolbar();
                ClientData.toolFrame.Navigate(p);
                Page p2 = new blank();
                ClientData.mainFrame.Navigate(p2);
            }
            else
            {
                InitializeComponent();
            }
        }
        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
        }
        public void LoginClick(object sender, RoutedEventArgs e)
        {
            TaskAwaiter<bool> taskAwaiter = DatabaseFunctions.client.attemptNewLogin(this.userN.Text, this.pass.Text, (bool)this.stay.IsChecked).GetAwaiter();
            while (!taskAwaiter.IsCompleted)
            {

            }
            if (taskAwaiter.GetResult())
            {
                Page p = new Toolbar();
                ClientData.toolFrame.Navigate(p);
                Page p2 = new blank();
                ClientData.mainFrame.Navigate(p2);
            }
            else
            {
                Message.Content = "Username or Password incorrect, Try again, or contact Support.";
            }
        }
    }
}
