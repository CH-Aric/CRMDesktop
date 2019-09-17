using CRMDesktop.Pages;
using CRMDesktop.Pages.Customers;
using CRMDesktop.Pages.Inventory;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRMDesktop
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    public partial class Toolbar : Page
    {
        public Toolbar()
        {
            InitializeComponent();
        }
        public void onClickAccess(object sender, RoutedEventArgs e)
        {
            StyledButton sb = (StyledButton)sender;
            if (sb.Content.Equals("Pricing Guide"))
            {
                Price_Page page = new Price_Page();
                ClientData.mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Inventory"))
            {
                Items_Page page = new Items_Page();
                ClientData.mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Chat"))
            {
                Chat_Page page = new Chat_Page();
                ClientData.mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Coupon Checker"))
            {
                CouponChecker_Page page = new CouponChecker_Page();
                ClientData.mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Manage Favourites"))
            {
                Favourites_Page page = new Favourites_Page();
                ClientData.mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("View Tasks"))
            {
                Tasks_Page page = new Tasks_Page();
                ClientData.mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("CDR"))
            {
                CDR_Page page = new CDR_Page(true, "");
                ClientData.mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Missed Calls"))
            {
                Check_Page page = new Check_Page();
                ClientData.mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("View Customers"))
            {
                CustomerList_Page page = new CustomerList_Page();
                ClientData.sideFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Role Assignment"))
            {
                RoleAssignment_Page page = new RoleAssignment_Page();
                ClientData.sideFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Create Agent"))
            {
                Create_Agent page = new Create_Agent();
                ClientData.mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Punch Clock"))
            {
                Punch_Page page = new Punch_Page();
                ClientData.mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Punch Admin"))
            {
                PunchAdmin_Page page = new PunchAdmin_Page();
                ClientData.mainFrame.Navigate(page);
            }
        }
    }//

}
