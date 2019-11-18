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
    /// Interaction logic for TaskCreate_Page.xaml
    /// </summary>
    public partial class TaskCreate_Page : Page
    {
        private List<DataEntry> Names;
        private List<DataEntry> Values;
        private Dictionary<string, List<string>> templates;
        int highestTID=0;
        public TaskCreate_Page()
        {
            InitializeComponent();
            this.loadFromDatabase();
            this.Values = new List<DataEntry>();
            this.Names = new List<DataEntry>();
        }
        public void loadFromDatabase()
        {
            TaskCallback call = new TaskCallback(this.populateTemplates);
            string statement = "SELECT Name,IDKey FROM tasktemplates";
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void populateTemplates(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            this.templates = FormatFunctions.createValuePairs(input);
            if (templates.Count > 0)
            {
                this.templatePicker.ItemsSource = this.templates["Name"];
            }
            highestTID=int.Parse(templates["IDKey"][templates["IDKey"].Count-1]);
        }
        public void populateFields(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            for (int i = 0; i < dictionary["IDKey"].Count; i++)
            {
                DataEntry item = new DataEntry(int.Parse(dictionary["IDKey"][i]), dictionary["Value"][i])
                {
                    Text = FormatFunctions.PrettyDate(dictionary["Value"][i])
                };
                DataEntry item2 = new DataEntry(int.Parse(dictionary["IDKey"][i]), dictionary["Index"][i])
                {
                    Text = FormatFunctions.PrettyDate(dictionary["Index"][i])
                };
                List<UIElement> list = new List<UIElement>() { item, item2 };
                int[] j = new int[] { 3, 3 };
                GridFiller.rapidFillSpacedPremadeObjects(list, gridStack, j, new bool[] { true, true });
                this.Values.Add(item);
                this.Names.Add(item2);
            }
        }
        public void onClickedSaveTemplate(object sender, RoutedEventArgs e)
        {
            DatabaseFunctions.SendToPhp("INSERT INTO tasktemplates (Name,Description,IDKey) VALUES ('"+templateName.Text+"','"+descField.Text+"','"+highestTID+1+"');");
            string statement = "SELECT IDKey FROM tasktemplates ORDER BY IDKey DESC LIMIT 1;";
            TaskCallback call = saveTemplateFields;
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void saveTemplateFields(string result)
        {
            string text = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result))["IDKey"][0];
            string text2 = "INSERT INTO taskfields (taskfields.Index,taskfields.Value,TemplateID) VALUES ";
            int ID = highestTID + 1;
            for (int i = 0; i < this.Names.Count; i++)
            {
                if (!string.IsNullOrEmpty(this.Names[i].Text))
                {
                    string stripped1 = FormatFunctions.CleanDateNew(Names[i].Text);
                    string stripped2 = FormatFunctions.CleanDateNew(Values[i].Text);
                    text2 = text2+ "('"+ stripped1+  "', '"+ stripped2+ "', '"+ ID+ "'),";
                }
            }
            text2 = text2 + "('', '', '" + ID + "')";
            DatabaseFunctions.SendToPhp(text2);
            Tasks_Page page = new Tasks_Page();
            ClientData.mainFrame.Navigate(page);
        }
        public void onClickedLoadTemplate(object sender, RoutedEventArgs e)
        {
            int num = int.Parse(DatabaseFunctions.lookupInDictionary((string)this.templatePicker.SelectedItem, "Name", "IDKey", this.templates));
            string statement = "SELECT taskfields.Value,taskfields.Index,IDKey FROM taskfields WHERE TemplateID='" + num + "';";
            TaskCallback call = new TaskCallback(this.populateFields);
            DatabaseFunctions.SendToPhp(false, statement, call);
        }
        public void onClickedCreate(object sender, RoutedEventArgs e)
        {
                
                DatabaseFunctions.SendToPhp(string.Concat(new object[]
                {
                    "INSERT INTO tasks (Name,AgentID) VALUES ('",
                    FormatFunctions.CleanDateNew(taskName.Text),
                    "','",
                    ClientData.AgentIDK,
                    "');"
                }));
                DatabaseFunctions.SendToPhp(false, "SELECT IDKey FROM tasks ORDER BY IDKey DESC LIMIT 1;", new TaskCallback(this.saveTaskFields));
            
        }
        public void saveTaskFields(string result)
        {
            string text = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result))["IDKey"][0];
            string text2 = "INSERT INTO taskfields (taskfields.Index,taskfields.Value,TaskID) VALUES ";
            for (int i = 0; i < this.Names.Count; i++)
            {
                if (!string.IsNullOrEmpty(this.Names[i].Text))
                {
                    string stripped1 = FormatFunctions.CleanDateNew(Names[i].Text);
                    string stripped2 = FormatFunctions.CleanDateNew(Values[i].Text);
                    text2 = string.Concat(new string[]
                    {
                        text2,
                        "('",
                        stripped1,
                        "', '",
                        stripped2,
                        "', '",
                        text,
                        "'),"
                    });
                }
            }
            text2 = text2 + "('', '', '" + text + "')";
            DatabaseFunctions.SendToPhp(text2);
            Tasks_Page page = new Tasks_Page();
            ClientData.mainFrame.Navigate(page);
        }
        public void onClickAddFields(object sender, RoutedEventArgs e)
        {
            DataEntry item = new DataEntry(0, "")
            {
                Text = "",
                ToolTip = "Value here"
            };
            DataEntry item2 = new DataEntry(0, "")
            {
                Text = "",
                ToolTip = "Name here"
            };

            List<UIElement> list = new List<UIElement>() { item, item2 };
            int[] j = new int[] { 3, 3 };
            GridFiller.rapidFillSpacedPremadeObjects(list, gridStack, j, new bool[] { true, true });
            this.Values.Add(item);
            this.Names.Add(item2);
        }
    }
}
