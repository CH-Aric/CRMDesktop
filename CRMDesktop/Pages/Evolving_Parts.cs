using CRMDesktop.Pages.Customers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        StackPanel holder, bodyHolder;
        TextBox Address,NameEntry,Phone,Email,Region,Source, Postal;
        DatePicker LastContact, FirstContact;
        Grid controlGrid, bodyGrid;
        Button Save, Add, Files;
        List<DataPair> dp;

        public int CusID;
        public Tombstone_Page(int cusID) : base()
        {
            Initialize();
            CusID = cusID;
            string sql = "SELECT cusindex.Name,cusfields.IDKey AS FID,cusindex.IDKey,cusfields.Value,cusfields.Index FROM cusfields INNER JOIN cusindex ON cusfields.CusID=cusindex.IDKey WHERE cusindex.IDKey='" + CusID + "';";
            TaskCallback call = TombstonePrinter;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }

        public void Initialize()
        {
            holder = new StackPanel();
            controlGrid = new Grid() { Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)) };
            Content = holder;
            holder.Children.Add(controlGrid);
            Header = "Customer Info";
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(7, GridUnitType.Star) });
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star) });


            Save = new Button() { Content="Save"};
            Add = new Button() { Content="Add New Job"};
            Files = new Button() { Content = "Customer Files" };
            Save.Click += onClickSave;
            Add.Click += onClickAddJob;
            Files.Click += onFileButton;
            Address = new TextBox();
            NameEntry = new TextBox();
            Phone = new TextBox();
            Email = new TextBox();
            Region = new TextBox();
            Source = new TextBox();
            FirstContact = new DatePicker();
            LastContact = new DatePicker();
            Postal = new TextBox();
            Label NameLabel = new Label() { Content = "Name:", HorizontalAlignment = HorizontalAlignment.Right };
            Label AddressLabel = new Label() { Content = "Address:", HorizontalAlignment = HorizontalAlignment.Right };
            Label PhoneLabel = new Label() { Content = "Phone:", HorizontalAlignment = HorizontalAlignment.Right };
            Label EmailLabel = new Label() { Content = "Email:", HorizontalAlignment = HorizontalAlignment.Right };
            Label RegionLabel = new Label() { Content = "Region:", HorizontalAlignment = HorizontalAlignment.Right };
            Label SourceLabel = new Label() { Content = "Source:", HorizontalAlignment = HorizontalAlignment.Right };
            Label FirstLabel = new Label() { Content = "First Contact:", HorizontalAlignment = HorizontalAlignment.Right };
            Label LastLabel = new Label() { Content = "Last Contact:", HorizontalAlignment = HorizontalAlignment.Right };
            Label PostalLabel = new Label() { Content = "Postal Code:", HorizontalAlignment = HorizontalAlignment.Right };
            List<UIElement> list = new List<UIElement>() { Add, Save};
            GridFiller.rapidFillSpacedPremadeObjects(list,controlGrid,new int[] { 2, 2}, new bool[] { false,false});
            list = new List<UIElement>() { Files};
            GridFiller.rapidFillSpacedPremadeObjects(list,controlGrid,new int[] { 1},new bool[] { false});
            list = new List<UIElement>() { NameLabel, NameEntry, AddressLabel, Address };
            GridFiller.rapidFillPremadeObjects(list,controlGrid,new bool[] { true,true,true,true});
            list = new List<UIElement>() { PhoneLabel,Phone,EmailLabel,Email};
            GridFiller.rapidFillPremadeObjects(list, controlGrid, new bool[] { true, true, true, true});
            list = new List<UIElement>() { RegionLabel,Region,SourceLabel,Source};
            GridFiller.rapidFillPremadeObjects(list, controlGrid, new bool[] { true, true, true, true});
            list = new List<UIElement>() { FirstLabel,FirstContact,LastLabel,LastContact};
            GridFiller.rapidFillPremadeObjects(list, controlGrid, new bool[] { true, true, true, true});
            list = new List<UIElement>() { PostalLabel, Postal };
            GridFiller.rapidFillPremadeObjects(list,controlGrid,new bool[] { true,true});
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
                    FirstContact.SelectedDate = DateTime.Parse(FormatFunctions.PrettyDate(dictionary["Value"][i]));
                }
                else if (dictionary["Index"][i].Contains("Last Contact"))
                {
                    LastContact.SelectedDate = DateTime.Parse(FormatFunctions.PrettyDate(dictionary["Value"][i]));
                }
                else if (dictionary["Index"][i].Contains("Source"))
                {
                    Source.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                }
                else if (dictionary["Index"][i].Contains("Postal"))
                {
                    Postal.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                }
            }
        }
        public void onClickAddJob(object sender, EventArgs e)
        {
            string sql = "INSERT INTO jobindex (CusID,Stage) VALUES ('"+CusID+"','0')";
            DatabaseFunctions.SendToPhp(sql);
        }
        public void onClickSave(object sender, EventArgs e)
        {
            string sql =  "UPDATE cusfields SET cusfields.Value='" + FormatFunctions.CleanDateNew(Address.Text) + "' WHERE cusfields.Index LIKE '%Address%' AND CusID='" + CusID + "'";
            string sql2 = "UPDATE cusfields SET cusfields.Value='" + FormatFunctions.CleanPhone(Phone.Text) + "' WHERE cusfields.Index LIKE '%Phone%' AND CusID='" + CusID + "'";
            string sql3 = "UPDATE cusfields SET cusfields.Value='" + FormatFunctions.CleanDateNew(Email.Text) + "' WHERE cusfields.Index LIKE '%Email%' AND CusID='" + CusID + "'";
            string sql4 = "UPDATE cusfields SET cusfields.Value='" + FormatFunctions.CleanDateNew(Region.Text) + "' WHERE cusfields.Index LIKE '%Region%' AND CusID='" + CusID + "'";
            string sql5 = "UPDATE cusfields SET cusfields.Value='" + FormatFunctions.CleanDateNew(LastContact.Text) + "' WHERE cusfields.Index LIKE '%ast Contac%' AND CusID='" + CusID + "'";
            string sql6 = "UPDATE cusindex SET Name='" + FormatFunctions.CleanDateNew(NameEntry.Text) + "' WHERE IDKey='" + CusID + "'";
            string sql7 = "UPDATE cusfields SET cusfields.Value='" + FormatFunctions.CleanDateNew(Source.Text) + "' WHERE cusfields.Index LIKE '%Source%' AND CusID='" + CusID + "'";
            string sql8 = "UPDATE cusfields SET cusfields.value='" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "' WHERE cusfields.Index LIKE '%odified On%' AND CusID= '" + CusID + "'";//'Modified On','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d h:mm:ss")) + "'
            string sql9 = "UPDATE cusfields SET cusfields.Value='" + FormatFunctions.CleanDateNew(Postal.Text) + "' WHERE cusfields.Index LIKE '%Postal%' AND CusID='" + CusID + "'";

            List<string> batch = new List<string>() { sql, sql2, sql3, sql4, sql5, sql6, sql7, sql8, sql9};
            DatabaseFunctions.SendBatchToPHP(batch);

            /*foreach (DataPair dataPair in dp)//dp must be initialized
            {
                if (dataPair.isNew)
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[]
                    {
                        "INSERT INTO cusfields (cusfields.Value,cusfields.Index,CusID) VALUES('",
                        dataPair.Value.Text,
                        "','",
                        dataPair.Index.Text,
                        "','",
                        this.CusID,
                        "')"
                    }));
                    dataPair.isNew = false;
                }
                else if (dataPair.Index.Text != dataPair.Index.GetInit() || dataPair.Value.Text != dataPair.Value.GetInit())
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[] { "UPDATE cusfields SET Value = '", dataPair.Value.Text, "',Index='", dataPair.Index.Text, "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');" }));
                }
            }*/
        }
        public void onClickAddFields(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Index.Text = "";
            List<UIElement> list = new List<UIElement>() { dataPair.Index, dataPair.Value };
            GridFiller.rapidFillPremadeObjects(list, bodyGrid, new bool[] { true, true });
            dp.Add(dataPair);
        }
        public void onFileButton(object sender, RoutedEventArgs e)
        {
            string dir = @"\\CH-FILESERVER\Root\Files\Customer Files\CoolHeat Comfort Customer List\Residential Customers\" + NameEntry.Text + @"\" + Address.Text + @"\";
            System.IO.Directory.CreateDirectory(dir);
            Process.Start("explorer.exe", dir);
        }
    }
    public class Job_Tab : TabItem
    {
        int JobID;
        Grid controlGrid,bodyGrid;
        Button Save, Add, Advance;
        Label Index, Value, NameLabel, StageLabel;
        TextBox NameBox;
        StackPanel holder;
        List<DataPair> dp;

        public Job_Tab(int job) :base()
        {
            InitializeComponent();
            JobID = job;
            string sql = "SELECT * FROM jobfields WHERE JobID='" + JobID + "'";
            TaskCallback call = populateGrid;
            DatabaseFunctions.SendToPhp(false, sql, call);
            string sql2 = "SELECT Name,Stage FROM jobindex WHERE IDKey='"+JobID+"'";
            TaskCallback call2 = populateName;
            DatabaseFunctions.SendToPhp(false, sql2, call2);
        }
        public void InitializeComponent()
        {
            holder = new StackPanel();
            controlGrid = new Grid();
            bodyGrid = new Grid();
            controlGrid = new Grid() { Background = new SolidColorBrush(Color.FromRgb(0, 0, 0))};
            bodyGrid = new Grid() { Background = new SolidColorBrush(Color.FromRgb(0, 0, 0))};//Create the grids
            holder.Children.Add(controlGrid);//Add the grids to the holder
            holder.Children.Add(bodyGrid);
            Content = holder;
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star)});
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star)});
            controlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star)});
            Add = new StyledButton() { Content="Add Field"};
            Add.Click += onClickAddFields;
            Save = new StyledButton() { Content = "Save Changes" };
            Save.Click += onClickSave;
            Advance = new StyledButton() { Content = "Advance" };
            Advance.Click += onClickAdvance;
            NameLabel = new Label() { Content = "Job Name:" };
            NameBox = new TextBox();
            StageLabel = new Label();
            List<UIElement> uIs = new List<UIElement>() { NameLabel,NameBox,StageLabel};
            GridFiller.rapidFillPremadeObjects(uIs,controlGrid,new bool[] { true,true,true});
            uIs =new List<UIElement>() { Add,Advance,Save};
            GridFiller.rapidFillPremadeObjects(uIs,controlGrid,new bool[] { false,false,false});
            Label l1=new Label(){ Content = "Index" };
            Label l2 = new Label() { Content = "Value" };
            uIs = new List<UIElement>() {l1,l2};
            GridFiller.rapidFillSpacedPremadeObjects(uIs,controlGrid,new int[] { 1, 2},new bool[] { true, true });

            bodyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star)});
            bodyGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(20, GridUnitType.Star)});
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
                    d.Index.Text = FormatFunctions.PrettyDate(dictionary["Index"][i]);
                    d.Value.Text = FormatFunctions.PrettyDate(dictionary["Value"][i]);
                    GridFiller.rapidFillPremadeObjects(list, bodyGrid, new bool[] { true, true });
                    dp.Add(d);
                }
            }
        }
        public void populateName(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                NameBox.Text = dictionary["Name"][0];
                StageLabel.Content="Stage: "+dictionary["Stage"][0];
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
                        FormatFunctions.CleanDateNew(dataPair.Value.Text),
                        "','",
                        FormatFunctions.CleanDateNew(dataPair.Index.Text),
                        "','",
                        this.JobID,
                        "')"
                    }));
                    dataPair.isNew = false;
                }
                else if (dataPair.Index.Text != dataPair.Index.GetInit() || dataPair.Value.Text != dataPair.Value.GetInit())
                {
                    DatabaseFunctions.SendToPhp(string.Concat(new object[] { "UPDATE jobfields SET jobfields.Value = '", FormatFunctions.CleanDateNew(dataPair.Value.Text), "',jobfields.Index='", FormatFunctions.CleanDateNew(dataPair.Index.Text), "' WHERE (IDKey= '", dataPair.Index.GetInt(), "');" }));
                }
            }
            string nsql = "UPDATE jobindex SET Name='"+NameBox.Text+"' WHERE IDKey='"+JobID+"'";
            DatabaseFunctions.SendToPhp(nsql);
        }
        public void onClickAddFields(object sender, EventArgs e)
        {
            DataPair dataPair = new DataPair(0, "", "");
            dataPair.setNew();
            dataPair.Value.Text = "";
            dataPair.Index.Text = "";
            List<UIElement> list = new List<UIElement>() { dataPair.Index, dataPair.Value };
            GridFiller.rapidFillPremadeObjects(list, bodyGrid, new bool[] { true, true });
            dp.Add(dataPair);
        }
        public void onClickAdvance(object sender, EventArgs e)
        {
            Advance_Page page = new Advance_Page(JobID);
            ClientData.mainFrame.Navigate(page);
        }
    }
}