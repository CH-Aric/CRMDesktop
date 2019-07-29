using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace CRMDesktop.Pages
{
    /// <summary>
    /// Interaction logic for Chat_Page.xaml
    /// </summary>
    public partial class Chat_Page : Page
    {
        private Dictionary<string, List<string>> pickerIndex;
        public Chat_Page()
        {
            InitializeComponent();
            getChatMessages();
            pickerIndex = new Dictionary<string, List<string>>();
            pickerIndex.Add("Name", new List<string>
            {
                "All"
            });
            pickerIndex.Add("g", new List<string>
            {
                "1"
            });
            pickerIndex.Add("IDKey", new List<string>
            {
                "0"
            });
            getFavoriteAgents();
            getFavoriteGroups();
        }
        public void getChatMessages()
        {
            string statement = string.Concat(new object[]
            {
                "SELECT chat.Message,chat.Timestamp,agents.FName FROM chat INNER JOIN agents ON chat.AgentID=agents.IDKey WHERE chat.TargetID = '",
                ClientData.AgentIDK,
                "' OR chat.AgentID = '",
                ClientData.AgentIDK,
                "' OR 'Chat.Global' = '1' OR('chat.Global' = '2' AND chat.TargetID IN(SELECT IDKey FROM groupmembers WHERE MemberID = '",
                ClientData.AgentIDK,
                "')); "
            });
            TaskCallback call = new TaskCallback(this.populateChat);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void getFavoriteAgents()
        {
            string statement = "SELECT agents.Fname AS Name,agents.IDKey, '0' AS g FROM agents INNER JOIN chatfavorite ON agents.IDKey=chatfavorite.TargetID WHERE chatfavorite.AgentID='" + ClientData.AgentIDK + "';";
            TaskCallback call = new TaskCallback(this.populatePicker);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void getFavoriteGroups()
        {
            string statement = "SELECT IDKey,GroupName AS Name, '2' AS g FROM groupmembers WHERE MemberID='" + ClientData.AgentIDK + "';";
            TaskCallback call = new TaskCallback(this.populatePicker);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void populateChat(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Message"].Count; i++)
                {
                    Label item = new Label
                    {
                        Content = string.Concat(new string[]
                        {
                            dictionary["Timestamp"][i],
                            ":",
                            dictionary["FName"][i],
                            ":",
                            dictionary["Message"][i]
                        })
                    };
                    ChatStack.Children.Add(item);
                }
            }
        }
        public void populatePicker(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            if (pickerIndex == null)
            {
                pickerIndex = FormatFunctions.createValuePairs(input);
            }
            else
            {
                Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(input);
                foreach (string key in dictionary.Keys)
                {
                    pickerIndex[key] = pickerIndex[key].Union(dictionary[key]).ToList<string>();
                }
            }
            Target.ItemsSource = pickerIndex["Name"];
        }
        public void OnClickSendMsg(object sender, RoutedEventArgs e)
        {
            int num = int.Parse(DatabaseFunctions.lookupInDictionary((string)Target.SelectedItem, "Name", "IDKey", this.pickerIndex));
            int num2 = int.Parse(DatabaseFunctions.lookupInDictionary((string)Target.SelectedItem, "Name", "g", this.pickerIndex));
            string text = DateTime.Now.ToString("yyyy'-'MM'-'dd HH':'mm':'ss");
            string sql = "INSERT INTO chat (Message,AgentID,TargetID,Global,Timestamp) VALUES ('" + Message.Text + "','" + ClientData.AgentIDK + "','" + num + "','" + num2 + "','" + text + "')";
            DatabaseFunctions.SendToPhp(sql);
            this.getChatMessages();
        }
        public void OnClickMan(object sender, RoutedEventArgs e)
        {
            Favourites_Page page = new Favourites_Page();
            ClientData.mainFrame.Navigate(page);
        }
    }
}