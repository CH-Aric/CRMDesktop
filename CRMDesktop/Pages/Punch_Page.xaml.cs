﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace CRMDesktop.Pages
{
    public partial class Punch_Page : Page
    {
        public bool PunchedIn=false;
        public bool createPunchOnResult=false;
        public bool statelessPunch=false;
        public Punch_Page()
        {
            InitializeComponent();
            scroll.Height = ClientData.FrameHeight - 50;
            getCurrentPunchState();
            getPunches();
        }
        public void onclick(object sender,RoutedEventArgs e)
        {
            createPunchOnResult = true;
            getCurrentPunchState();
        }
        public void onClickStateless(object sender, RoutedEventArgs e)
        {
            statelessPunch = true;
            getCurrentPunchState();
        }
        public void getCurrentPunchState()
        {
            string sql = "SELECT State FROM punchclock WHERE AgentID='" + ClientData.AgentIDK + "' AND State!='less' ORDER BY IDKey DESC LIMIT 1";
            TaskCallback call = writeState;
            DatabaseFunctions.SendToPhp(false,sql,call);
        }
        public void writeState(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                PunchedIn = bool.Parse(dictionary["State"][0]);
                if (((PunchedIn&&!createPunchOnResult) || (!PunchedIn&&createPunchOnResult))&&!statelessPunch)
                {

                    ClockState.Text = "Current State: Clocked In";
                }
                else if(!statelessPunch)
                {
                    ClockState.Text = "Current State: Clocked Out";
                }
            }
            if (createPunchOnResult)
            {
                createPunchOnResult = false;
                string sql = "INSERT INTO punchclock (AgentID,Timestamp,Coordinates,State) VALUES('"+ClientData.AgentIDK+"','"+FormatFunctions.CleanDateNew(DateTime.Now.ToString())+"','Desktop','"+!PunchedIn+"')";
                DatabaseFunctions.SendToPhp(sql);
            }
            if (statelessPunch)
            {
                statelessPunch = false;
                string sql = "INSERT INTO punchclock (AgentID,Timestamp,Coordinates,State) VALUES('" + ClientData.AgentIDK + "','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString()) + "','Desktop','less')";
                DatabaseFunctions.SendToPhp(sql);
            }
            getPunches();
        }
        public void getPunches()
        {
            string sql = "SELECT * FROM punchclock WHERE AgentID='"+ClientData.AgentIDK+"'";
            TaskCallback call = populatePunches;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populatePunches(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                string[] list = new string[3];
                for(int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    list[0] = dictionary["Coordinates"][i];
                    list[2] = dictionary["State"][i];
                    list[1] = FormatFunctions.PrettyDate(dictionary["TimeStamp"][i]);
                    if (list[2] != "less")
                    {
                        GridFiller.rapidFillColorized(list, logGrid, bool.Parse(list[2]));
                    }
                    else
                    {
                        list[2] = "Location Log";
                        GridFiller.rapidFill(list, logGrid);
                    }
                }
            }
        }
    }
}
