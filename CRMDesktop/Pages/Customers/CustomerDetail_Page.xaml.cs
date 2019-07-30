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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CRMDesktop.Pages.Customers
{
    /// <summary>
    /// Interaction logic for CustomerDetail_Page.xaml
    /// </summary>
    public partial class CustomerDetail_Page : Page
    {
        private List<DataPair> entryDict;
        private int customer;
        public CustomerDetail_Page(int i)
        {
            customer = i;
            InitializeComponent();
            TaskCallback call = new TaskCallback(this.populatePage);
            DatabaseFunctions.SendToPhp(false, "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + i + "';", call);
        }
        public void populatePage(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            entryDict = new List<DataPair>();
            NameDisplay.Content = dictionary["Name"][0];
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    DataPair dataPair = new DataPair(int.Parse(dictionary["FID"][i]), dictionary["value"][i], dictionary["Index"][i]);
                    dataPair.Value.Text = dictionary["value"][i];
                    dataPair.Value.ToolTip = "Value here";
                    dataPair.Index.Text = dictionary["Index"][i];
                    dataPair.Index.ToolTip = "Index here";
                    List<UIElement> list = new List<UIElement>() { dataPair.Index, dataPair.Value };
                    int[] space = new int[] { 2, 2 };
                    bool[] box = new bool[] { true, true };
                    GridFiller.rapidFillSpacedPremadeObjects(list, mainGrid, space, box);
                    this.entryDict.Add(dataPair);
                }
            }
            this.populateFileList();
        }
        public void populateFileList()
        {
            string[] customerFileList = DatabaseFunctions.getCustomerFileList(this.NameDisplay.Content.ToString());
            foreach (string text in customerFileList)
            {
                if ((text != "." || text != "..") && customerFileList.Length > 1)
                {
                    SecurityButton dataButton = new SecurityButton(this.NameDisplay.Content + "/" + text, new string[] { "Employee" })
                    {
                        Content = text
                    };
                    dataButton.Click += this.onFileButton;
                    GridFiller.rapidFillPremadeObjects(new List<UIElement>() { dataButton }, mainGrid, new bool[] { false });
                }
            }
        }
        public void onClicked(object sender, EventArgs e)
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
                else if (dataPair.Index.Text != dataPair.Index.GetInit())
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[]
                    {
                        "UPDATE cusfields SET Value = '",
                        dataPair.Value.Text,
                        "',Index='",
                        dataPair.Index.Text,
                        "' WHERE (IDKey= '",
                        dataPair.Index.GetInt(),
                        "');"
                    }));
                }
            }
        }
        public void onClickAddFields(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Value.ToolTip = "Index here";
            dataPair.Index.Text = "";
            dataPair.Index.ToolTip = "Value here";
            GridFiller.rapidFillPremadeObjects(new List<UIElement>() { dataPair.Index, dataPair.Value }, mainGrid, new bool[] { true, true });
            this.entryDict.Add(dataPair);
        }
        public void onFileButton(object sender, EventArgs e)
        {

        }
        public void onClickCDR(object sender, EventArgs e)
        {
            //App.MDP.Detail.Navigation.PushAsync(new CDR_Page(false, customer + ""));
            CDR_Page page = new CDR_Page(false, customer + "");
            ClientData.mainFrame.Navigate(page);
        }
        public void onBooking(object sender, EventArgs e)
        {
            //App.MDP.Detail.Navigation.PushAsync(new Booking_Page(customer));
            Booking_Page page = new Booking_Page(customer);
            ClientData.mainFrame.Navigate(page);
        }
    }
}
