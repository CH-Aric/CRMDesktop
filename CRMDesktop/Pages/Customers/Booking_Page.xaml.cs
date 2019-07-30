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

namespace CRMDesktop.Pages.Customers
{
    /// <summary>
    /// Interaction logic for Booking_Page.xaml
    /// </summary>
    public partial class Booking_Page : Page
    {
        int customerID;
        public Booking_Page(int cusID)
        {
            customerID = cusID;
            InitializeComponent();
            searchCustomerData();
        }
        public void onClickAdvance(object sender, EventArgs e)
        {
            Advance_Page page = new Advance_Page();
            ClientData.mainFrame.Navigate(page);
        }
        public async void renderBookingMap(string Address)
        {
            /*var geoloc = await Geocoding.GetLocationsAsync(Address);
            place = geoloc.FirstOrDefault();
            Xamarin.Forms.Maps.Map map = new Xamarin.Forms.Maps.Map(MapSpan.FromCenterAndRadius(new Position(place.Latitude, place.Longitude), Distance.FromKilometers(1)))
            {
                IsShowingUser = true,
                HeightRequest = 100,
                WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            HeadData.Children.Add(map, 0, 3);
            Grid.SetColumnSpan(map, 2);*/
        }
        public async void onClickNavigate(object sender, EventArgs e)
        {
            //var options = new MapLaunchOptions { NavigationMode = NavigationMode.Driving };
            //await Xamarin.Essentials.Map.OpenAsync(place, options);
        }
        public void searchCustomerData()
        {
            string sql = "SELECT cusfields.Index,cusfields.Value,cusindex.Name FROM crm2.cusfields INNER JOIN crm2.cusindex ON cusfields.CusID=cusindex.IDKey WHERE (cusfields.Index LIKE '%phone%' OR cusfields.Index LIKE '%address%' OR cusfields.Index LIKE '%book%') AND cusfields.CusID='" + customerID + "';";
            TaskCallback call = populateCustomerData;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populateCustomerData(string result)
        {
            string address = "2591 Ottawa Regional Road 174";
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                nameLabel.Content = dictionary["Name"][0];
                for (int i = 0; i < dictionary["Index"].Count; i++)
                {
                    if (dictionary["Index"][i].Contains("hone"))
                    {
                        phoneLabel.Content = dictionary["Value"][i];
                    }
                    else if (dictionary["Index"][i].Contains("ook"))
                    {
                        bookLabel.Content = "Booked For: " + dictionary["Value"][i];
                    }
                    else if (dictionary["Index"][i].Contains("ress"))
                    {
                        address = dictionary["Value"][i];
                        navButton.Content += dictionary["Value"][i];
                        address = dictionary["Value"][i];
                    }
                }
            }
            renderBookingMap(address);
        }
    }
}
