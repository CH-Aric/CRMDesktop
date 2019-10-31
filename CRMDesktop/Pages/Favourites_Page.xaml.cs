using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CRMDesktop.Pages
{
    public partial class Favourites_Page : Page
    {
        private List<DataSwitch> Favorites;
        private List<DataSwitch> Group;
        private IDictionary<string, List<string>> pickerIndex;
        public Favourites_Page()
        {
            InitializeComponent();
            this.Favorites = new List<DataSwitch>();
            this.Group = new List<DataSwitch>();
            this.getAgents();
            this.getFavorites();
            this.getGroups();
            scroll.Height = ClientData.mainFrame.Height-TopBar.Height;
        }
        public void getAgents()
        {
            string statement = "SELECT FName,IDKey FROM agents;";
            TaskCallback call = new TaskCallback(this.populateList);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void populateList(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            for (int i = 0; i < dictionary["FName"].Count; i++)
            {
                string text = dictionary["FName"][i] ?? "";
                Label item = new Label
                {
                    Content = (text ?? ""),
                };
                DataSwitch item2 = new DataSwitch(int.Parse(dictionary["IDKey"][i]))
                {
                };
                DataSwitch item3 = new DataSwitch(int.Parse(dictionary["IDKey"][i]))
                {

                };
                List<UIElement> list = new List<UIElement>() { item, item2, item3 };
                GridFiller.rapidFillPremadeObjects(list, TSection, new bool[]{ true,true,true});
                Favorites.Add(item2);
                Group.Add(item3);
            }
        }
        public void getFavorites()
        {
            string statement = "SELECT TargetID FROM chatfavorite WHERE AgentID='" + ClientData.AgentIDK + "'";
            TaskCallback call = new TaskCallback(this.populateFavorites);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void populateFavorites(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["TargetID"].Count; i++)
                {
                    DatabaseFunctions.findDataSwitchinList(this.Favorites, int.Parse(dictionary["TargetID"][i])).setStartState(true);
                }
            }
        }
        public void getGroups()
        {
            string statement = "SELECT GroupName,GroupID FROM groupmembers WHERE Admin='1' AND MemberID='" + ClientData.AgentIDK + "';";
            TaskCallback call = new TaskCallback(this.populateGroups);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void populateGroups(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            this.pickerIndex = FormatFunctions.createValuePairs(input);
            GroupSelector.ItemsSource = this.pickerIndex["GroupName"];
            GroupSelector.SelectedItem = 1;
        }
        public void onClickAddToGroup(object sender, RoutedEventArgs e)
        {
            string text = "INSERT INTO groupmembers (GroupName,GroupID,MemberID) VALUES ";
            int num = 0;
            foreach (DataSwitch dataSwitch in this.Group)
            {
                if (dataSwitch.getState())
                {
                    num++;
                    text+="('"+ this.GroupSelector.SelectedItem+ "','"+this.pickerIndex["GroupID"][this.GroupSelector.SelectedIndex]+ "','"+ dataSwitch.GetInt()+ "'), ";
                }
            }
            if (num > 0)
            {
                text = text.TrimEnd(new char[]
                {
                    ' '
                });
                text = text.TrimEnd(new char[]
                {
                    ','
                });
                DatabaseFunctions.SendToPhp(text);
            }
        }
        public void onClickRemoveFromGroup(object sender, RoutedEventArgs e)
        {
            string text = "DELETE FROM groupmembers WHERE ";
            int num = 0;
            foreach (DataSwitch dataSwitch in this.Group)
            {
                if (dataSwitch.getState())
                {
                    num++;
                    text = string.Concat(new object[]
                    {
                        text,
                        "(GroupID='",
                        this.pickerIndex["GroupID"][this.GroupSelector.SelectedIndex],
                        "' AND MemberID='",
                        dataSwitch.GetInt(),
                        "') OR "
                    });
                }
            }
            if (num > 0)
            {
                text += "GroupID='-1'";
                DatabaseFunctions.SendToPhp(text);
            }
        }
        public void onClickCreateGroup(object sender, RoutedEventArgs e)
        {
            string statement = "SELECT GroupID FROM groupmembers ORDER BY GroupID DESC LIMIT 1";
            TaskCallback call = new TaskCallback(this.createGroup);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void createGroup(string result)
        {
            int num = int.Parse(FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result))["GroupID"][0]) + 1;
            DatabaseFunctions.SendToPhp(string.Concat(new object[]
            {
                "INSERT INTO groupmembers (MemberID,Admin,GroupName,GroupID) VALUES ('",
                ClientData.AgentIDK,
                "','1','",
                GroupEntry.Text,
                "','",
                num,
                "');"
            }));
        }
        public void onClickDeleteGroup(object sender, RoutedEventArgs e)
        {
            DatabaseFunctions.SendToPhp(string.Concat(new object[]
            {
                "DELETE FROM groupmembers WHERE (GroupID='",
                this.pickerIndex["GroupID"][this.GroupSelector.SelectedIndex],
                "' AND MemberID='",
                ClientData.AgentIDK,
                "')"
            }));
        }
        public void onClickSaveFavorite(object sender, RoutedEventArgs e)
        {
            string text = "INSERT INTO chatfavorite (TargetID,AgentID) VALUES ";
            string text2 = "DELETE FROM chatfavorite WHERE ";
            int num = 0;
            int num2 = 0;
            foreach (DataSwitch dataSwitch in Favorites)
            {
                if (dataSwitch.getState() && dataSwitch.hasChanged())
                {
                    num++;
                    text = string.Concat(new object[]
                    {
                        text,
                        "('",
                        dataSwitch.GetInt(),
                        "','",
                        ClientData.AgentIDK,
                        "'), "
                    });
                }
                else if (!dataSwitch.getState() && dataSwitch.hasChanged())
                {
                    num2++;
                    text2 = string.Concat(new object[]
                    {
                        text2,
                        "(TargetID='",
                        dataSwitch.GetInt(),
                        "' AND AgentID='",
                        ClientData.AgentIDK,
                        "') OR "
                    });
                }
            }
            if (num > 0)
            {
                text = text.TrimEnd(new char[]
                {
                    ' '
                });
                text = text.TrimEnd(new char[]
                {
                    ','
                });
                DatabaseFunctions.SendToPhp(text);
            }
            if (num2 > 0)
            {
                text2 += " AgentID=-1";
                DatabaseFunctions.SendToPhp(text2);
            }
        }
    }
}
