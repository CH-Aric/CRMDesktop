using CRMDesktop.Pages;
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
            }
        }
    }
}