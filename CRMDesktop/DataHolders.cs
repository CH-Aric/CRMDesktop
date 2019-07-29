using System;
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
}
