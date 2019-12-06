﻿using System;
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
            string sql = "SELECT cusindex.Name,cusindex.IDKey,cusfields.Value,cusfields.Index,jobindex.Stage FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID INNER JOIN jobindex ON cusindex.IDKey=jobindex.CusID WHERE (cusfields.Index LIKE '%Phone%')";
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
                        string text = FormatFunctions.PrettyDate(dictionary["Name"][i]) + " ," + FormatFunctions.PrettyDate(dictionary["Value"][i]);
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
            
                Evolving_Page page = new Evolving_Page(dataButton.Integer);
                ClientData.mainFrame.Navigate(page);
            
        }
        public void onClickedSearch(object sender, RoutedEventArgs e)
        {
            string text = "%" + SearchEntry.Text + "%";
            string statement = "SELECT DISTINCT cusindex.IDKey FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID INNER JOIN jobindex ON cusindex.IDKey=jobindex.CusID WHERE (cusfields.Value LIKE '" + text + "' OR cusindex.Name LIKE '" + text + "' OR jobindex.Name LIKE '" + text + "')";
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
            StyledButton x = (StyledButton)sender;
            x.Background = new SolidColorBrush(Colors.SlateGray);
        }
        public void onCreateStageSpecificData(string cusID)
        {
            List<string> batch = new List<string>();
            string noteSQL = "INSERT INTO jobfields (cusfields.Index,cusfields.Value,CusID) VALUES ('Notes:','','" + cusID + "'),('Created On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "','" + cusID + "'),('Modified On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "','" + cusID + "')";//'"+ClientData.AgentIDK+"','"+FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss"))+"'
            batch.Add(noteSQL);
            string sql2 = "INSERT INTO cusfields (cusfields.Index,cusfields.Value,CusID) VALUES ('PhoneNumber','','" + cusID + "')";
            batch.Add(sql2);
            if (NewPicker.SelectedIndex == 0)//For creating leads
            {
                //BookingDate, Notes
                string sql = "INSERT INTO jobfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 1)//Booked!
            {
                string sql = "INSERT INTO jobfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('Salesman','','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 2)//Quoted!
            {
                string sql = "INSERT INTO jobfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','False','" + cusID + "'),('Deposit Received','False','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 3)//Followup on Quote
            {
                string sql = "INSERT INTO jobfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','False','" + cusID + "'),('Deposit Received','False','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "'),('LastContact','mm/dd','" + cusID + "')";
                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 4)//Sold!
            {
                string sql = "INSERT INTO jobfields (cusfields.Index,cusfields.Value,CusID) VALUES ('BookingDate','','" + cusID + "'),('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','True','" + cusID + "'),('Deposit Received','True','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "')";

                batch.Add(sql);
            }
            else if (NewPicker.SelectedIndex == 5)//Install!
            {
                string sql = "INSERT INTO jobfields (cusfields.Index,cusfields.Value,CusID) VALUES ('Salesman','','" + cusID + "'),('QuoteTotal','','" + cusID + "'),('Signature','True','" + cusID + "'),('Deposit Received','True','" + cusID + "'),('Payment Method','Fill Me Out!','" + cusID + "'),('InstallDate','','" + cusID + "')";
                batch.Add(sql);
            }
            DatabaseFunctions.SendBatchToPHP(batch);
        }
        public void OpenPage(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));

            string sqlj = "INSERT INTO jobindex (Stage,CusID) VALUES ('" + (NewPicker.SelectedIndex + 1) + "','" + dictionary["IDKey"][0] + "')";
            string sqlf = "INSERT INTO cusfields (CusID,Index,Value) VALUES ('" + dictionary["IDKey"][0] + "','Phone',''),('" + dictionary["IDKey"][0] + "','Address',''),('" + dictionary["IDKey"][0] + "','Email',''),('" + dictionary["IDKey"][0] + "','Created On',''),('" + dictionary["IDKey"][0] + "','Region',''),('" + dictionary["IDKey"][0] + "','Notes',''),('" + dictionary["IDKey"][0] + "','Modified On',''),('" + dictionary["IDKey"][0] + "','Source','')";
            DatabaseFunctions.SendToPhp(sqlj);
            DatabaseFunctions.SendToPhp(sqlf);
            SecurityButton x = new SecurityButton(int.Parse(dictionary["IDKey"][0]), new string[] { "Employee" }) { Integer2 = int.Parse(dictionary["Stage"][0]) };
            RoutedEventArgs y = new RoutedEventArgs();
            onClicked(x, y);
        }
        public void PerformSearch(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            TaskCallback call = new TaskCallback(this.populateList);
            if (dictionary.Count > 0)
            {
                string text = "SELECT cusindex.Name,jobindex.IDKey,cusfields.Value,cusfields.Index,cusindex.Stage FROM cusindex INNER JOIN cusfields ON cusindex.IDKey=cusfields.CusID INNER JOIN jobindex ON cusindex.IDKey=jobindex.CusID WHERE (cusfields.Index LIKE '%Phone%') AND (";
                foreach (string str in dictionary["IDKey"])
                {
                    text = text + " jobindex.IDKey='" + str + "' OR";
                }
                text += " cusindex.IDKey='0');";
                this.PurgeCells();
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
            if ((string)NewPicker.SelectedItem == "All")
            {
                sql += "ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Leads")
            {
                sql += " AND jobindex.Stage='1' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Booked")
            {
                sql += " AND jobindex.Stage='2' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Quoted")
            {
                sql += " AND jobindex.Stage='3' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Follow Up With")
            {
                sql += " AND jobindex.Stage='3' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Sold")
            {
                sql += " AND jobindex.Stage='4' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Installs")
            {
                sql += " AND jobindex.Stage='5' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Installing")
            {
                sql += " AND jobindex.Stage='6' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "QA")
            {
                sql += " AND jobindex.Stage='7' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Clients")
            {
                sql += " AND jobindex.Stage='8' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Archived")
            {
                sql += " AND jobindex.Stage='9' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Service Lead")
            {
                sql += " AND jobindex.Stage='10' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Service Appointment")
            {
                sql += " AND jobindex.Stage='11' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Serviced")
            {
                sql += " AND jobindex.Stage='12' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Maintenance Appointment")
            {
                sql += " AND jobindex.Stage='14' ORDER BY cusindex.IDKey DESC";
            }
            else if ((string)NewPicker.SelectedItem == "Maintenance")
            {
                sql += " AND jobindex.Stage='15' ORDER BY cusindex.IDKey DESC";
            }
            return sql;
        }
    }
}