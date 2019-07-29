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
    public partial class TaskEdit_Page : Page
    {
        private List<DataPair> entryDict;
        private int task;
        public TaskEdit_Page(int i)
        {
            this.task = i;
            InitializeComponent();
            TaskCallback call = new TaskCallback(this.populateFields);
            DatabaseFunctions.SendToPhp(false, "SELECT * FROM taskfields WHERE TaskID='" + i + "';", call);
        }
        public void populateFields(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            this.entryDict = new List<DataPair>();
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    DataPair dataPair = new DataPair(int.Parse(dictionary["IDKey"][i]), dictionary["Value"][i], dictionary["Index"][i]);
                    dataPair.Value.Text = dictionary["Value"][i];
                    dataPair.Value.ToolTip = "Value here";
                    dataPair.Index.Text = dictionary["Index"][i];
                    dataPair.Index.ToolTip = "Index here";
                    this.entryDict.Add(dataPair);

                    List<UIElement> list = new List<UIElement>();
                    list.Add(dataPair.Value);
                    list.Add(dataPair.Index);
                    GridFiller.rapidFillPremadeObjects(list, TSection, new bool[] { true, true });
                }
            }
        }
        public void onClicked(object sender, EventArgs e)
        {
            foreach (DataPair dataPair in this.entryDict)
            {
                if (dataPair.isNew)
                {
                    DatabaseFunctions.SendToPhp(
                        "INSERT INTO taskfields (taskfields.Value,taskfields.Index,TaskID) VALUES('" + dataPair.Value.Text + "','" + dataPair.Index.Text + "','" + this.task + "')");
                    dataPair.isNew = false;
                }
                else if (dataPair.Index.Text != dataPair.Index.GetInit())
                {
                    DatabaseFunctions.SendToPhp("UPDATE taskfields SET Value = '" + dataPair.Value.Text + "',Index='" + dataPair.Index.Text + "' WHERE (IDKey= '" + dataPair.Index.GetInt() + "');");
                }
            }
        }
        public void onClickAddFields(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Value.ToolTip = "Index here";
            dataPair.Index.Text = "";
            dataPair.Index.ToolTip = "Value here";
            List<UIElement> list = new List<UIElement>();
            list.Add(dataPair.Value);
            list.Add(dataPair.Index);
            GridFiller.rapidFillPremadeObjects(list, TSection, new bool[] { true, true });
            this.entryDict.Add(dataPair);
        }
    }
}
