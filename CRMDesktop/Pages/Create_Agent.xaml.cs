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
    /// Interaction logic for Create_Agent.xaml
    /// </summary>
    public partial class Create_Agent : Page
    {
        public List<int> groups;
        public Create_Agent()
        {
            InitializeComponent();
            getGroups();
        }
        public void onClicked(object sender, RoutedEventArgs e)
        {
            string sql = "INSERT INTO agents (FName,LName,AgentNum,Username,Password,Active, Cpoints,Apoints, Email) VALUES ('"+AgentName.Text+"','"+AgentLName.Text+"','"+AgentNum.Text+"','"+Username.Text+"','"+Password.Text+"','1','0','0','"+Email.Text+"')";
            DatabaseFunctions.SendToPhp(sql);
            string statement = "SELECT IDKey FROM agents ORDER BY IDKey DESC LIMIT 1;";
            TaskCallback call = secondaryData;
            DatabaseFunctions.SendToPhp(false, statement, call);
            MessageBox.Show("Agent Created Successfully!");
        }
        public void secondaryData(string result)
        {
            string text = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result))["IDKey"][0];
            if (GroupPicker.SelectedIndex>-1)
            {
                List<string> list = new List<string>();
                string sql2 = "INSERT INTO groupmembers (MemberID,GroupID,GroupName) VALUES (" + text + ",'" + groups[GroupPicker.SelectedIndex] + "','" + GroupPicker.SelectedItem + "')";
                list.Add(sql2);
                if (Role.SelectedIndex != 2)
                {
                    string sql3 = "INSERT INTO agentroles (AgentID,AgentRole) VALUES ('" + text + "','" + Role.SelectedIndex + "')";
                    list.Add(sql3);
                }
                DatabaseFunctions.SendBatchToPHP(list);
            }
        }
        public void getGroups()
        {
            string sql = "SELECT GroupName,GroupID FROM groupmembers WHERE Admin='1' AND MemberID='" + ClientData.AgentIDK + "';";
            TaskCallback call = populateGroups;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateGroups(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            GroupPicker.ItemsSource=dictionary["GroupName"];
            groups = new List<int>();
            if (dictionary.Count > 0)
            {
                foreach (string s in dictionary["GroupID"])
                {
                    groups.Add(int.Parse(s));
                }
            }
        }
    }
}