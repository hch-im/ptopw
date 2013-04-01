/*
 *   Copyright (C) 2011, Mobile and Internet Systems Laboratory.
 *   Department of Computer Science, Wayne State University.
 *   All rights reserved.
 *
 *   Authors: Hui Chen (huichen@wayne.edu), Youhuizi Li (huizi@wayne.edu) 
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections;
using System.Windows.Forms;

namespace ptopw
{
    public class DataCollection
    {
        private Hashtable param;
        public static String DEFAULT_CONF_FILE = "ptopw.conf";
        
        private DataStatistics dataStat;
        private MainWindow window;
        private PtopwAPIImpl api;
        private PtopPerformanceCounter counter = null;
        private Thread statThread = null;
        private int DisplayResult = 1;
        
        public DataCollection(MainWindow mwin)
        {
            window = mwin;             
	        //load conf file
	        loadConfFile(DEFAULT_CONF_FILE);
            mwin.SetParams(param);

            try
            {
                dataStat = new DataStatistics(param);
            }
            catch (MyException ex)
            {
                mwin.ProcessException(ex);
            }
            dataStat.Log("Finished init data statistics...");
            api = new PtopwAPIImpl(dataStat);
            dataStat.Log("Finished init API server...");
            DisplayResult = PublicFuns.str2Int32((String)param["DisplayResult"], 1);

            initPerformanceCounter();
            dataStat.Log("Finished init performance counter...");
        }

        private void initPerformanceCounter()
        {
            counter = new PtopPerformanceCounter();
            try
            {
                counter.CreateCategory();
            }
            catch (MyException ex)
            {
                window.ProcessException(ex);
            }

        }

        public void setNetworkInterface(int index)
        {
            param["WNICIndex"] = index;
            dataStat.SetWnicIndex(index);
        }

        public void start()
        {
            if (statThread == null)
            {
                statThread = new Thread(statThreadFunc);
                statThread.IsBackground = true;
                statThread.Start();
                dataStat.Log("start data stat thread...");
            }
        }

        void loadConfFile(string filename)
        {
            param = new Hashtable();
	        ConfigFileParser parser = new ConfigFileParser(filename);
            try
            {
                parser.parse(param);
            }
            catch (MyException ex)
            {
                window.ProcessException(ex);
            }
        }

        // CptopwApp message handlers
        private void statThreadFunc()
        {
	        int sleep = Convert.ToInt32(param["Sleep"]);
            DataNode dn;
            dataStat.Log("before exec data stat loop...");
            while(isThreadActive())
	        {
                try
                {
                    dataStat.Log("exec data statistic...");
                    dataStat.stat();
                    if (DisplayResult == 1)
                    {
                        dn = dataStat.GetDisplayData();
                        window.UpdateDataNode(dn);
                        counter.addOrUpdatePerformanceCounter(dn);
                    }
                }
                catch (MyException ex) {
                    window.ProcessException(ex);
                }
                finally
                {
                    Thread.Sleep(sleep);
                }
	        }
        }

        public ArrayList retrieveData(long time)
        {
            return dataStat.GetRequired(time);
        }

        public bool isThreadActive()
        {
            return (statThread!= null && statThread.IsAlive);
        }
    }
}
