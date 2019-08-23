﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace CRMDesktop.Pages.Customers
{
    /// <summary>
    /// Interaction logic for Booking_Page.xaml
    /// </summary>
    public partial class Booking_Page : Page
    {
        int customer;
        private List<DataPair> entryDict;
        public string address;
        public Booking_Page(int cusID)
        {
            customer = cusID;
            InitializeComponent();
            searchCustomerData();
            scroll.Height = ClientData.mainFrame.Height - 60;
        }
        public void onClickAdvance(object sender, RoutedEventArgs e)
        {
            Advance_Page page = new Advance_Page(customer);
            ClientData.mainFrame.Navigate(page);
        }
        public async void renderBookingMap(string Address)
        {
            /*var geoloc = await Geocoding.GetLocationsAsync(Address);
            place = geoloc.FirstOrDefault();
            Xamarin.Forms.Maps.Map map = new Xamarin.Forms.Maps.Map(MapSpan.FromCenterAndRadius(new Position(place.Latitude, place.Longitude), Distance.FromKilometers(1)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            HeadData.Children.Add(map, 0, 3);
            Grid.SetColumnSpan(map, 2);*/
        }
        public async void onClickNavigate(object sender, RoutedEventArgs e)
        {
            //var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            //await Xamarin.Essentials.Map.OpenAsync(place, options);
        }
        public void searchCustomerData()
        {
            string sql = "SELECT cusfields.Index,cusfields.value,cusindex.Name,cusindex.IDKey AS FID FROM crm2.cusfields INNER JOIN crm2.cusindex ON cusfields.CusID=cusindex.IDKey WHERE (cusfields.Index LIKE '%phone%' OR cusfields.Index LIKE '%address%' OR cusfields.Index LIKE '%book%') AND cusfields.CusID='" + customer + "';";
            TaskCallback call = populatePage;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populatePage(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            entryDict = new List<DataPair>();
            string address = "";
            if (dictionary.Count > 0)
            {
                nameLabel.Text = dictionary["Name"][0];
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    if (dictionary["Index"][i].Contains("hone"))
                    {
                        phoneLabel.Text = dictionary["value"][i];
                    }
                    else if (dictionary["Index"][i].Contains("otes"))
                    {
                        noteLabel.Text += dictionary["value"][i];
                    }
                    else if (dictionary["Index"][i].Contains("ookin"))
                    {
                        BookingDate.Text = FormatFunctions.PrettyDate(dictionary["value"][i]);
                        scroll.Height = 0;
                    }
                    else
                    {
                        DataPair dataPair = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["value"][i], dictionary["Index"][i]);
                        dataPair.Value.Text = dictionary["value"][i];
                        dataPair.Value.ToolTip = "Value here";
                        dataPair.Value.Width = ClientData.mainFrame.Width * 0.7 - 10;
                        dataPair.Index.Text = dictionary["Index"][i];
                        dataPair.Index.ToolTip = "Index here";
                        dataPair.Index.Width = ClientData.mainFrame.Width * 0.3 - 10;
                        StackPanel stackLayout = new StackPanel
                        {
                            Orientation = Orientation.Horizontal
                        };
                        stackLayout.Children.Add(dataPair.Index);
                        stackLayout.Children.Add(dataPair.Value);
                        BodyGrid.Children.Add(stackLayout);
                        entryDict.Add(dataPair);
                        if (dictionary["Index"][i].Contains("dress"))
                        {
                            address = dictionary["Value"][i];
                        }
                    }
                }
            }
            renderBookingMap(address);
            //populateFileList();
        }
        public void onClicked(object sender, RoutedEventArgs e)
        {
            List<string> batch = new List<string>();
            foreach (DataPair dataPair in this.entryDict)
            {
                if (dataPair.isNew)
                {
                    string s = "INSERT INTO cusfields (cusfields.Value,cusfields.Index,CusID) VALUES('" + FormatFunctions.CleanDateNew(dataPair.Value.Text) + "','" + FormatFunctions.CleanDateNew(dataPair.Index.Text) + "','" + this.customer + "')";
                    batch.Add(s);
                    dataPair.isNew = false;
                }
                else if (!dataPair.Index.Text.Equals(dataPair.Index.GetInit()))
                {
                    string s = "UPDATE cusfields SET cusfields.Value = '" + FormatFunctions.CleanDateNew(dataPair.Value.Text) + "',cusfields.Index='" + FormatFunctions.CleanDateNew(dataPair.Index.Text) + "' WHERE (IDKey= '" + dataPair.Index.GetInt() + "');";
                    batch.Add(s);
                }
            }
            string sql = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(noteLabel.Text) + "' WHERE cusfields.Index LIKE'%otes%' AND CusID= '" + customer + "'";
            batch.Add(sql);
            string sql2 = "UPDATE cusindex SET Name='" + FormatFunctions.CleanDateNew(nameLabel.Text) + "' WHERE IDKey= '" + customer + "'";
            batch.Add(sql2);
            string sql3 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(BookingDate.Text) + "' WHERE cusfields.Index LIKE '%ookin%' AND CusID= '" + customer + "'";
            batch.Add(sql3);
            string sql4 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(phoneLabel.Text) + "' WHERE cusfields.Index LIKE '%hone%' AND CusID= '" + customer + "'";
            batch.Add(sql4);
            DatabaseFunctions.SendBatchToPHP(batch);
        }
        public void onClickAddFields(object sender, RoutedEventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Value.ToolTip = "Index here";
            dataPair.Value.Width = ClientData.mainFrame.Width * 0.7 - 10;
            dataPair.Index.Text = "";
            dataPair.Index.ToolTip = "Value here";
            dataPair.Index.Width = ClientData.mainFrame.Width * 0.3 - 10;
            StackPanel stackLayout = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };
            stackLayout.Children.Add(dataPair.Index);
            stackLayout.Children.Add(dataPair.Value);
            BodyGrid.Children.Add(stackLayout);
            entryDict.Add(dataPair);
        }
        public void populateFileList()
        {
            string[] customerFileList = DatabaseFunctions.getCustomerFileList(nameLabel.Text);
            foreach (string text in customerFileList)
            {
                if ((text != "." || text != "..") && customerFileList.Length > 1)
                {
                    SecurityButton dataButton = new SecurityButton(nameLabel.Text + "/" + text, new string[] { "Employee" })
                    {
                        Content = text
                    };
                    dataButton.Click += onFileButton;
                    StackPanel stackLayout = new StackPanel();
                    stackLayout.Children.Add(dataButton);
                    BodyGrid.Children.Add(stackLayout);
                }
            }
        }
        public void onFileButton(object sender, RoutedEventArgs e)
        {
            string dir = @"\\CH-FILESERVER\Root\Files\Customer Files\CoolHeat Comfort Customer List\Residential Customers\" + address + @" - " + nameLabel.Text + @"\";
            System.IO.Directory.CreateDirectory(dir);
            Process.Start("explorer.exe", dir);
        }
    }
}
