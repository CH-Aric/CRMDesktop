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

namespace CRMDesktop.Pages
{
    public partial class Tasks_Page : Page
    {
        private Dictionary<string, List<string>> dict;
        private Dictionary<string, SecurityButton> buttonDict;
        private List<DataSwitch> switchDict;
        private Dictionary<string, List<string>> agents;
        private Dictionary<string, List<string>> groups;
        private bool gMode;
        public Tasks_Page()
        {
            InitializeComponent();
            this.loadFromDatabase();
        }
        private async void Clicked_Task(object sender, EventArgs e)
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        public void loadFromDatabase()
        {
            TaskCallback call = new TaskCallback(this.populateList);
            TaskCallback call2 = new TaskCallback(this.populateAgents);
            TaskCallback call3 = new TaskCallback(this.populateGroups);
            DatabaseFunctions.SendToPhp(false, string.Concat(new object[]
            {
                "SELECT * FROM Tasks WHERE AgentID='",
                ClientData.AgentIDK,
                "' OR GroupID IN(SELECT GroupID FROM crm2.groupmembers WHERE MemberID='",
                ClientData.AgentIDK,
                "');"
            }), call);
            DatabaseFunctions.SendToPhp(false, "SELECT FName,IDKey FROM agents WHERE Active='1';", call2);
            DatabaseFunctions.SendToPhp(false, "SELECT GroupName,GroupID FROM groupmembers WHERE MemberID='" + ClientData.AgentIDK + "';", call3);
        }
        public void populateList(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            this.dict = FormatFunctions.createValuePairs(input);
            if (this.dict.Count > 1)
            {
                this.buttonDict = new Dictionary<string, SecurityButton>();
                this.switchDict = new List<DataSwitch>();
                for (int i = 0; i < this.dict["Name"].Count; i++)
                {
                    if (!this.buttonDict.ContainsKey(this.dict["IDKey"][i]))
                    {
                        string text = this.dict["Name"][i] ?? "";
                        SecurityButton dataButton = new SecurityButton(int.Parse(this.dict["IDKey"][i]), new string[] { "Employee" })
                        {
                            Content = text,
                        };
                        DataSwitch item = new DataSwitch(int.Parse(this.dict["IDKey"][i]))
                        {
                        };
                        dataButton.Click += this.onClicked;
                        buttonDict.Add(this.dict["IDKey"][i], dataButton);
                        switchDict.Add(item);
                        List<UIElement> list = new List<UIElement>();
                        list.Add(dataButton);
                        list.Add(item);
                        GridFiller.rapidFillPremadeObjects(list, TSection, new bool[] { true, true });
                    }
                    else
                    {
                        SecurityButton dataButton2 = buttonDict[dict["IDKey"][i]];
                        dataButton2.Content = dataButton2.Content + ", " + dict["Value"][i];
                    }
                }
            }
        }
        public void populateAgents(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            agents = FormatFunctions.createValuePairs(input);
            agentPicker.ItemsSource = this.agents["FName"];
        }
        public void populateGroups(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            groups = FormatFunctions.createValuePairs(input);
        }
        public void onClicked(object sender, EventArgs e)
        {
            SecurityButton dataButton = (SecurityButton)sender;
            TaskEdit_Page page = new TaskEdit_Page(dataButton.Integer);
            ClientData.mainFrame.Navigate(page);
        }
        public void onClickedAssign(object sender, EventArgs e)
        {
            string text;
            if (!this.gMode)
            {
                int num = int.Parse(DatabaseFunctions.lookupInDictionary((string)agentPicker.SelectedItem, "FName", "IDKey", agents));
                text = "UPDATE tasks SET AgentID='" + num + "' , GroupID='0' WHERE";
                foreach (DataSwitch dataSwitch in switchDict)
                {
                    if (dataSwitch.hasChanged())
                    {
                        text = string.Concat(new object[]
                        {
                            text,
                            " IDKey='",
                            dataSwitch.GetInt(),
                            "' OR"
                        });
                    }
                }
                text += " IDKey='-1'";
            }
            else
            {
                int num = int.Parse(DatabaseFunctions.lookupInDictionary((string)agentPicker.SelectedItem, "GroupName", "GroupID", groups));
                text = "UPDATE tasks SET GroupID='" + num + "', AgentID='0' WHERE";
                foreach (DataSwitch dataSwitch2 in switchDict)
                {
                    if (dataSwitch2.hasChanged())
                    {
                        text = string.Concat(new object[]
                        {
                            text,
                            " IDKey='",
                            dataSwitch2.GetInt(),
                            "' OR"
                        });
                    }
                }
                text += " IDKey='-1'";
            }
            DatabaseFunctions.SendToPhp(text);
        }
        public void onClickedCreate(object sender, EventArgs e)
        {
            //TaskEdit_Page page = new TaskEdit_Page();
           // ClientData.mainFrame.Navigate(page);//TODO MAKE THIS WORK WITH TASKCREATE_PAGE WHEN ADDED
        }
        public void onClickedSearch(object sender, EventArgs e)
        {
            GridFiller.PurgeGrid(TSection);
            string text = "%" + SearchEntry.Text + "%";
            string statement = string.Concat(new object[]
            {
                "SELECT DISTINCT tasks.IDKey,tasks.Name FROM tasks INNER JOIN taskfields ON tasks.IDKey=taskfields.TaskID  WHERE tasks.AgentID='",
                ClientData.AgentIDK,
                "' AND ( tasks.Name LIKE '",
                text,
                "' OR taskfields.Value LIKE '",
                text,
                "');"
            });
            TaskCallback call = new TaskCallback(populateList);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void onToggledGroup(object sender, EventArgs e)
        {
            gMode = !gMode;
            if (gMode)
            {
                agentPicker.ToolTip = "Select Group";
                agentPicker.ItemsSource = groups["GroupName"];
                return;
            }
            agentPicker.ToolTip = "Select Agent";
            agentPicker.ItemsSource = agents["FName"];
        }
    }
}
