﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRMDesktop.Pages.Customers
{
    /// <summary>
    /// Interaction logic for Quote_Page.xaml
    /// </summary>
    public partial class Quote_Page : Page
    {
        private List<DataPair> entryDict, entryDictQ;
        string address;
        private int customer;
        private int stage;
        public Quote_Page(int customerIn, int stageIn)
        {
            customer = customerIn;
            stage = stageIn;
            InitializeComponent();
            searchCustomers();
            populateFileList();
        }
        public void searchCustomers()
        {
            TaskCallback call2 = populatePage;
            DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + customer + "' AND cusfields.Index <> 'QUOTEFIELD';", call2);
            TaskCallback call3 = populateQuoteList;
            DatabaseFunctions.SendToPhp(false, "SELECT cusfields.IDKey,cusfields.Value,cusfields.AdvValue FROM cusfields WHERE cusfields.CusID='" + customer + "' AND cusfields.Index='QUOTEFIELD';", call3);
        }
        public void onClickAdvance(object sender, EventArgs e)
        {
            Advance_Page page = new Advance_Page(customer);
            ClientData.mainFrame.Navigate(page);
        }
        public void populatePage(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            entryDict = new List<DataPair>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    if (dictionary["Index"][i].Contains("hone"))
                    {
                        phoneLabel.Content = dictionary["Value"][i];
                    }
                    else
                    {
                        DataPair dataPair = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["Index"][i], dictionary["Value"][i]);
                        dataPair.Value.Text = dictionary["Value"][i];
                        dataPair.Value.ToolTip = "Value here";
                        dataPair.Index.Text = dictionary["Index"][i];
                        dataPair.Index.ToolTip = "Index here";
                        List<UIElement> list = new List<UIElement>() { dataPair.Index, dataPair.Value };
                        int[] j = new int[] { 3, 3 };
                        GridFiller.rapidFillSpacedPremadeObjects(list, bottomStack, j, new bool[] { true, true });
                        entryDict.Add(dataPair);
                    }
                }
            }
        }
        public void populateFileList()
        {
            string[] customerFileList = DatabaseFunctions.getCustomerFileList(nameLabel.Text);
            foreach (string text in customerFileList)
            {
                if ((text != "." || text != "..") && customerFileList.Length > 1)
                {
                    SecurityButton dataButton = new SecurityButton(nameLabel.Text + "/" + text, new string[] { "Sales" })
                    {
                        Content = text
                    };
                    dataButton.Click += onFileButton;
                    List<UIElement> list = new List<UIElement>() { dataButton };
                    GridFiller.rapidFillPremadeObjects(list, fileGrid, new bool[] { true, true });
                }
            }
        }
        public void populateQuoteList(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            entryDictQ = new List<DataPair>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Value"].Count; i++)
                {
                    DataPair dataPair = new DataPair(0, dictionary["Value"][i], dictionary["AdvValue"][i]);
                    dataPair.Value.Text = dictionary["Value"][i];
                    dataPair.Value.ToolTip = "Item";
                    dataPair.Index.Text = dictionary["AdvValue"][i];
                    dataPair.Index.ToolTip = "Amount";
                    List<UIElement> list = new List<UIElement>() { dataPair.Index, dataPair.Value };
                    int[] j = new int[] { 3, 3 };
                    GridFiller.rapidFillSpacedPremadeObjects(list, quoteStack, j, new bool[] { true, true });
                    entryDictQ.Add(dataPair);
                }
            }
        }
        public void onClicked(object sender, EventArgs e)
        {
            foreach (DataPair dataPair in entryDict)
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
                    DatabaseFunctions.SendToPhp(string.Concat(new object[] { "UPDATE cusfields SET Value = '", dataPair.Value.Text, "',Index='", dataPair.Index.Text, "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');" }));
                }
            }
            string sql = "DELETE FROM cusfields WHERE CusID='" + customer + "' AND cusfields.Index='QUOTEFIELD'";
            DatabaseFunctions.SendToPhp(sql);
            foreach (DataPair dp in entryDictQ)
            {
                if (dp.Value.Text != "" && dp.Index.Text != "")
                {
                    string sql2 = "INSERT INTO cusfields(cusfields.Value,cusfields.Index,CusID,cusfields.AdvValue) VALUES ('" + dp.Value.Text + "','QUOTEFIELD','" + customer + "','" + dp.Index.Text + "')";
                    DatabaseFunctions.SendToPhp(sql2);
                }
            }
            Quote_Page page = new Quote_Page(customer, stage);
            ClientData.mainFrame.Navigate(page);
        }
        public void onClickAddFields(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Value.ToolTip = "Index here";
            dataPair.Index.Text = "";
            dataPair.Index.ToolTip = "Value here";

            List<UIElement> list = new List<UIElement>() { dataPair.Index, dataPair.Value };
            int[] i = new int[] { 3, 3 };
            GridFiller.rapidFillSpacedPremadeObjects(list, bottomStack, i, new bool[] { true, true });
            entryDictQ.Add(dataPair);
            entryDict.Add(dataPair);
            Button x = (Button)sender;
            if (x != row)
            {
                if (x == sig)
                {
                    dataPair.Index.Text = "Signature";
                    dataPair.Value.Text = "True";
                }
                if (x == fie)
                {
                    dataPair.Index.Text = "Deposit Received";
                    dataPair.Value.Text = "True";
                }
                if (x == met)
                {
                    dataPair.Index.Text = "Payment Method";
                }
            }
        }
        public void onClickAddFieldsQ(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");//TODO REWORK with Picker from the pricing guide!
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Value.ToolTip = "Item";
            dataPair.Index.Text = "";
            dataPair.Index.ToolTip = "Amount";
            List<UIElement> list = new List<UIElement>() { dataPair.Index,dataPair.Value};
            int[] i = new int[] { 3,3};
            GridFiller.rapidFillSpacedPremadeObjects(list,quoteStack,i,new bool[]{ true,true});
            entryDictQ.Add(dataPair);
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
