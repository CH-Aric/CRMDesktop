using System.Collections.Generic;
using System.Windows.Controls;

namespace CRMDesktop.Pages.Customers
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class Evolving_Page : Page
    {
        List<Job_Tab> JobSheets;
        int CusID;

        public Evolving_Page(int customer)
        {
            InitializeComponent();
            CusID = customer;
            Tombstone_Page tombPage = new Tombstone_Page(CusID);
            Tab.Items.Add(tombPage);
            string sql2 = "SELECT * FROM jobindex WHERE CusID='" + CusID + "'";
            TaskCallback call2 = populateSheetNames;
            DatabaseFunctions.SendToPhp(false, sql2, call2);
        }
        public void populateSheetNames(string result)
        {
            JobSheets = new List<Job_Tab>();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            for (int i = 0; i < dictionary["IDKey"].Count; i++)
            {
                Job_Tab x = new Job_Tab(int.Parse(dictionary["IDKey"][i]));
                Label b = new Label() { Content=dictionary["Name"][i] };
                x.Header = b;
                Tab.Items.Add(x);
                JobSheets.Add(x);
            }
        }
    }
}
