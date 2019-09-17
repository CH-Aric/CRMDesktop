using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            string sql = "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index,cusindex.Stage FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE ( cusfields.Index LIKE '%hone%')";//cusfields.Index LIKE '%Address%' OR
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
                        string text = dictionary["Name"][i] + " ," + dictionary["Value"][i];
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
                        dataButton2.Content = dataButton2.Content + " ," + dictionary["Value"][i];
                    }
                }
            }
        }
        public void onClicked(object sender, RoutedEventArgs e)
        {
            SecurityButton dataButton = (SecurityButton)sender;
            if (dataButton.GetInt2() == 0 || dataButton.GetInt2() == 10 || dataButton.GetInt2() == 9)
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
            else if (dataButton.GetInt2() ==3)
            {
                Quote_Page page = new Quote_Page(dataButton.Integer,stageSearch);
                ClientData.mainFrame.Navigate(page);
            }
            else if (dataButton.GetInt2() > 3 && dataButton.GetInt2() < 9)
            {
                Install_Page page = new Install_Page(dataButton.Integer, stageSearch);
                ClientData.mainFrame.Navigate(page);
            }
        }
        public void onClickedSearch(object sender, RoutedEventArgs e)
        {
            string text = "%" + SearchEntry.Text + "%";
            string statement = "SELECT DISTINCT cusindex.IDKey FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID WHERE (cusfields.Value LIKE '%" + text + "%' OR cusindex.Name LIKE '%" + text + "%')";
            statement += appendPickerResult();
            TaskCallback call = new TaskCallback(this.PerformSearch);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void onClickedCreate(object sender, RoutedEventArgs e)
        {
            string sql = "INSERT INTO cusindex (Stage,Name) VALUES ('" + (NewPicker.SelectedIndex + 1) + "','CustomerName')";
            DatabaseFunctions.SendToPhp(sql);
            System.Threading.Thread.Sleep(50);
            string sql2 = "SELECT IDKey,Stage FROM cusindex ORDER BY IDKey Desc LIMIT 1";
            TaskCallback call = OpenPage;
            DatabaseFunctions.SendToPhp(false, sql2, call);
            StyledButton x = (StyledButton)sender;
            x.Background = new SolidColorBrush(Colors.SlateGray);
        }
        public void onCreateStageSpecificData(string cusID)
        {
            List<string> batch = new List<string>();
            string noteSQL = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('Notes:','','" + cusID + "'),('Created On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "','" + cusID + "'),('Modified On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "','" + cusID + "')";//'"+ClientData.AgentIDK+"','"+FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss"))+"'
            batch.Add(noteSQL);
            if (NewPicker.SelectedIndex == 0)//For creating leads
            {
                //BookingDate, Notes
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','"+cusID+"'),('PhoneNumber','','"+cusID+"')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 1)//Booked!
            {
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('PhoneNumber','','" + cusID + "'),('Salesman','','"+cusID+"')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 2)//Quoted!
            {
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('PhoneNumber','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','False','" + cusID + "'),('Deposit Received','False','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 3)//Followup on Quote
            {
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('PhoneNumber','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','False','" + cusID + "'),('Deposit Received','False','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "'),('LastContact','mm/dd','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 4)//Sold!
            {
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('PhoneNumber','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','True','" + cusID + "'),('Deposit Received','True','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 5)//Install!
            {
                string sql = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('PhoneNumber','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','True','" + cusID + "'),('Deposit Received','True','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "'),('InstallDate','','"+cusID+"')";
                batch.Add(sql);
            }
            DatabaseFunctions.SendBatchToPHP(batch);
        }
        public void OpenPage(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            SecurityButton x = new SecurityButton(int.Parse(dictionary["IDKey"][0]), new string[] { "Employee" })
            {
                Integer2 = int.Parse(dictionary["Stage"][0])
            };
            RoutedEventArgs y = new RoutedEventArgs();
            onCreateStageSpecificData(dictionary["IDKey"][0]);
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
                sql += " AND cusindex.Stage='9' ORDER BY cusindex.IDKey DESC";
            }
            else if (s == "Archived")
            {
                sql += " AND cusindex.Stage='10' ORDER BY cusindex.IDKey DESC";
            }
            return sql;
        }
    }
}