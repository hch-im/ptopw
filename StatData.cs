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
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;

namespace ptopw
{
    public class StatData
    {
	    int DataRecordMode;     //1  # 1. Only save the final energy result.  2. Also save intermediate result.
	    int DataConservePeriod; // 300 # s, for how long we should conserve the history record.
        int StatisticLevel;      //# 1. only stat and show component level energy result; 2. stat and show process level energy result.
        double Weight;

	    EnergyModel  model;
	    private ArrayList data;
        private DataNode tempNode = null;

        public StatData(Hashtable param)
        {
            model = new EnergyModel(param);
    	    DataRecordMode = Convert.ToInt32(param["DataRecordMode"]);
	        DataConservePeriod = Convert.ToInt32(param["DataConservePeriod"]);
            StatisticLevel = Convert.ToInt32(param["StatisticLevel"]);
            Weight = Convert.ToDouble(param["CurrentPowerWeight"]);

            data = new ArrayList();
	        //TODO init parameters
        }

        public DataNode getLastPowerInfo()
        {
            if (data.Count == 0)
            {
                return null;
            }

            return (DataNode)data[0];
        }

        //millisecond
        public ArrayList GetDataRequired(long time)
        {
            ArrayList list = new ArrayList();

            if (data.Count == 0)
                return list;

            long t = DateTime.Now.Ticks;
            int i;

            DataNode temp;
            for (i = 0; i<data.Count; i++)
            {
                temp = (DataNode)data[i];
                TimeSpan span = new TimeSpan(t - temp.time);

                if (span.TotalMilliseconds > time)
                    break;
                list.Add(temp);
            }

            return list;            
        }

        public ArrayList getDataInfo()
        {
            if (data.Count == 0)
            {
                return null;
            }
            if (data.Count < 30)
            {
                return new ArrayList(data.GetRange(0, data.Count));
            }

            return new ArrayList(data.GetRange(0, 30));
        }

        //millisecond
	    private void deleteNodes(long interval)
        {
	        bool find = false;
	        long t =  DateTime.Now.Ticks;

	        foreach(DataNode temp1 in data){
		        if(find){
                    data.Remove(temp1);
		        }else{
                    TimeSpan span = new TimeSpan(t - temp1.time);
			        if(span.TotalMilliseconds >= interval){
				        find = true;
                        data.Remove(temp1);
			        }
		        }
	        }
        }

	    public void beginRecord()    //change the add flag, must be invoked before
        {
	        tempNode = new DataNode();
            tempNode.time = DateTime.Now.Ticks;
        }

        public void addProcessInfo(ProcessInfo info)
        {
            tempNode.pData.Add(info);
        }

        //invoked before addProcessInfo
	    public void addSystemInfo(SystemInfo sysInfo)
        {
		    tempNode.sData = sysInfo;
        }

        public void endRecord()
        {
            updateEnergy();
            data.Insert(0, tempNode);
            tempNode = null;
            deleteNodes(DataConservePeriod * 1000);
        }

        private void updateEnergy()
        {
            if (tempNode == null)
                return;

            DataNode dn = tempNode;

            //convert raw data two energy
            dn.sEnergy = new SystemEnergy();
            dn.sEnergy.interval = dn.sData.timeInterval;
            model.systemEnergy(dn.sData, dn.sEnergy);

            DataNode last = null;
            if(data.Count > 2)
            {
                last = (DataNode)data[1];
                dn.sEnergy.computeWeightedPower(last.sEnergy.weightedPower, Weight);
            }

            if (StatisticLevel == 2)
            {
                Hashtable engs = null;
                if(last != null) engs = last.pEnergy;
                foreach (ProcessInfo info in dn.pData)
                {
                    ProcessEnergy eng = new ProcessEnergy();
                    eng.name = info.name;
                    eng.no = info.no;
                    model.processEnergy(info, dn.sEnergy, dn.sData, eng);

                    if (engs != null && engs.ContainsKey(eng.no))
                    {
                        ProcessEnergy lasteng = (ProcessEnergy)engs[eng.no];
                        eng.computeWeightedPower(lasteng.weightedPower, Weight);
                    }
                    dn.pEnergy.Add(eng.no, eng);
                }
            }
            else
            {
                dn.pEnergy = null;
            }

            if (DataRecordMode == 1)//not record the raw data
            {
                dn.pData = null;
            }
        }

	    public void logEnergy(StreamWriter io)
        {
	        if(io == null || data.Count == 0) return;

            DataNode dn = (DataNode)data[0];
	        SystemEnergy  seng = dn.sEnergy;
            String tstr = TicketToTimeStr(dn.time);
            try
            {
                if (seng != null)
                {
                    io.Write("SYSENG,");
                    io.Write(tstr + ",");
                    io.Write(String.Format("{0:0.000}", seng.cpuEng) + ",");
                    io.Write(String.Format("{0:0.000}", seng.memEng) + ",");
                    io.Write(String.Format("{0:0.000}", seng.diskEng) + ",");
                    io.Write(String.Format("{0:0.000}", seng.nicEng) +",");

                    io.Write(String.Format("{0:0.000}", seng.cpuPower) + ",");
                    io.Write(String.Format("{0:0.000}", seng.memPower) + ",");
                    io.Write(String.Format("{0:0.000}", seng.diskPower) + ",");
                    io.WriteLine(String.Format("{0:0.000}", seng.nicPower));

                    io.WriteLine("-----------------------------------------------------------------------");

                    io.Flush();
                }

                if (StatisticLevel == 2)
                {
                    ArrayList pengArr = dn.GetProcessEnergyList();
                    foreach (ProcessEnergy peng in pengArr)
                    {
                       // io.Write("PIDENG,");
                       // io.Write(tstr + ",");
                        io.Write(peng.name + ",");
                        //io.Write(tstr);
                        io.Write(tstr + ",");
                        io.Write(peng.no + ",");
                        //io.Write(tstr);
                        io.Write(String.Format("{0:0.000}", peng.cpuEng) + ",");
                        io.Write(String.Format("{0:0.000}", peng.memEng) + ",");
                        io.Write(String.Format("{0:0.000}", peng.diskEng) + ",");
                        io.Write(String.Format("{0:0.000}", peng.nicEng) + ",");

                        io.Write(String.Format("{0:0.000}", peng.cpuPower) + ",");
                        io.Write(String.Format("{0:0.000}", peng.memPower) + ",");
                        io.Write(String.Format("{0:0.000}", peng.diskPower) + ",");
                        io.Write(String.Format("{0:0.000}", peng.nicPower)+",");
                        io.WriteLine(String.Format("{0:0.000}", peng.weightedPower) + ",");

                        //io.WriteLine(dn.MaxPTotal.Get(peng.GetID()));

                        io.Flush();
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
        }

	    public void logStatData(StreamWriter io)
        {
	        if(io == null || data.Count == 0) return;
            DataNode dn = (DataNode)data[0];
	        SystemInfo  sinfo = dn.sData;
            String tstr = TicketToTimeStr(dn.time);

            try{
	            if (sinfo != null)
	            {
                    io.Write("SYSINFO,");
                    io.Write(tstr + ",");
                    io.Write(String.Format("{0:0.000}", sinfo.timeInterval) + ",");
                    io.Write(sinfo.cpuFrequency + ",");
                    io.Write(sinfo.cpuTime + ",");
                    io.Write(sinfo.memRead + ",");
                    io.Write(sinfo.memWrite + ",");
                    io.Write(sinfo.memCopy + ",");
                    io.Write(sinfo.percentDiskIdleTime + ",");
                    io.Write(sinfo.percentDiskReadTime + ",");
                    io.Write(sinfo.percentDiskWriteTime + ",");
                    io.Write(sinfo.nrecv + ",");
                    io.Write(sinfo.nsend + ",");
                    io.WriteLine(sinfo.winfo.speed / 8);
                    io.Flush();
	            }

                if (StatisticLevel == 2)
                {
                    ArrayList pinfoarr = dn.pData;
                    foreach (ProcessInfo pinfo in pinfoarr)
                    {
                        io.Write("PIDINFO,");
                        io.Write(tstr + ",");
                        io.Write(pinfo.no + ",");
                        io.Write(pinfo.cpuTime + ",");
                        io.Write(String.Format("{0:0.000}", pinfo.minput) + ",");
                        io.Write(String.Format("{0:0.000}", pinfo.moutput) + ",");
                        io.Write(String.Format("{0:0.000}", pinfo.dread) + ",");
                        io.Write(String.Format("{0:0.000}", pinfo.dwrite) + ",");
                        io.Write(String.Format("{0:0.000}", pinfo.nrecv) + ",");
                        io.WriteLine(String.Format("{0:0.000}", sinfo.nsend));
                        io.Flush();
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
        }

        private String TicketToTimeStr(long time)
        {
            DateTime Time = new DateTime(time);

            return String.Format("{0:T}", Time);
        }

        public int getDataRecordMode()
        {
            return DataRecordMode;
        }

        public int statCount()
        {
            if (data == null)
                return 0;
            else
                return data.Count;
        }
    }
}
