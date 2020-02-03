using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CRMDesktop
{
    public static class FormatFunctions
    {
        private static Regex digitsOnly = new Regex("[^\\d]");
        static bool Responded = false;
        static Dictionary<string, List<string>> dict;
        static Dictionary<string,List<string>> AgentDictionary;
        public static string CleanPhone(string phone)
        {
            return FormatFunctions.digitsOnly.Replace(phone, "");
        }
        public static string PrettyPhone(string phone)
        {
            if (phone.Length < 10)
            {
                return phone;
            }
            return string.Format("({0}) {1}-{2}", phone.Substring(0, 3), phone.Substring(3, 3), phone.Substring(6));
        }
        public static string CleanDateNew(string Date)
        {
            if (Date != null)
            {
                string s1 = Date.Replace(",", "^");
                string s2 = s1.Replace(":", "<");
                string s3 = s2.Replace("'",">");
                return s3;
            }
            return "";
        }
        public static string PrettyDate(string Date)
        {
            string s1 = Date.Replace("^", ",");
            string s2 = s1.Replace("<", ":");
            string s3 = s2.Replace(">","'");
            return s3;
        }
        public static string JustDay(string datein)
        {
            return CleanDate(datein)[0];
        }
        public static string[] CleanDate(string datein)
        {
            return datein.Split(' ')[0].Split('-');
        }
        public static string stripper(string intake)
        {
            string intake2 = intake;
            intake2 = intake2.Replace("\"", "");
            intake2 = intake2.Replace("\\", "");
            intake2 = intake2.Replace("[", "");
            intake2 = intake2.Replace("]", "");
            intake2 = intake2.Replace("{", "");
            intake2 = intake2.Replace("}", "");
            intake2 = intake2.Replace("[", "");
            intake2 = intake2.Replace("NewRow:", "");
            return intake2;
        }
        public static string[] SplitToPairs(string input)
        {
            input = stripper(input);
            return input.Split(',');
        }
        public static Dictionary<string, List<string>> createValuePairs(string[] input)
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            for (int i = 0; i < input.Length; i++)
            {
                string[] array = input[i].Split(':');
                if (array.Length < 2)
                {
                    return dictionary;
                }
                if (dictionary.ContainsKey(array[0]))
                {
                    dictionary[array[0]].Add(array[1]);
                }
                else
                {
                    dictionary.Add(array[0], new List<string>());
                    dictionary[array[0]].Add(array[1]);
                }
            }
            return dictionary;
        }
        public static Dictionary<string, List<string>> createValuePairs(string[] input, bool TimeOverload)
        {
            Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
            for (int i = 0; i < input.Length; i++)
            {
                string[] array = input[i].Split(':');
                if (array.Length < 2)
                {
                    return dictionary;
                }
                if (dictionary.ContainsKey(array[0]))
                {
                    if (array.Length == 4)
                    {
                        dictionary[array[0]].Add(array[1] + ":" + array[2] + ":" + array[3]);
                    }
                    else
                    {
                        dictionary[array[0]].Add(array[1]);
                    }
                }
                else
                {
                    dictionary.Add(array[0], new List<string>());
                    if (array.Length == 4)
                    {
                        dictionary[array[0]].Add(array[1] + ":" + array[2] + ":" + array[3]);
                    }
                    else
                    {
                        dictionary[array[0]].Add(array[1]);
                    }
                }
            }
            return dictionary;
        }
        public async static Task<string> smartsearch(string term, string table)
        {
            Responded = false;
            TaskCallback call = boolSetter;
            string sql = "SELECT smartStatement FROM smartsearch WHERE table='" + table + "' AND term='" + term + "';";
            DatabaseFunctions.SendToPhp(false, sql, call);
            while (!Responded)
            {
                await Task.Delay(50);
            }
            string toReturn = "";
            if (dict.Count > 0)
            {
                foreach (string s in dict["smartStatement"])
                {
                    toReturn += " OR " + s;
                }
            }
            return toReturn;
        }
        public static void boolSetter(string result)
        {
            string[] input = FormatFunctions.SplitToPairs(result);
            dict = FormatFunctions.createValuePairs(input);
            Responded = true;
        }
        public static List<UIElement> scrubOutUnwanted(List<UIElement> listToScrub, UIElement example)
        {
            List<UIElement> returnList = new List<UIElement>();
            foreach (UIElement x in listToScrub)
            {
                if (!x.GetType().Equals(example.GetType()))
                {
                    returnList.Add(x);
                }
            }
            return returnList;
        }
        public static List<UIElement> scrubOutUnlessWanted(List<UIElement> listToScrub, UIElement example,List<Label> scrubList)
        {
            List<UIElement> returnList = new List<UIElement>();
            foreach (UIElement x in listToScrub)
            {
                if (x.GetType().Equals(example.GetType()))
                {
                    returnList.Add(x);
                }
                else
                {
                    scrubList.Add((Label)x);
                }
            }
            return returnList;
        }
        public static string getRelevantDates(DateTime d,string fieldname)
        {
            string day = d.ToString("yyyy/M/d");
            string dayalt = d.ToString("yyyy-M-d");
            string[] x = day.Split(' ');
            string[] y = dayalt.Split(' ');
            string r = fieldname + " LIKE '%" + x[0] + " %' OR "+fieldname+" LIKE '%" + y[0] + " %'";
            for (int i = 0; i < 6; i++)
            {
                d=d.AddDays(1);
                day = d.ToString("yyyy/M/d");
                dayalt = d.ToString("yyyy-M-d");
                x = day.Split(' ');
                y = dayalt.Split(' ');
                r += " OR " + fieldname + " LIKE '%" + x[0] + " %' OR " + fieldname + " LIKE '%" + y[0] + " %'";
            }
            return r;
        }
        public static string getAgeOfTimestamp(string time)
        {
            DateTime d = DateTime.Parse(time);
            string result = string.Empty;
            var timeSpan = DateTime.Now.Subtract(d);

            if (timeSpan <= TimeSpan.FromSeconds(60))
            {
                result = string.Format("{0} seconds ago", timeSpan.Seconds);
            }
            else if (timeSpan <= TimeSpan.FromMinutes(60))
            {
                result = timeSpan.Minutes > 1 ?
                    String.Format("about {0} minutes ago", timeSpan.Minutes) :
                    "about a minute ago";
            }
            else if (timeSpan <= TimeSpan.FromHours(24))
            {
                result = timeSpan.Hours > 1 ?
                    String.Format("about {0} hours ago", timeSpan.Hours) :
                    "about an hour ago";
            }
            else if (timeSpan <= TimeSpan.FromDays(30))
            {
                result = timeSpan.Days > 1 ?
                    String.Format("about {0} days ago", timeSpan.Days) :
                    "yesterday";
            }
            else if (timeSpan <= TimeSpan.FromDays(365))
            {
                result = timeSpan.Days > 30 ?
                    String.Format("about {0} months ago", timeSpan.Days / 30) :
                    "about a month ago";
            }
            else
            {
                result = timeSpan.Days > 365 ?
                    String.Format("about {0} years ago", timeSpan.Days / 365) :
                    "about a year ago";
            }
            return result;
        }
        public static string StageTranslate(int intake)
        {
            if (intake == 1)
            {
                return "Lead";
            }
            else if (intake == 2)
            {
                return "Booking";
            }
            else if (intake == 3)
            {
                return "Quote";
            }
            else if (intake == 4)
            {
                return "Sold";
            }
            else if (intake == 5)
            {
                return "Install Preperation";
            }
            else if (intake == 6)
            {
                return "Installing";
            }
            else if (intake == 7)
            {
                return "Quality Assurance";
            }
            else if (intake == 8)
            {
                return "Client";
            }
            else if (intake == 9)
            {
                return "Archived";
            }
            else if (intake == 10)
            {
                return "Service Lead";
            }
            else if (intake == 11)
            {
                return "Service Appointment";
            }
            else if (intake == 12)
            {
                return "Completed Service";
            }
            else if (intake == 13)
            {
                return "Maintenance Appointment";
            }
            else if (intake == 14)
            {
                return "Maintenance";
            }
            return "Classification Error: Contact IT with the Client Name and this:"+intake;
        }
        public static int StageTranslate(string intake)
        {
            if (intake == "Lead")
            {
                return 1;
            }
            else if (intake == "Booking")
            {
                return 2;
            }
            else if (intake == "Quote")
            {
                return 3;
            }
            else if (intake == "Sold")
            {
                return 4;
            }
            else if (intake == "Install Preperation")
            {
                return 5;
            }
            else if (intake == "Installing")
            {
                return 6;
            }
            else if (intake == "Quality Assurance")
            {
                return 7;
            }
            else if (intake == "Client")
            {
                return 8;
            }
            else if (intake == "Archived")
            {
                return 9;
            }
            else if (intake == "Service Lead")
            {
                return 10;
            }
            else if (intake == "Service Appointment")
            {
                return 11;
            }
            else if (intake == "Completed Service")
            {
                return 12;
            }
            else if (intake == "Maintenance Appointment")
            {
                return 13;
            }
            else if (intake == "Maintenance")
            {
                return 14;
            }
            return 99;
        }
        public static void LoadAgentDictionary()
        {
            string sql = "SELECT IDKey, FName FROM agents WHERE Active='1'";
            TaskCallback call = populateDictionary;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        static void populateDictionary(string results)
        {
            AgentDictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(results));
            int x = 0;
        }
        public static int lookupAgentID(string intake)
        {
            for(int i = 0; i < AgentDictionary["IDKey"].Count; i++)
            {
                if (AgentDictionary["FName"][i] == intake)
                {
                    return int.Parse(AgentDictionary["IDKey"][i]);
                }
            }
            return 0;
        }
        public static string lookupAgentName(int intake)
        {
            string intakeS = intake + "";
            for (int i = 0; i < AgentDictionary["IDKey"].Count; i++)
            {
                if (AgentDictionary["IDKey"][i] == intakeS)
                {
                    return AgentDictionary["FName"][i];
                }
            }
            return "Unknown Agent #"+intake;
        }
    }
}
