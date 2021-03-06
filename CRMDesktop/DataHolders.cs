﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CRMDesktop
{
    public class SecurityButton : DataButton
    {
        private string[] Locks;
        public SecurityButton(int i, string[] locks)
        {
            Locks = locks;
            Integer = i;
            SecurityAlpha();
        }
        public SecurityButton(string i, string[] locks)
        {
            Locks = locks;
            String = i;
            SecurityAlpha();
        }
        private bool checkKeys()
        {
            int i = 0;
            foreach (string s in Locks)
            {
                if (ClientData.hasSecurityKey(s))
                {
                    i++;
                }
            }
            if (i == Locks.Length)
            {
                return true;
            }
            return false;
        }
        private void SecurityAlpha()
        {
            if (!checkKeys())
            {
                IsEnabled = false;
                Content = "Unauthorized, Please see your supervisor";
            }
        }
    }
    public class DataButton : StyledButton
    {
        public DataButton()
        {

        }
        public DataButton(int i)
        {
            this.Integer = i;
        }
        public DataButton(string i)
        {
            this.String = i;
        }
        public int GetInt()
        {
            return this.Integer;
        }
        public int GetInt2()
        {
            return this.Integer2;
        }
        public string GetString()
        {
            return this.String;
        }
        public string GetString2()
        {
            return this.String2;
        }
        public int Integer;
        public int Integer2;
        public string String;
        public string String2;
    }
    public class StyledButton : Button
    {
        public StyledButton()
        {
            base.Background = new SolidColorBrush(Color.FromRgb(198, 248, 255));
            base.BorderBrush = Brushes.Black;
            base.BorderThickness = new Thickness(2);
            base.Foreground = Brushes.Black;
        }
    }
    public class DataEntry : TextBox
    {
        public DataEntry(int i, string s)
        {
            this.Integer = i;
            this.InitValue = s;
        }
        public int GetInt()
        {
            return this.Integer;
        }
        public string GetInit()
        {
            return this.InitValue;
        }
        public int Integer;
        public string InitValue;
    }
    public class FlaggedDataPair :DataPair
    {
        public int Flag=0;
        public FlaggedDataPair(int i, string I,string V, int j) : base(i, I, V)
        {
            Flag = j;
        }
    }
    public class DataPair
    {
        public DataPair(int i, string I, string V)
        {
            this.Index = new DataEntry(i, I);
            this.Value = new DataEntry(i, V);
        }
        public void setNew()
        {
            this.isNew = true;
        }
        public DataEntry Index;
        public DataEntry Value;
        public bool isNew;
    }
    public class DataDoubleSwitch : DataSwitch
    {
        public DataDoubleSwitch(int i, int j) :base(i)
        {
            Integer2 = j;
        }
        public int getSecondInt()
        {
            return Integer2;
        }
        public int Integer2;
    }
    public class DataSwitch : CheckBox
    {
        public DataSwitch(int i)
        {
            this.Integer = i;
        }
        public int GetInt()
        {
            return this.Integer;
        }
        public void setStartState(bool b)
        {
            base.IsChecked = b;
            this.StartState = b;
        }
        public bool hasChanged()
        {
            return base.IsChecked != this.StartState;
        }
        public bool getState()
        {
            return (bool)IsChecked;
        }
        public int Integer;
        public bool StartState;
    }
    public class AwesomeHyperLinkLabel : Label
    {

    }
    public delegate void TaskCallback(string Result);
    public class RadioButtonCore : INotifyPropertyChanged
    {
        public RadioButtonCore()
        {
            MyList = new ObservableCollection<RadioButtonDataHandler>();
            FillData();
        }
        public IList<RadioButtonDataHandler> MyList { get; set; }
        private RadioButtonDataHandler _selectedItem;
        public RadioButtonDataHandler SelectedItem
        {
            get => _selectedItem;
            set { _selectedItem = value; OnPropertyChanged(); }
        }

        void FillData()
        {
            for (int i = 0; i < 6; i++)
            {
                MyList.Add(new RadioButtonDataHandler { Id = i, Name = "Option " + i });
            }
        }

        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string propName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        #endregion
    }
    public class RadioButtonDataHandler
    {
        public int Id { get; set; }
        public string Name { get; set; }
        //Override string and return what you want to be displayed
        public override string ToString() => Name;
    }
    public class salesAgentPicker : ComboBox
    {
        List<string> agentNames;
        List<string> agentNums;
        public salesAgentPicker() : base()
        {
            string sql = "SELECT agents.FName,agents.IDKey FROM agents INNER JOIN agentroles ON agents.IDKey=agentroles.AgentID AND agentroles.AgentRole='0' AND agents.Active='1'";
            TaskCallback call = loadAgents;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void loadAgents(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            agentNames = dictionary["FName"];
            agentNums = dictionary["IDKey"];
        }
        public int getSelectID()
        {
            return int.Parse(agentNums[DatabaseFunctions.findIndexInList(agentNames, SelectedItem.ToString())]);
        }
    }
    public class AgentPicker : ComboBox
    {
        List<string> agentNames;
        List<string> agentNums;
        public AgentPicker() : base()
        {
            string sql = "SELECT FName,IDKey FROM agents WHERE Active='1'";
            TaskCallback call = loadAgents;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void loadAgents(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            agentNames = dictionary["FName"];
            agentNums = dictionary["IDKey"];
        }
        public int getSelectID()
        {
            return int.Parse(agentNums[DatabaseFunctions.findIndexInList(agentNames, SelectedItem.ToString())]);
        }
    }
}
