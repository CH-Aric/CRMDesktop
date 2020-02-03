using System;
using System.Windows.Controls;

namespace CRMDesktop.Pages
{
    /// <summary>
    /// Interaction logic for Calendar_Page.xaml
    /// </summary>
    public partial class Calendar_Page : Page
    {
        public Calendar_Page()
        {
            InitializeComponent();
            int Month = DateTime.Now.Month;
            int Year = DateTime.Now.Year;
            string first = DateTime.Parse(Year+"-"+Month+"-01").DayOfWeek.ToString();
            
        }
        public void labelCalendar()
        {

        }
    }
}
