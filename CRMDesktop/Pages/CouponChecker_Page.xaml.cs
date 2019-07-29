using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CRMDesktop.Pages
{
    /// <summary>
    /// Interaction logic for CouponChecker_Page.xaml
    /// </summary>
    public partial class CouponChecker_Page : Page
    {
            public CouponChecker_Page()
            {
                InitializeComponent();
            }
            public void onClickCheck(object sender, RoutedEventArgs e)
            {
                string today = "" + DateTime.Today;
                string sql = "SELECT * FROM coupons WHERE Code='" + CouponEntry.Text + "' AND ((StartDate<'" + today + "'AND EndDate>'" + today + "') OR EndDate IS NULL);";
                TaskCallback call = populate;
                DatabaseFunctions.SendToPhp(false, sql, call);
            }
            public void populate(string result)
            {
                Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
                if (dictionary.Count > 0)
                {
                    detailDisplay.Content = dictionary["Description"][0];
                }
                else
                {
                    detailDisplay.Content = "Please check the coupon Code and Expiry and try again: " + CouponEntry.Text + " Does not appear to be valid!";
                }
            }
        }
}
