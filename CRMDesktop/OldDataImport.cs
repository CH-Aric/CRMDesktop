using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRMDesktop
{

    class OldDataImport
    {
        Dictionary<string, List<string>> cusFieldArchive;
        public void import()
        {
            string sql = "SELECT * FROM cusfields WHERE cusfields.Index<>'Phone' AND cusfields.Index<>'Address' AND cusfields.Index<>'Source' AND cusfields.Index<>'Modified On' AND cusfields.Index<>'Notes' AND cusfields.Index<>'Region' AND cusfields.Index<>'Created On' AND cusfields.Index<>'Email' AND TaskID IS NULL";
            TaskCallback call = archiveCusfields;
            DatabaseFunctions.SendToPhp(false,sql,call);
        }
        public void archiveCusfields(string result)
        {
            List<String> list = new List<String>();
            cusFieldArchive = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (cusFieldArchive.Count > 0)
            {
                for (int i = 0; i < cusFieldArchive["Index"].Count; i++)
                {
                    //These lines move Data from Cusfields to Jobfiels, Never needs to be run again!
                    /*string x = "INSERT INTO jobfields (JobID,jobfields.Index,jobfields.Value,AdvValue) VALUES ('" + cusFieldArchive["CusID"][i] + "','" + cusFieldArchive["Index"][i] + "','" + cusFieldArchive["Value"][i] + "','" + cusFieldArchive["AdvValue"][i] + "')";
                    list.Add(x);
                    string y = "UPDATE cusfields SET TaskID='999999' WHERE IDKey='"+cusFieldArchive["IDKey"][i]+"'";
                    list.Add(y);*/

                }
            }
            DatabaseFunctions.SendBatchToPHP(list);
        }
    }
}
