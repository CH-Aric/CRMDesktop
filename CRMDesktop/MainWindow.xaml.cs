using CRMDesktop.Pages;
using CRMDesktop.Pages.Inventory;
using System.Windows;

namespace CRMDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            mainFrame.Height = SystemParameters.PrimaryScreenHeight-102;
            mainFrame.Width = SystemParameters.PrimaryScreenWidth;
            ClientData.FrameHeight = mainFrame.Height;
            ClientData.FrameWidth = mainFrame.Width;
            ClientData.mainFrame = mainFrame;
        }
        public void onClickAccess(object sender,RoutedEventArgs e)
        {
            StyledButton sb = (StyledButton)sender;
            if (sb.Content.Equals("Pricing Guide"))
            {
                Price_Page page = new Price_Page();
                mainFrame.Navigate(page);
            }else if (sb.Content.Equals("Inventory"))
            {
                Items_Page page = new Items_Page();
                mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Chat"))
            {
                Chat_Page page = new Chat_Page();
                mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Coupon Checker"))
            {
                CouponChecker_Page page = new CouponChecker_Page();
                mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Manage Favourites"))
            {
                Favourites_Page page = new Favourites_Page();
                mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("View Tasks"))
            {
                Tasks_Page page = new Tasks_Page();
                mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("CDR"))
            {
                CDR_Page page = new CDR_Page(true,"");
                mainFrame.Navigate(page);
            }
            else if (sb.Content.Equals("Missed Calls"))
            {
                Check_Page page = new Check_Page();
                mainFrame.Navigate(page);
            }
        }
    }
}