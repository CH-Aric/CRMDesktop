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
            DateTime d = (DateTime)DayPicker.SelectedDate;
            string sql = "SELECT * FROM punchclock WHERE AgentID='"+agents[Agent.SelectedIndex]+"' AND ("+ FormatFunctions.getRelevantDates(d)+ ")";
            if ((bool)!LocCheck.IsChecked)
            {
                sql += " AND (State='True' OR State='False')";
            }
            TaskCallback call = populateStamps;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populate()
        {
            string sql2 = "SELECT agents.FName,agents.IDKey FROM agents WHERE Active='1'";
            TaskCallback call3 = populateSalesCombo;
            DatabaseFunctions.SendToPhp(false, sql2, call3);
        }
        public void populateLiveFeed(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                string[] s = new string[] { dictionary["FName"][0], convertState(dictionary["State"][0]), dictionary["Coordinates"][0], FormatFunctions.PrettyDate(dictionary["TimeStamp"][0]), FormatFunctions.getAgeOfTimestamp(FormatFunctions.PrettyDate(dictionary["TimeStamp"][0])), dictionary["Note"][0]};
                GridFiller.rapidFill(s,LiveBody);
            }
        }
        public void populateSalesCombo(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            Agent.ItemsSource = dictionary["FName"];
            agents = dictionary["IDKey"];
            Agent2.ItemsSource = dictionary["FName"];
            Agent3.ItemsSource = dictionary["FName"];
            //Use agents list to populate other pages!
            foreach (string s in agents)
            {
                string sql3 = "SELECT agents.FName,punchclock.State,punchclock.Coordinates,punchclock.TimeStamp,punchclock.Note FROM agents INNER JOIN punchclock ON agents.IDKey = punchclock.AgentID WHERE punchclock.AgentID='"+s+"' ORDER BY punchclock.IDKey DESC";
                TaskCallback call2 = populateLiveFeed;
                DatabaseFunctions.SendToPhp(false, sql3, call2);
            }
        }
        public void populateStamps(string result)
        {
            GridFiller.PurgeHeader(BodyGrid);
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            List<string> clockin = new List<string>();
            List<string> clockout = new List<string>();
            if (dictionary.Count > 0)
            {
                for(int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    string[] y = new string[4];
                    y[0] = FormatFunctions.PrettyDate(dictionary["TimeStamp"][i]);
                    y[1] = FormatFunctions.PrettyDate(dictionary["Coordinates"][i]);
                    y[2] = FormatFunctions.PrettyDate(dictionary["Note"][i]);
                    y[3] = convertState(dictionary["State"][i]);
                    if (dictionary["State"][i] == "True")
                    {
                        GridFiller.rapidFillColorized(y,BodyGrid,true);
                        clockin.Add(FormatFunctions.PrettyDate(dictionary["TimeStamp"][i]));
                    }
                    else if (dictionary["State"][i] == "False")
                    {
                        GridFiller.rapidFillColorized(y, BodyGrid, false);
                        clockout.Add(FormatFunctions.PrettyDate(dictionary["TimeStamp"][i]));
                    }
                    else
                    {
                        GridFiller.rapidFill(y, BodyGrid);
                    }
                }
                double x = calculateHours(clockin, clockout) / 60;
                HourDisplay.Content = "Total Hours: " + x;
                AgentDisplay.Content = "Viewing: " + Agent.SelectedValue;
                WeekDisplay.Content = "Week of: " + DayPicker.SelectedDate;
            }
        }
        public double calculateHours(List<string> starts,List<string> ends)
        {
            double output = 0;
            List<DateTime> s = new List<DateTime>();
            List<DateTime> e = new List<DateTime>();
            foreach (string i in starts)
            {
                s.Add(Convert.ToDateTime(i));
            }
            foreach (string i in ends)
            {
                e.Add(Convert.ToDateTime(i));
            }
            for(int i = 0; i < Math.Min(e.Count, s.Count); i++)
            {
                System.TimeSpan diff = e[i].Subtract(s[i]);
                output += diff.TotalMinutes;
            }
            return output;
        }
        public string convertState(string intake)
        {
            if (intake == "True")
            {
                return "Punched In";
            }
            else if (intake == "False")
            {
                return "Punched Out";
            }
            return "";
        }
        //Start of Pending Tab Code
        public void onClickPend(object sender, RoutedEventArgs e)
        {
            TaskCallback call = populatePend;
            DateTime d = (DateTime)DayPicker2.SelectedDate;
            string sql = "SELECT * FROM punchclock WHERE AgentID='" + agents[Agent2.SelectedIndex] + "' AND (" + FormatFunctions.getRelevantDates(d) + ")";
            if ((bool)Pending.IsChecked)
            {
                sql += " AND Approved != '1'";
            }
            DatabaseFunctions.SendToPhp(false,sql,call);
        }
        public void populatePend(string result)
        {
            GridFiller.PurgeHeader(DetailBody);
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for(int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    DataButton b = new DataButton();
                    b.Content = "Approve";
                    b.Integer = int.Parse(dictionary["IDKey"][i]);
                    b.Integer2 = int.Parse(dictionary["Approved"][i]);
                    if (dictionary["Approved"][i] == "1")
                    {
                        b.Background = new SolidColorBrush(ClientData.rotatingConfirmationColors[0]);
                        b.Content = "Disapprove";
                    }
                    else
                    {
                        b.Background = new SolidColorBrush(ClientData.rotatingNegativeColors[0]);
                    }
                    b.Click += onClickApprove;
                    DataButton b2 = new DataButton();
                    b2.Content = "Add Note";
                    b2.Integer = int.Parse(dictionary["IDKey"][i]);
                    b2.Click += onClickANote;
                    Label T = new Label();
                    T.Content = FormatFunctions.PrettyDate(dictionary["TimeStamp"][i]);
                    Label L = new Label();
                    L.Content = FormatFunctions.PrettyDate(dictionary["Coordinates"][i]);
                    Label I = new Label();
                    I.Content = convertState(dictionary["State"][i]);
                    Label N = new Label();
                    N.Content = FormatFunctions.PrettyDate(dictionary["Note"][i]);
                    List<UIElement> list = new List<UIElement>() { b,b2,T,L,I,N};
                    GridFiller.rapidFillPremadeObjects(list, DetailBody, new bool[] { false, false, true, true, true, true });
                }
            }
        }
        public void onClickApprove(object sender, RoutedEventArgs e)
        {
            DataButton b = (DataButton)sender;
            b.Background= ClientData.getGridColorCC(true);
            string sql;
            if (b.GetInt2()== 1)
            {
                b.Background = new SolidColorBrush(ClientData.rotatingConfirmationColors[0]);
                b.Content = "Approve";
                b.Integer2 = 0;
                sql = "UPDATE punchclock SET Approved='0' WHERE IDKey='"+b.GetInt()+"'";
            }
            else
            {
                b.Content = "Disapprove";
                b.Background = new SolidColorBrush(ClientData.rotatingNegativeColors[0]);
                b.Integer2 = 1;
                sql = "UPDATE punchclock SET Approved='1' WHERE IDKey='" + b.GetInt() + "'";
            }
            DatabaseFunctions.SendToPhp(sql);
        }
        public void onClickANote(object sender, RoutedEventArgs e)
        {
            DataButton b = (DataButton)sender;
            string sql = "UPDATE punchclock SET AdminNote='"+AdminNote.Text+"' WHERE IDKey='"+b.GetInt()+"'";
            DatabaseFunctions.SendToPhp(sql);
            b.Background = new SolidColorBrush(Color.FromRgb(213, 213, 213));
        }
        //Start of Tardiness tab code
        public void onClickTardy(object sender, RoutedEventArgs e)
        {
            populateTardiGridBypass();
        }
        public void populateTardiGridBypass()
        {
            TaskCallback call = populateTardiGrid;
            string sql = "SELECT * FROM agentrecords WHERE AgentID='" + agents[Agent3.SelectedIndex] + "'";
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateTardiGrid(string result)
        {
            GridFiller.PurgeHeader(TardiGrid);
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            int sick = 0;
            int tardy = 0;
            if (dictionary["IDKey"].Count > 0)
            {
                for(int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    GridFiller.rapidFill(new string[] { dictionary["RecordType"][i], dictionary["Date"][i], dictionary["Note"][i] }, TardiGrid);
                    if (dictionary["RecordType"][i] == "Sick")
                    {
                        sick++;
                    }else if (dictionary["RecordType"][i] == "Tardy")
                    {
                        tardy++;
                    }
                }
            }
            Sick.Content = "Sick Days: " + sick;
            Late.Content = "Late Days: " + tardy;
        }
        public void onClickSick(object sender, RoutedEventArgs e)
        {
            string sql = "INSERT INTO agentrecords (AgentID,Note,Date,RecordType) VALUES ('"+agents[Agent3.SelectedIndex]+"','"+AdminNote.Text+"','"+ DayPicker3.SelectedDate.Value.ToString("yyyy/M/d") + "','Sick')";
            DatabaseFunctions.SendToPhp(sql);
            populateTardiGridBypass();
        }
        public void onClickLate(object sender, RoutedEventArgs e)
        {
            string sql = "INSERT INTO agentrecords (AgentID,Note,Date,RecordType) VALUES ('" + agents[Agent3.SelectedIndex] + "','" + AdminNote.Text + "','" + DayPicker3.SelectedDate.Value.ToString("yyyy/M/d") + "','Tardy')";
            DatabaseFunctions.SendToPhp(sql);
            populateTardiGridBypass();
        }
    }
}