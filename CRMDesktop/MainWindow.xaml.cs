using CRMDesktop.Pages;
using CRMDesktop.Pages.Customers;
using CRMDesktop.Pages.Inventory;
using System.Diagnostics;
using System.Net;
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
            ServicePointManager.DefaultConnectionLimit = 25;
            mainFrame.Height = SystemParameters.MaximizedPrimaryScreenHeight-topFrame.Height*2;
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
            FormatFunctions.LoadAgentDictionary();
            mainFrame.Navigate(page);
        }
    }
}