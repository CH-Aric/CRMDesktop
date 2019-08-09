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
using System.Windows.Shapes;

namespace CRMDesktop.Pages.Customers
{
    /// <summary>
    /// Interaction logic for Advance_Page.xaml
    /// </summary>
    public partial class Advance_Page : Page
    {
        int customer;
        List<SecurityButton> Buttons;
        public Advance_Page(int customerIn)
        {
            InitializeComponent();
            Buttons = new List<SecurityButton>();
            customer = customerIn;
            assessStage();
        }
        public void assessStage()
        {
            string sql = "SELECT Stage FROM cusindex WHERE IDKey='" + customer + "'";
            TaskCallback call = populateAssessment;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateAssessment(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            List<UIElement> list = new List<UIElement>();
            int Stage = int.Parse(dictionary["Stage"][0]);
            if (Stage < 2)
            {
                SecurityButton l = new SecurityButton(2, new string[] { "Employee" }) { Content = "Booking" };
                l.Click += onClicked;
                Buttons.Add(l);
                list.Add(l);
            }
            if (Stage < 3)
            {
                SecurityButton l = new SecurityButton(3, new string[] { "Employee" }) { Content = "Quote" };
                l.Click += onClicked;
                Buttons.Add(l);
                list.Add(l);
            }
            if (Stage < 4)
            {
                SecurityButton l = new SecurityButton(4, new string[] { "Employee" }) { Content = "Sale" };
                l.Click += onClicked;
                Buttons.Add(l);
                list.Add(l);
            }
            if (Stage < 5)
            {
                SecurityButton l = new SecurityButton(5, new string[] { "Employee" }) { Content = "Install" };
                l.Click += onClicked;
                Buttons.Add(l);
                list.Add(l);
            }
            if (Stage < 6)
            {
                SecurityButton l = new SecurityButton(6, new string[] { "Employee" }) { Content = "Installing" };
                l.Click += onClicked;
                Buttons.Add(l);
                list.Add(l);
            }
            if (Stage < 7)
            {
                SecurityButton l = new SecurityButton(7, new string[] { "Employee" }) { Content = "Quality Assurance" };
                l.Click += onClicked;
                Buttons.Add(l);
                list.Add(l);
            }
            if (Stage < 8)
            {
                SecurityButton l = new SecurityButton(8, new string[] { "Employee" }) { Content = "Clients" };
                l.Click += onClicked;
                Buttons.Add(l);
                list.Add(l);
            }
            if (Stage < 9)
            {
                SecurityButton l = new SecurityButton(9, new string[] { "Employee" }) {  Content = "Archive" };
                l.Click += onClicked;
                Buttons.Add(l);
                list.Add(l);
            }
            bool[] b = new bool[list.Count];
            GridFiller.rapidFillPremadeObjects(list,grid,b);
        }
        public void onClicked(object sender, EventArgs e)
        {
            SecurityButton db = (SecurityButton)sender;
            string sql = "UPDATE cusindex SET Stage='" + db.GetInt() + "' WHERE IDKey='" + customer + "'";
            DatabaseFunctions.SendToPhp(sql);
            blank page = new blank();
            ClientData.mainFrame.Navigate(page);
        }
    }
}
