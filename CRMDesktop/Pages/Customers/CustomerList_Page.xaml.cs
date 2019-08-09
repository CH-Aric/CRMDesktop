using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CRMDesktop.Pages.Customers
{
    /// <summary>
    /// Interaction logic for CustomerList_Page.xaml
    /// </summary>
    public partial class CustomerList_Page : Page
    {
        int stageSearch = 0;
        public CustomerList_Page()
        {
            this.InitializeComponent();
            scroll.Height = ClientData.FrameHeight-75;
        }
        public void loadFromDatabase()
        {
            TaskCallback call = populateList;
            string sql = "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index,cusindex.Stage FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Index LIKE '%Address%' OR cusfields.Index LIKE '%Phone%')";
            sql += appendPickerResult();
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateList(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            Dictionary<string, SecurityButton> dictionary2 = new Dictionary<string, SecurityButton>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Name"].Count; i++)
                {
                    if (!dictionary2.ContainsKey(dictionary["IDKey"][i]))
                    {
                        string text = dictionary["Name"][i] + "" + dictionary["Value"][i];
                        SecurityButton dataButton = new SecurityButton(int.Parse(dictionary["IDKey"][i]), new string[] { "Employee" })
                        {
                            Content = text,
                            Width=ClientData.sideFrame.Width*0.925
                        };
                        dataButton.Click += this.onClicked;
                        dataButton.Integer2 = int.Parse(dictionary["Stage"][i]);
                        List<UIElement> list = new List<UIElement>() { dataButton };
                        bool[] box = new bool[] { false };
                        GridFiller.rapidFillPremadeObjectsStandardHeight(list, dataGrid, box, 25);

                        dictionary2.Add(dictionary["IDKey"][i], dataButton);
                    }
                    else
                    {
                        SecurityButton dataButton2 = dictionary2[dictionary["IDKey"][i]];
                        dataButton2.Content = dataButton2.Content + ", " + dictionary["Value"][i];
                    }
                }
            }
        }
        public void onClicked(object sender, RoutedEventArgs e)
        {
            SecurityButton dataButton = (SecurityButton)sender;
            if (dataButton.GetInt2() == 0)
            {
                CustomerDetail_Page page = new CustomerDetail_Page(dataButton.Integer);
                ClientData.mainFrame.Navigate(page);
            }
            else if (dataButton.GetInt2() == 1)
            {
                Request_Page page = new Request_Page(dataButton.Integer);
                ClientData.mainFrame.Navigate(page);
            }
            else if (dataButton.GetInt2() == 2)
            {
                Booking_Page page = new Booking_Page(dataButton.Integer);
                ClientData.mainFrame.Navigate(page);
            }
            else if (dataButton.GetInt2() > 2 && dataButton.GetInt2() < 9)
            {
                Quote_Page page = new Quote_Page(dataButton.Integer,stageSearch);
                ClientData.mainFrame.Navigate(page);
            }
        }
        public void onClickedSearch(object sender, RoutedEventArgs e)
        {
            string text = "%" + SearchEntry.Text + "%";
            string statement = "SELECT DISTINCT cusindex.IDKey FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Value LIKE '" + text + "' OR cusindex.Name LIKE '" + text + "')";
            statement += appendPickerResult();
            TaskCallback call = new TaskCallback(this.PerformSearch);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void onClickedCreate(object sender, RoutedEventArgs e)
        {
            string sql = "INSERT INTO cusindex (Stage) VALUES ('" + (NewPicker.SelectedIndex + 1) + "')";
            DatabaseFunctions.SendToPhp(sql);
            System.Threading.Thread.Sleep(500);
            string sql2 = "SELECT IDKey,Stage FROM cusindex ORDER BY IDKey Desc LIMIT 1";
            TaskCallback call = OpenPage;
            DatabaseFunctions.SendToPhp(false, sql2, call);
        }
        public void OpenPage(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            SecurityButton x = new SecurityButton(int.Parse(dictionary["IDKey"][0]), new string[] { "Employee" });
            x.Integer2 = int.Parse(dictionary["Stage"][0]);
            RoutedEventArgs y = new RoutedEventArgs();
            onClicked(x, y);
        }
        public void PerformSearch(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            TaskCallback call = new TaskCallback(this.populateList);
            if (dictionary.Count > 0)
            {
                string text = "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index,cusindex.Stage FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Index LIKE '%Address%' OR cusfields.Index LIKE '%Phone%') AND (";
                foreach (string str in dictionary["IDKey"])
                {
                    text = text + " cusindex.IDKey='" + str + "' OR";
                }
                text += " cusindex.IDKey='0');";
                PurgeCells();
                DatabaseFunctions.SendToPhp(false, text, call);
            }
        }
        public void PurgeCells()
        {
            GridFiller.PurgeAllGrid(dataGrid);
        }
        public string appendPickerResult()
        {
            string sql = "";
            string s = (string)picker.Text;
            if (s == "All")
            {
                sql += "ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "Leads")
            {
                sql += " AND cusindex.Stage='1' ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "Booked")
            {
                sql += " AND cusindex.Stage='2' ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "Quoted")
            {
                sql += " AND cusindex.Stage='3' ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "Follow Up With")
            {
                sql += " AND cusindex.Stage='3' ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "Sold")
            {
                sql += " AND cusindex.Stage='4' ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "Installs")
            {
                sql += " AND cusindex.Stage='5' ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "Installing")
            {
                sql += " AND cusindex.Stage='6' ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "QA")
            {
                sql += " AND cusindex.Stage='7' ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "Clients")
            {
                sql += " AND cusindex.Stage='8' ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "Archived")
            {
                sql += " AND cusindex.Stage='9' ORDER BY cusindex.IDKey DESC";
            }
            return sql;
        }
    }
}
