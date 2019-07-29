using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for CDR_Page.xaml
    /// </summary>
    public partial class CDR_Page : Page
    {
        public CDR_Page(bool Searchmode, string Searchfor)
        {

            this.InitializeComponent();
            if (!Searchmode)
            {
                SearchEntry.Text = Searchfor;
                SearchEntry.IsEnabled = false;
                SearchButton.IsEnabled = false;
                string text = "SELECT DISTINCT cusfields.Value FROM cusfields WHERE cusfields.Index LIKE '%Phone%' AND ( CusID = '" + Searchfor + "');";
                TaskCallback call = new TaskCallback(this.performSearch3);
                DatabaseFunctions.SendToPhp(false, text, call);
            }
            scroll.Height = ClientData.FrameHeight - 102;
        }
        public void performSearch2(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            string text = "SELECT DISTINCT cusfields.Value FROM cusfields WHERE cusfields.Index LIKE '%Phone%' AND (cusfields.Value='-14' ";
            foreach (string str in dictionary["CusID"])
            {
                text = text + "OR CusID = '" + str + "' ";
            }
            text += ");";
            TaskCallback call = new TaskCallback(this.performSearch3);
            DatabaseFunctions.SendToPhp(false, text, call);
        }
        public void performSearch3(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            string text = "SELECT uniqueid,cnum,cnam,disposition,duration,did,recordingfile,src,calldate FROM asteriskcdrdb.cdr WHERE ";
            foreach (string phone in dictionary["Value"])
            {
                string text2 = FormatFunctions.CleanPhone(phone);
                text = text + "dst LIKE'%" + text2 + "%' OR src LIKE'%" + text2 + "%' OR clid LIKE'%" + text2 + "%' OR cnam LIKE'%" + text2 + "%' OR cnum LIKE'%" + text2 + "%' OR outbound_cnam LIKE'%" + text2 + "%' OR outbound_cnum LIKE'%" + text2 + "%' OR ";
            }
            text = Regex.Replace(text, "[-()]", "");
            text += "src='-14' ORDER BY calldate DESC LIMIT 500;";
            TaskCallback call = new TaskCallback(this.populateResults);
            DatabaseFunctions.SendToPhp(true, text, call);
        }
        public void populateResults(string result)
        {
            this.PurgeCells();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result), true);
            if (dictionary.Count > 1)
            {
                for (int i = 0; i < dictionary["uniqueid"].Count; i++)
                {
                    string text = dictionary["calldate"][i] + " : " + FormatFunctions.PrettyPhone(dictionary["cnam"][i]);
                    SecurityButton dataButton = new SecurityButton(int.Parse(Regex.Replace(dictionary["uniqueid"][i], "^[^.]+.", "")), new string[] { "Manager" })
                    {
                        Content = text
                    };
                    dataButton.Click += onClicked;
                    dataButton.String = dictionary["calldate"][i];
                    dataButton.String2 = dictionary["recordingfile"][i];
                    List<UIElement> list = new List<UIElement>
                    {
                        dataButton
                    };
                    GridFiller.rapidFillPremadeObjects(list, TSection, new bool[] { true });
                }
            }
        }
        public void openFile(string result)
        {

        }
        public async void onClicked(object sender, RoutedEventArgs e)
        {
            SecurityButton dataButton = (SecurityButton)sender;
            TaskCallback call = new TaskCallback(this.openFile);
            string loadedfile = DatabaseFunctions.getFile(dataButton.String, dataButton.String2, call);
            //await PopupNavigation.Instance.PushAsync(new Audio_Popup(loadedfile), true); //TODO Make this work in Mobile
        }
        public void onClickedSearch(object sender, RoutedEventArgs e)
        {
            string statement = "SELECT DISTINCT CusID FROM cusfields WHERE Value LIKE '%" + this.SearchEntry.Text + "%'";
            TaskCallback call = new TaskCallback(this.performSearch2);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void onClickedExplicitySearch(object sender, RoutedEventArgs e)
        {
            string text = "SELECT uniqueid,cnum,cnam,disposition,duration,did,recordingfile,src,calldate FROM asteriskcdrdb.cdr WHERE ";
            string text2 = this.SearchEntry.Text;
            text = text + "dst LIKE'%" + text2 + "%' OR src LIKE'%" + text2 + "%' OR clid LIKE'%" + text2 + "%' OR cnam LIKE'%" + text2 + "%' OR cnum LIKE'%" + text2 + "%' OR outbound_cnam LIKE'%" + text2 + "%' OR outbound_cnum LIKE'%" + text2 + "%' LIMIT 500;";
            text = Regex.Replace(text, "[-()]", "");
            TaskCallback call = new TaskCallback(this.populateResults);
            DatabaseFunctions.SendToPhp(true, text, call);
        }
        public void PurgeCells()
        {
            GridFiller.PurgeGrid(TSection);
        }
    }
}
