using CRMDesktop.Pages;
using CRMDesktop.Pages.Customers;
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
            mainFrame.Height = SystemParameters.MaximizedPrimaryScreenHeight-topFrame.Height;
            sideFrame.Height = mainFrame.Height;
            mainFrame.Width = SystemParameters.PrimaryScreenWidth*0.8;
            sideFrame.Width = SystemParameters.PrimaryScreenWidth * 0.2;
            topFrame.Width = SystemParameters.PrimaryScreenWidth;
            ClientData.FrameHeight = mainFrame.Height;
            ClientData.FrameWidth = mainFrame.Width;
            ClientData.mainFrame = mainFrame;
            ClientData.sideFrame = sideFrame;
            ClientData.toolFrame = topFrame;
            Login_Page page = new Login_Page();
            mainFrame.Navigate(page);
        }
    }
    
}