using CRMDesktop.Pages.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CRMDesktop.Pages
{
    class Evolving_Parts
    {

    }
    public class Tombstone_Page : TabItem
    {
        StackPanel holder;
        TextBox Address,NameEntry,Phone,Email,Region,Source;
        Label FirstContact;
        DatePicker LastContact;
        Grid controlGrid;

        public int CusID;
        public Tombstone_Page(int cusID) : base()
        {
            InitializeComponents();
            CusID = cusID;
            string sql = "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.cusID=cusindex.IDKey WHERE cusfields.CusID='" + CusID + "';";
            TaskCallback call = TombstonePrinter;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void InitializeComponents()
        {
            holder.Children.Add(controlGrid);
            controlGrid = new Grid() { Background=new SolidColorBrush(Color.FromRgb(0,0,0))};
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });

            Address = new TextBox();
            NameEntry = new TextBox();
            Phone = new TextBox();
            Email = new TextBox();
            Region = new TextBox();
            Source = new TextBox();
            FirstContact = new Label();
            LastContact = new DatePicker();
            Label NameLabel = new Label() { Content = "Name:", HorizontalAlignment = HorizontalAlignment.Right };
            Label AddressLabel = new Label() { Content = "Address:", HorizontalAlignment = HorizontalAlignment.Right };
            Label PhoneLabel = new Label() { Content = "Phone:", HorizontalAlignment = HorizontalAlignment.Right };
            Label EmailLabel = new Label() { Content = "Email:", HorizontalAlignment = HorizontalAlignment.Right };
            Label RegionLabel = new Label() { Content = "Region:", HorizontalAlignment = HorizontalAlignment.Right };
            Label SourceLabel = new Label() { Content = "Source:", HorizontalAlignment = HorizontalAlignment.Right };
            Label FirstLabel = new Label() { Content = "First Contact:", HorizontalAlignment = HorizontalAlignment.Right };
            Label LastLabel = new Label() { Content = "Last Contact:", HorizontalAlignment = HorizontalAlignment.Right };
            List<UIElement> list = new List<UIElement>() { NameLabel,NameEntry,AddressLabel,Address};
            GridFiller.rapidFillPremadeObjects(list,controlGrid,new bool[] { true,true,true,true});
            list = new List<UIElement>() { PhoneLabel,Phone,EmailLabel,Email};
            GridFiller.rapidFillPremadeObjects(list, controlGrid, new bool[] { true, true, true, true });
            list = new List<UIElement>() { RegionLabel,Region,SourceLabel,Source };
            GridFiller.rapidFillPremadeObjects(list, controlGrid, new bool[] { true, true, true, true });
            list = new List<UIElement>() { FirstLabel,FirstContact,LastLabel,LastContact};
            GridFiller.rapidFillPremadeObjects(list, controlGrid, new bool[] { true, true, true, true });
        }
        public void TombstonePrinter(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            NameEntry.Text = FormatFunctions.PrettyDate(dictionary["Name"][0]);
            for (int i = 0; i < dictionary["Name"].Count; i++)
            {
                if (dictionary["Index"][i].Contains("Address"))
                {
                    Address.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("Phone"))
                {
                    Phone.Text = FormatFunctions.PrettyPhone(dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("Email"))
                {
                    Email.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("Region"))
                {
                    Region.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("First Contact"))
                {
                    FirstContact.Content = FormatFunctions.PrettyDate("First Contact: " + dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("Last Contact"))
                {
                    LastContact.SelectedDate = DateTime.Parse(FormatFunctions.PrettyDate(dictionary["Value"][i]));
                }
                else if (dictionary["Index"][i].Contains("Source"))
                {
                    Source.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                }
            }
        }
        public void onClickAddJob(object sender, EventArgs e)
        {
            string sql = "INSERT INTO jobindex ";
        }
        public void onClickSave(object sender, EventArgs e)
        {
            string sql = "UPDATE cusfields SET Value='" + FormatFunctions.CleanDateNew(Address.Text) + "' WHERE Index LIKE '%Address%' AND CusID='" + CusID + "'";
            string sql2 = "UPDATE cusfields SET Value='" + FormatFunctions.CleanDateNew(Phone.Text) + "' WHERE Index LIKE '%Phone%' AND CusID='" + CusID + "'";
            string sql3 = "UPDATE cusfields SET Value='" + FormatFunctions.CleanDateNew(Email.Text) + "' WHERE Index LIKE '%Email%' AND CusID='" + CusID + "'";
            string sql4 = "UPDATE cusfields SET Value='" + FormatFunctions.CleanDateNew(Region.Text) + "' WHERE Index LIKE '%Region%' AND CusID='" + CusID + "'";
            string sql5 = "UPDATE cusfields SET Value='" + FormatFunctions.CleanDateNew(LastContact.SelectedDate.ToString()) + "' WHERE Index LIKE '%Address%' AND CusID='" + CusID + "'";
            string sql6 = "UPDATE cusindex SET Name='" + NameEntry.Text + "' WHERE IDKey='" + CusID + "'";
            string sql7 = "UPDATE cusfields SET Value='" + FormatFunctions.CleanDateNew(Source.Text) + "' WHERE Index LIKE '%Source%' AND CusID='" + CusID + "'";
            string sql8 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "' WHERE cusfields.Index LIKE '%odified On%' AND CusID= '" + CusID + "'";//'Modified On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "'

            List<string> batch = new List<string>() { sql, sql2, sql3, sql4, sql5, sql6, sql7, sql8};
            DatabaseFunctions.SendBatchToPHP(batch);
        }
    }
    public class Job_Tab : TabItem
    {
        public Grid MainGrid;
        int JobID;
        Grid controlGrid,bodyGrid;
        Button Save, Add, Advance;
        Label Index, Value;
        StackPanel holder;
        List<DataPair> dp;
        public Job_Tab(int job) :base()
        {
            InitializeComponent();
            JobID = job;
            string sql = "SELECT * FROM jobfields WHERE JobID='" + JobID + "'";
            TaskCallback call = populateGrid;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void InitializeComponent()
        {
            holder.Children.Add(controlGrid);
            holder.Children.Add(bodyGrid);
            controlGrid = new Grid() { Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)) };
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });
            controlGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(25, GridUnitType.Pixel) });
            Add = new StyledButton() { Content="Add Field"};
            Add.Click += onClickAddFields;
            Save = new StyledButton() { Content = "Save Changes" };
            Save.Click += onClickSave;
            Advance = new StyledButton() { Content = "Save Changes" };
            Advance.Click += onClickAdvance;
            List<UIElement> uIs=new List<UIElement>() { Add,Advance,Save};
            GridFiller.rapidFillPremadeObjects(uIs,controlGrid,new bool[] { false,false,false});
            Label l1=new Label(){ Content = "Index" };
            Label l2 = new Label() { Content = "Value" };
            uIs = new List<UIElement>() { l1,l2};
            GridFiller.rapidFillSpacedPremadeObjects(uIs,controlGrid,new int[] { 0, 1},new bool[] { true, true });

            bodyGrid = new Grid() { Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)) };
            bodyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
            bodyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });
        }
        public void populateGrid(string result)
        {
            dp = new List<DataPair>();
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    DataPair d = new DataPair(int.Parse(dictionary["IDKey"][i]), dictionary["Index"][i], dictionary["Value"][i]);
                    List<UIElement> list = new List<UIElement>() { d.Index, d.Value };
                    GridFiller.rapidFillPremadeObjects(list, MainGrid, new bool[] { true, true });
                    dp.Add(d);
                }
            }
        }
        public void onClickSave(object sender, EventArgs e)
        {
            foreach (DataPair dataPair in this.dp)
            {
                if (dataPair.isNew)
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[]
                    {
                        "INSERT INTO jobfields (jobfields.Value,jobfields.Index,JobID) VALUES('",
                        dataPair.Value.Text,
                        "','",
                        dataPair.Index.Text,
                        "','",
                        this.JobID,
                        "')"
                    }));
                    dataPair.isNew = false;
                }
                else if (dataPair.Index.Text != dataPair.Index.GetInit() || dataPair.Value.Text != dataPair.Value.GetInit())
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[] { "UPDATE jobfields SET Value = '", dataPair.Value.Text, "',Index='", dataPair.Index.Text, "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');" }));
                }
            }
        }
        public void onClickAddFields(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Index.Text = "";
            List<UIElement> list = new List<UIElement>() { dataPair.Index, dataPair.Value };
            GridFiller.rapidFillPremadeObjects(list, MainGrid, new bool[] { true, true });
            dp.Add(dataPair);
        }
        public void onClickAdvance(object sender, EventArgs e)
        {
            Advance_Page page = new Advance_Page(JobID);
            ClientData.mainFrame.Navigate(page);
        }
    }
}
