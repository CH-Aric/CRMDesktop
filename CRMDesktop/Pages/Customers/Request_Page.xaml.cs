using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CRMDesktop.Pages.Customers
{
    /// <summary>
    /// Interaction logic for Request_Page.xaml
    /// </summary>
    public partial class Request_Page : Page
    { 
          private List<DataPair> entryDict;
         string address;
         private int customer;
         public Request_Page(int customerIn)
         {
             customer = customerIn;
             InitializeComponent();
             searchCustomers();
            scroll.Height = ClientData.mainFrame.Height -60;
         }
         public void searchCustomers()
         {
             TaskCallback call2 = populatePage;
             DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + customer + "';", call2);
         }
        public void onClickAdvance(object sender, RoutedEventArgs e)
        {
            Advance_Page page = new Advance_Page(customer);
            ClientData.mainFrame.Navigate(page);
        }
        public void populatePage(string result)
         {
             Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
             entryDict = new List<DataPair>();
            nameLabel.Text = dictionary["Name"][0];
             if (dictionary.Count > 0)
             {
                 for (int i = 0; i < dictionary["Index"].Count; i++)
                 {
                     if (dictionary["Index"][i].Contains("hone"))
                     {
                         phoneLabel.Text = dictionary["value"][i];
                     }
                     else if (dictionary["Index"][i].Contains("OTES"))
                     {
                         address = dictionary["value"][i];
                         noteLabel.Text += dictionary["value"][i];
                     }
                     else
                     {
                        DataPair dataPair = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["value"][i], dictionary["Index"][i]);
                        dataPair.Value.Text = dictionary["value"][i];
                        dataPair.Value.ToolTip = "Value here";
                        dataPair.Value.Width = ClientData.mainFrame.Width *0.7-10;
                        dataPair.Index.Text = dictionary["Index"][i];
                        dataPair.Index.ToolTip = "Index here";
                        dataPair.Index.Width = ClientData.mainFrame.Width *0.3-10;
                        StackPanel stackLayout = new StackPanel
                        {
                            Orientation=Orientation.Horizontal
                        };
                        stackLayout.Children.Add(dataPair.Index);
                        stackLayout.Children.Add(dataPair.Value);
                        TSection.Children.Add(stackLayout);
                        entryDict.Add(dataPair);
                     }
                 }
             }
             populateFileList();
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
                     TSection.Children.Add(stackLayout);
                 }
             }
         }
         public void onClicked(object sender, RoutedEventArgs e)
         {
             foreach (DataPair dataPair in this.entryDict)
             {
                 if (dataPair.isNew)
                 {
                     DatabaseFunctions.SendToPhp(string.Concat(new object[]
                     {
                             "INSERT INTO cusfields (cusfields.Value,cusfields.Index,CusID) VALUES('",
                             dataPair.Value.Text,
                             "','",
                             dataPair.Index.Text,
                             "','",
                             this.customer,
                             "')"
                     }));
                     dataPair.isNew = false;
                 }
                 else if (!dataPair.Index.Text.Equals(dataPair.Index.GetInit()))
                 {
                     DatabaseFunctions.SendToPhp(string.Concat(new object[] { "UPDATE cusfields SET cusfields.Value = '", dataPair.Value.Text, "',cusfields.Index='", dataPair.Index.Text, "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');" }));
                 }
             }
             string sql = "UPDATE cusfields SET cusfields.value='" + noteLabel.Text + "' WHERE cusfields.Index='Notes:' AND IDKey= '" + customer + "'";
             DatabaseFunctions.SendToPhp(sql);
            string sql2 = "UPDATE cusindex SET Name='"+nameLabel.Text+ "' WHERE IDKey= '"+customer+ "'";
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
             TSection.Children.Add(stackLayout);
             entryDict.Add(dataPair);
         }
         public void onFileButton(object sender, EventArgs e)
         {

         }
         public void onClickCDR(object sender, EventArgs e)
         {
            CDR_Page page = new CDR_Page(false, customer + "");
            ClientData.mainFrame.Navigate(page);
         }
    }
}
