using CRMDesktop.Pages.Customers;
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

namespace CRMDesktop.Pages
{
    /// <summary>
    /// Interaction logic for Job_Page.xaml
    /// </summary>
    public partial class Job_Page : Page
    {
        int JobID;
        List<DataPair> dp;
        public Job_Page(int jobID)
        {
            InitializeComponent();
            JobID = jobID;
            string sql = "SELECT * FROM jobfields WHERE JobID='" + JobID + "'";
            TaskCallback call = populateGrid;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateGrid(string result)
        {
            dp = new List<DataPair>();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    DataPair d = new DataPair(int.Parse(dictionary["IDKey"][i]), dictionary["Index"][i], dictionary["Value"][i]);
                    List<UIElement> list = new List<UIElement>() { d.Index, d.Value };
                    GridFiller.rapidFillPremadeObjects(list, MainGrid, new bool[] { true, true });
                    dp.Add(d);
                }
            }
        }
        public void onClickSave(object sender, EventArgs e)
        {
            List<string> batch = new List<string>();
            foreach (DataPair dataPair in this.dp)
            {
                if (dataPair.isNew)
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[]
                    {
                        "INSERT INTO jobfields (jobfields.Value,jobfields.Index,JobID) VALUES('",
                        dataPair.Value.Text,
                        "','",
                        dataPair.Index.Text,
                        "','",
                        this.JobID,
                        "')"
                    }));
                    dataPair.isNew = false;
                }
                else if (dataPair.Index.Text != dataPair.Index.GetInit() || dataPair.Value.Text != dataPair.Value.GetInit())
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[] { "UPDATE jobfields SET Value = '", dataPair.Value.Text, "',Index='", dataPair.Index.Text, "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');" }));
                }
            }
        }
        public void onClickAddFields(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Index.Text = "";
            List<UIElement> list = new List<UIElement>() { dataPair.Index, dataPair.Value };
            GridFiller.rapidFillPremadeObjects(list, MainGrid, new bool[] { true, true });
            dp.Add(dataPair);
        }
        public void onClickAdvance(object sender, EventArgs e)
        {
            Advance_Page page = new Advance_Page(JobID);
            ClientData.mainFrame.Navigate(page);
        }
    }
}
