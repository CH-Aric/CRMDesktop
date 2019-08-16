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
    /// Interaction logic for RoleAssignment_Page.xaml
    /// </summary>
    public partial class RoleAssignment_Page : Page
    {
        List<UIElement> checkBoxes;
        List<Label> Labels;
        List<String> uniqueIDs;
           List<String> uniqueNames;
        public RoleAssignment_Page()
        {
            InitializeComponent();
            searchAgents();
        }
        public void searchAgents()
        {
            TaskCallback call1 = setupBoxes;
            string sql1 = "SELECT FName,IDKey FROM agents";
            DatabaseFunctions.SendToPhp(false, sql1, call1);
            TaskCallback call = populate;
            string sql = "SELECT AgentRole,AgentID,AgentRole FROM agentroles";
            DatabaseFunctions.SendToPhp(false,sql,call);
        }
        public void populate(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            List<DataDoubleSwitch> xlist = new List<DataDoubleSwitch>();
            foreach (UIElement x in checkBoxes)
            {
                xlist.Add((DataDoubleSwitch)x);
            }
            if (dictionary.Count>0)
            {
                for (int j = 0; j < dictionary["AgentRole"].Count; j++)
                {
                    int i = int.Parse(dictionary["AgentRole"][j]);
                    DatabaseFunctions.findDDataSwitchinList(xlist, int.Parse(dictionary["AgentID"][j]), i).setStartState(true);
                    
                }
            }
        }
        public void setupBoxes(string result)
        {
            Labels = new List<Label>();
            checkBoxes = new List<UIElement>();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            uniqueIDs = dictionary["IDKey"];
            uniqueNames = dictionary["FName"];
            foreach (string s in uniqueIDs)
            {//Possible roles are: Sales, Installer
                List<UIElement> elements = new List<UIElement>();
                elements.Add(new Label() { Content = s });
                elements.Add(new DataDoubleSwitch(int.Parse(s), 0) { HorizontalAlignment=HorizontalAlignment.Center});//Sales
                elements.Add(new DataDoubleSwitch(int.Parse(s), 1) { HorizontalAlignment = HorizontalAlignment.Center });//Installer
                checkBoxes.AddRange(elements);
                GridFiller.rapidFillPremadeObjects(elements, BodyGrid, new bool[] { true, false, false });
            }
            checkBoxes = FormatFunctions.scrubOutUnlessWanted(checkBoxes, new DataDoubleSwitch(0, 0), Labels);
            for (int j = 0; j < uniqueNames.Count; j++)
            {
                Labels[j].Content = uniqueNames[j];
            }
        }
        public void onClickSave(object sender,RoutedEventArgs e)
        {
            string text = "INSERT INTO agentroles (AgentRole,AgentID) VALUES ";
            string text2 = "DELETE FROM agentroles WHERE ";
            int num = 0;
            int num2 = 0;
            foreach (DataDoubleSwitch dd in checkBoxes)
            {
                if (dd.getState() && dd.hasChanged())
                {
                    num++;
                    text += "('" + dd.getSecondInt() + "','" + dd.GetInt() + "'), ";
                }
                else if (!dd.getState() && dd.hasChanged())
                {
                    num2++;
                    text2 += "(TargetID='" + dd.getSecondInt() + "' AND AgentID='" + dd.GetInt() + "') OR ";
                }
            }
            if (num > 0)
            {
                text = text.TrimEnd(new char[] { ' ' });
                text = text.TrimEnd(new char[]  {  ','});
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
