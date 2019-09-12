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
    /// Interaction logic for PunchAdmin_Page.xaml
    /// </summary>
    public partial class PunchAdmin_Page : Page
    {

        List<string> agents;
        public PunchAdmin_Page()
        {
            InitializeComponent();
            populate();

        }

        public void onClick(object sender,RoutedEventArgs e)
        {
            string sql = "SELECT * FROM punchclock WHERE AgentID='"+agents[Agent.SelectedIndex]+"' AND ("+ getRelevantDates()+ ")";

        }
        public void populate()
        {
            string sql2 = "SELECT agents.FName,agents.IDKey FROM agents WHERE Active='1'";
            TaskCallback call3 = populateSalesCombo;
            DatabaseFunctions.SendToPhp(false, sql2, call3);
        }
        public void populateSalesCombo(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            Agent.ItemsSource = dictionary["FName"];
            agents = dictionary["IDKey"];
        }
        public void populateStamps(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for(int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    string[] y = new string[4];
                    y[0] = dictionary["TimeStamp"][i];
                    y[1] = dictionary["Coordinates"][i];
                    y[2] = dictionary["Note"][i];
                    y[3] = dictionary["State"][i];
                    GridFiller.rapidFill(y, BodyGrid);
                }
            }
        }
        public string getRelevantDates()
        {
            DateTime d =(DateTime) DayPicker.SelectedDate;
            string r = "TimeStamp LIKE '%"+DayPicker.SelectedDate+"%'";
            for(int i = 0; i < 6; i++)
            {
                d.AddDays(1);
                r += " OR TimeStamp LIKE '%" + d + "%'";
            }
            return r;
        }
    }
}
