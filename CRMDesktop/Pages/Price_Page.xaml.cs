using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CRMDesktop.Pages
{
    /// <summary>
    /// Interaction logic for Price_Page.xaml
    /// </summary>
    public partial class Price_Page : Page
    {
        Dictionary<int, List<UIElement>> EditDict;
        public Price_Page()
        {
            InitializeComponent();
            string sql = "SELECT DISTINCT(Brand) FROM pricesheet";
            TaskCallback call = populateBrands;
            DatabaseFunctions.SendToPhp(false, sql, call);
            scroll.Height = ClientData.FrameHeight-102;
        }
        public void onClicked(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM pricesheet WHERE Brand LIKE '%" + BrandPicker.SelectedItem + "%' AND ItemType LIKE '%" + TypePicker.SelectedItem + "%'";
            if (BrandPicker.SelectedIndex == -1)
            {
                sql += " AND Brand !='Additives'";
            }
            TaskCallback call = populateSearch;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void onClickedSave(object sender, RoutedEventArgs e)
        {

        }
        public void populateBrands(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            BrandPicker.ItemsSource = dictionary["Brand"];
            List<string> TypeSource = new List<string>() { "A/C","Furnace","Water Tank","Tankless"};
            TypePicker.ItemsSource = TypeSource;
        }
        public void populateSearch(string result)
        {
            PurgeCells();
            printHeader();
            TSection.Background = new SolidColorBrush(Color.FromArgb(255,0,0,0));
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                EditDict = new Dictionary<int, List<UIElement>>();
                for (int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    if (!(bool)EditMode.IsChecked)
                    {
                        string[] s = new string[] { dictionary["Desc1"][i], dictionary["Desc2"][i], "$" + dictionary["Price"][i], "$" + dictionary["PriceSale"][i] };
                        int count = 4;
                        if (BrandPicker.SelectedIndex == -1 || !BrandPicker.SelectedItem.Equals("Additives"))
                        {
                            count++;
                            s = new string[] { dictionary["Brand"][i], s[0], s[1], s[2], s[3] };
                        }
                        if (TypePicker.SelectedIndex == -1)
                        {
                            count++;
                            if (count == 6)
                            {
                                s = new string[] { dictionary["ItemType"][i], s[0], s[1], s[2], s[3], s[4] };
                            }
                            else
                            {
                                s = new string[] { dictionary["ItemType"][i], s[0], s[1], s[2], s[3] };
                            }
                        }
                        int[] Spacing = new int[] { 1, 1, 1, 1, 1, 1 };
                        if (s.Length == 5)
                        {
                            Spacing = new int[] { 1, 2, 1, 1, 1 };
                        }
                        else if (s.Length == 4)
                        {
                            Spacing = new int[] { 2, 2, 1, 1 };
                        }
                        GridFiller.rapidFillSpaced(s, TSection, Spacing);
                    }
                    else
                    {
                        int[] Spacing = new int[] { 1, 1, 1, 1, 1, 1 };
                        DataEntry Desc1 = new DataEntry(int.Parse(dictionary["IDKey"][i]), dictionary["Desc1"][i]) { Text = dictionary["Desc1"][i] };
                        DataEntry Desc2 = new DataEntry(int.Parse(dictionary["IDKey"][i]), dictionary["Desc2"][i]) { Text = dictionary["Desc2"][i] };
                        DataEntry Price = new DataEntry(int.Parse(dictionary["IDKey"][i]), dictionary["Price"][i]) { Text = dictionary["Price"][i] };
                        DataEntry PriceSale = new DataEntry(int.Parse(dictionary["IDKey"][i]), dictionary["PriceSale"][i]) { Text = dictionary["PriceSale"][i] };
                        DataEntry Brand = new DataEntry(int.Parse(dictionary["IDKey"][i]), dictionary["Brand"][i]) { Text = dictionary["Brand"][i] };
                        DataEntry ItemType = new DataEntry(int.Parse(dictionary["IDKey"][i]), dictionary["ItemType"][i]) { Text = dictionary["ItemType"][i] };
                        List<UIElement> list = new List<UIElement>() { Desc1,Desc2,Price,PriceSale,Brand,ItemType};
                        EditDict.Add(int.Parse(dictionary["IDKey"][i]),list);
                        GridFiller.rapidFillSpacedPremadeObjects(list,TSection,Spacing,new bool[] { false, false, false , false, false, false });
                    }
                }
            }
        }
        public void printHeader()
        {
            string[] s = new string[] { "Spec 1", "Spec 2", "Wholesale", "Market" };
            if (BrandPicker.SelectedIndex == -1 || !BrandPicker.SelectedItem.Equals("Additives"))
            {
                s = new string[] { "Brand", "Spec 1", "Spec 2", "Wholesale", "Market" };
                if (TypePicker.SelectedIndex == -1)
                {
                    s = new string[] { "ItemType", "Brand", "Spec 1", "Spec 2", "Wholesale", "Market" };
                }
            }
            else if (TypePicker.SelectedIndex == -1)
            {
                s = new string[] { "ItemType", "Spec 1", "Spec 2", "Wholesale", "Market" };
            }
            int[] Spacing = new int[] { 1, 1, 1, 1, 1, 1 };
            if (!(bool)EditMode.IsChecked)
            {
                if (s.Length == 5)
                {
                    Spacing = new int[] { 1, 2, 1, 1, 1 };
                }
                else if (s.Length == 4)
                {
                    Spacing = new int[] { 2, 2, 1, 1 };
                }
            }
            GridFiller.rapidFillSpacedRowHeightLocked(s, TSection, Spacing, new int[] { 25, 0 });
        }
        public void PurgeCells()
        {
            GridFiller.PurgeAllGrid(TSection);
        }
    }
}
