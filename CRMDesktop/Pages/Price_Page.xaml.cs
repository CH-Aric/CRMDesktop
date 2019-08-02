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
        public void populateBrands(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            BrandPicker.ItemsSource = dictionary["Brand"];
        }
        public void populateSearch(string result)
        {
            PurgeCells();
            printHeader();
            TSection.Background = new SolidColorBrush(Color.FromArgb(255,0,0,0));
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                for (int i = 0; i < dictionary["IDKey"].Count; i++)
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
            if (s.Length == 5)
            {
                Spacing = new int[] { 1, 2, 1, 1, 1 };
            }
            else if (s.Length == 4)
            {
                Spacing = new int[] { 2, 2, 1, 1 };
            }
            GridFiller.rapidFillSpacedRowHeightLocked(s, HeaderGrid, Spacing, new int[] { 50, 0 });
        }
        public void PurgeCells()
        {
            GridFiller.PurgeGrid(TSection);
            GridFiller.PurgeHeader(HeaderGrid);
        }
    }
}
