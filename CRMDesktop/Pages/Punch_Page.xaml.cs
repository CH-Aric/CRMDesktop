﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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
            scroll.Height = ClientData.FrameHeight - 100;
            getPunches();
            getCurrentPunchState(false);
        }
        public void onclick(object sender,RoutedEventArgs e)
        {
            StyledButton x = (StyledButton)sender;
            x.Background = new SolidColorBrush(Colors.SlateGray);
            createPunchOnResult = true;
            getCurrentPunchState(true);
        }
        public void onClickStateless(object sender, RoutedEventArgs e)
        {
            StyledButton x = (StyledButton)sender;
            x.Background = new SolidColorBrush(Colors.SlateGray);
            statelessPunch = true;
            getCurrentPunchState(true);
        }
        public void getCurrentPunchState(bool mode)
        {
            string sql = "SELECT State FROM punchclock WHERE AgentID='" + ClientData.AgentIDK + "' AND State!='less' ORDER BY IDKey DESC LIMIT 1";
            TaskCallback call;
            if (mode)
            {

                 call= writeState;
            }
            else
            {
                call = renderState;
            }
            DatabaseFunctions.SendToPhp(false,sql,call);
        }
        public void renderState(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                PunchedIn = bool.Parse(dictionary["State"][0]);
                if (PunchedIn)
                {
                    PunchButton.Background = new SolidColorBrush(ClientData.rotatingConfirmationColors[0]);
                    ClockState.Content = "Current State: Clocked In";
                }
                else
                {
                    PunchButton.Background = new SolidColorBrush(ClientData.rotatingNegativeColors[0]);
                    ClockState.Content = "Current State: Clocked Out";
                }
            }
        }
        public void writeState(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                PunchedIn = bool.Parse(dictionary["State"][0]);
                if (((PunchedIn&&!createPunchOnResult) || (!PunchedIn&&createPunchOnResult))&&!statelessPunch)
                {
                    PunchButton.Background = new SolidColorBrush(ClientData.rotatingConfirmationColors[0]);
                    ClockState.Content = "Current State: Clocked In";
                }
                else if(!statelessPunch)
                {
                    PunchButton.Background = new SolidColorBrush(ClientData.rotatingNegativeColors[0]);
                    ClockState.Content = "Current State: Clocked Out";
                }
            }
            if (createPunchOnResult)
            {
                createPunchOnResult = false;
                string sql = "INSERT INTO punchclock (AgentID,Timestamp,Coordinates,State,Note) VALUES('"+ClientData.AgentIDK+"','"+FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d HH:mm:ss"))+"','Desktop','"+!PunchedIn+"','"+TextEntry.Text+"')";
                DatabaseFunctions.SendToPhp(sql);
            }
            if (statelessPunch)
            {
                statelessPunch = false;
                string sql = "INSERT INTO punchclock (AgentID,Timestamp,Coordinates,State,Note) VALUES('" + ClientData.AgentIDK + "','" + FormatFunctions.CleanDateNew(DateTime.Now.ToString("yyyy/M/d HH:mm:ss")) + "','Desktop','less','"+ TextEntry.Text+ "')";
                DatabaseFunctions.SendToPhp(sql);
            }
            getPunches();
        }
        public void getPunches()
        {
            string sql = "SELECT * FROM punchclock WHERE AgentID='"+ClientData.AgentIDK+ "' ORDER BY IDKey DESC LIMIT 10";
            TaskCallback call = populatePunches;
            DatabaseFunctions.SendToPhp(false, sql, call);
        }
        public void populatePunches(string result)
        {
            Dictionary<string, List<string>> dictionary = FormatFunctions.createValuePairs(FormatFunctions.SplitToPairs(result));
            if (dictionary.Count > 0)
            {
                GridFiller.PurgeHeader(logGrid);
                string[] list = new string[3];
                for(int i = 0; i < dictionary["IDKey"].Count; i++)
                {
                    list[0] = dictionary["Note"][i];
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
