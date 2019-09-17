using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace CRMDesktop
{
    public class ClientData
    {
        public static int StoredKey;
        public static int AgentID;
        public static int AgentIDK;
        public static Thickness GridMargin = new Thickness(1);
        public static Color[] rotatingColors = new Color[2] { Color.FromRgb(255, 255, 255), Color.FromRgb(213, 213, 213) };
        public static Color[] rotatingConfirmationColors = new Color[2] { Color.FromRgb(181, 255, 186), Color.FromRgb(181, 255, 221) };
        public static Color[] rotatingNegativeColors = new Color[2] { Color.FromRgb(255, 211, 181), Color.FromRgb(255, 186, 181) };
        public static SolidColorBrush textColor = new SolidColorBrush(Color.FromRgb(0,0,0));
        public static int LastColor = 0;
        private string Username = "x";
        private string Password = "x";
        private bool Responded;
        private Dictionary<string, List<string>> dict;
        private static List<string> SecurityKeys;

        //Desktop Stuff
        public static double FrameHeight;
        public static double FrameWidth;
        public static Frame mainFrame,sideFrame,toolFrame;
        public static Brush getGridColor()
        {
            LastColor++;
            if (LastColor > rotatingColors.Length - 1)
            {
                LastColor = 0;
            }
            SolidColorBrush myBrush = new SolidColorBrush(rotatingColors[LastColor]);
            return myBrush;
        }
        public static Brush getGridColorCC(bool CC)
        {
            LastColor++;
            if (LastColor > rotatingColors.Length - 1)
            {
                LastColor = 0;
            }
            if (CC)
            {
                SolidColorBrush myBrush2 = new SolidColorBrush(rotatingConfirmationColors[LastColor]);
                return myBrush2;
            }
            SolidColorBrush myBrush = new SolidColorBrush(rotatingNegativeColors[LastColor]);
            return myBrush;
        }
        public void loadUserDatafromFile()
        {
            //TODO REWRITE FOR DESKTOP
            /*if (Application.Current.Properties.ContainsKey("UN"))
            {
                Username = (Application.Current.Properties["UN"] as string);
                Password = (Application.Current.Properties["PW"] as string);
            }*/
            Username = (Properties.Settings.Default.UN);
            Password = (Properties.Settings.Default.PW);
        }
        public void writeUserDataToFile(string u, string p)
        {
            Properties.Settings.Default.UN = u;
            Properties.Settings.Default.PW = p;
            Properties.Settings.Default.Save();
            //TODO REWRITE FOR DESKTOP
        }
        public void wipeUserDataFromFile()
        {
            this.writeUserDataToFile("", "");
        }
        public async Task<bool> attemptNewLogin(string u, string p, bool s)
        {
            this.Responded = false;
            TaskCallback call = new TaskCallback(response);
            string statement = "SELECT IDKey,AgentNum FROM agents WHERE Username='"+u+"'AND Password='"+p+"';";
            DatabaseFunctions.SendToPhp(false, statement, call);
            while (!Responded)
            {

                await Task.Delay(5);
            }
            bool result;
            if (dict.Count > 1)
            {
                if (s)
                {
                    writeUserDataToFile(u, p);
                }
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public async Task<bool> attemptSavedLoginAsync()
        {
            Responded = false;
            loadUserDatafromFile();
            TaskCallback call = response;
            string statement = "SELECT IDKey,AgentNum FROM agents WHERE Username='" + Username + "'AND Password='" + Password + "';";
            DatabaseFunctions.SendToPhp(false, statement, call);
            while (!Responded)
            {
                await Task.Delay(50);
            }
            bool result;
            if (dict.Count > 1)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }
        public void response(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            dict = FormatFunctions.createValuePairs(input);
            Responded = true;
            if (dict.Count > 1)
            {
                ClientData.AgentIDK = int.Parse(dict["IDKey"][0]);
                string sql = "SELECT PermissionGranted FROM agentpermissions WHERE AgentID='" + AgentIDK + "'";
                TaskCallback call = loadSecurityKeys;
                DatabaseFunctions.SendToPhp(false, sql, call);
            }
        }
        public void loadSecurityKeys(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            SecurityKeys = new List<string>();
            if (dictionary.Count > 0)
            {
                SecurityKeys = dictionary["PermissionGranted"];
            }
        }
        public static bool hasSecurityKey(string Key)
        {
            if (SecurityKeys.Contains("Admin"))
            {
                return true;
            }
            return SecurityKeys.Contains(Key);
        }
    }
}