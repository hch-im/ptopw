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
using System.Management;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ptopw
{
    class DataStatistics
    {
	    bool hasWNIC;           //wheter the system exists wireless NIC
        int wnicIndex;          //the index of the wireless network interface
	    int SaveResult;         //0   # 1:save the collected data into files , 0 : do not save the result info files
	    string  FileName;        //ResultLog.txt    #file name that save the result, only useful when SaveResult = 1
        int StatisticLevel;      //# 1. only stat and show component level energy result; 2. stat and show process level energy result.

	    SystemInfo  sysInfo;    //used to save temporal system info
	    StatData  tempData;
        StreamWriter log;
        FileStream fileStream;

        Hashtable counterTable = new Hashtable();
        ManagementObject win32ProcessorObj;
        Processor processor = new Processor();
        bool debug = false;

        [DllImport("mydll.dll", EntryPoint = "GetNetInfo", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool GetNetInfo(ref WNICInfo info);

        public DataStatistics(Hashtable param)
        {
	        SaveResult = PublicFuns.str2Int32((String)param["SaveResult"], 1);    
	        FileName = (String)param["FileName"];
            StatisticLevel = PublicFuns.str2Int32((String)param["StatisticLevel"], 0);
            wnicIndex = PublicFuns.str2Int32((String)param["WNICIndex"], 0);
	        tempData = new StatData(param);

	        hasWNIC = false;
	        sysInfo = new SystemInfo();



            win32ProcessorObj = RetrieveManagementObject("Win32_Processor");
            processor.update(win32ProcessorObj);
            sysInfo.pro = processor;

            try
            {
	            getSystemInfo();
	            createSystemCounters();
                initLog();
            }
            catch (MyException ex)
            {
            	throw ex;
            }
        }

        private void initLog()
        {
            try
            {
                    fileStream = new FileStream(FileName, FileMode.Create);
                    log = new StreamWriter(fileStream);
            }
            catch (System.Exception ex)
            {
                throw new MyException("Log file is in use.", MyException.MSG_INIT_LOG_INUSE_ERROR);       	
            }

        }

        private ManagementObject RetrieveManagementObject(String className)
        {
            ManagementClass proClass = new ManagementClass(className);
            ManagementObjectCollection classObjects = proClass.GetInstances();

            //TODO consider multiple devices such as processor, disk
            foreach (ManagementObject item in classObjects)
            {
                return item;
            }

            return null;
        }

	    public void stat()
        {
            ManagementObject diskObj = RetrieveManagementObject("Win32_PerfFormattedData_PerfDisk_PhysicalDisk");
            //read system info
            sysInfo.percentDiskIdleTime = (UInt64)diskObj["PercentIdleTime"];
            sysInfo.percentDiskReadTime = (UInt64)diskObj["PercentDiskReadTime"];
            sysInfo.percentDiskWriteTime = (UInt64)diskObj["PercentDiskWriteTime"];
            //network
            sysInfo.nrecv = 0;
            sysInfo.nsend = 0;
            if (hasWNIC)
            {
                WNICInfo info = new WNICInfo();
                info.index = wnicIndex;
                bool res = GetNetInfo(ref info);
                if(res)
                {
                    sysInfo.nrecv = info.In - sysInfo.winfo.In;
                    sysInfo.nsend = info.Out - sysInfo.winfo.Out;
                    sysInfo.winfo = info;
                }
            }
            //memory
            ManagementObject memObj = RetrieveManagementObject("Win32_PerfFormattedData_PerfOS_Memory");
            ManagementObject cacheObj = RetrieveManagementObject("Win32_PerfFormattedData_PerfOS_Cache");

            sysInfo.memRead = (UInt32)memObj["PagesOutputPerSec"];//pages/sec
            sysInfo.memWrite = (UInt32)memObj["PagesInputPerSec"];
            sysInfo.memCopy = (UInt32)cacheObj["CopyReadsPerSec"];//pages/sec
            //processor
            ManagementObject cpuObj = RetrieveManagementObject("Win32_Processor");
            sysInfo.cpuFrequency = (UInt32)cpuObj["CurrentClockSpeed"];
            sysInfo.cpuTime = 0;
            sysInfo.voltage = (UInt16)cpuObj["CurrentVoltage"];
            //disk
            sysInfo.diskRead = 0;
            sysInfo.diskWrite = 0;

            //save
            tempData.beginRecord();
            tempData.addSystemInfo(sysInfo);//before add process info, important
            //read process info

                Process[] processes = Process.GetProcesses();
                HashSet<UInt32> processNetStatus = NetInfoWrapper.netActiveProcesses();
                sysInfo.NetActiveProcessNum = processNetStatus.Count;

                foreach (Process p in processes)
                {
                    int id = 0;
                    try
                    {
                        String name = p.ProcessName;
                        id = p.Id;
                        
                        ProcessInfo pinfo = statProcessInfo(p);
                        if (pinfo != null)
                        {
                            if (processNetStatus.Contains((UInt32)p.Id))
                                pinfo.networkActive = true;
                            tempData.addProcessInfo(pinfo);

                            sysInfo.cpuTime += pinfo.cpuTime;
                            sysInfo.diskRead += pinfo.dread;
                            sysInfo.diskWrite += pinfo.dwrite;
                        }
                    }
                    catch (Exception ex)//process exited
                    {
                        if (counterTable.ContainsKey(id))
                        {
                            counterTable.Remove(id);
                        }
                        Console.WriteLine(ex.StackTrace);
                    }
                }

            //update time
            double time = new TimeSpan(DateTime.Now.Ticks).TotalMilliseconds;
            sysInfo.timeInterval = time - sysInfo.time;
            sysInfo.time = time;
            tempData.endRecord();

            //log if needed
            if (SaveResult == 1) LogResult();
        }

        private ProcessInfo statProcessInfo(Process p)
        {

            ProcessCounters counters;
            if (counterTable.ContainsKey(p.Id))//old process
            {
                counters = (ProcessCounters)counterTable[p.Id];
                if(p.HasExited)
                {
                    return null;
                }
            }
            else//new process
            {
                counters = new ProcessCounters(p.ProcessName);
                counterTable.Add(p.Id, counters);
            }             

            ProcessInfo info = counters.sample(p);

            return info;
        }

        void createSystemCounters()
        {
            try
            {
                //network
                PerformanceCounterCategory niccat = new PerformanceCounterCategory("Network Interface");
                String[] instanceNames = niccat.GetInstanceNames();

                for (int i = 0; i < instanceNames.Length; i++)
                {
                    if (instanceNames[i].IndexOf("Wireless") > 0)
                    {
                        hasWNIC = true;
                        break;
                    }
                }
            }
            catch (System.Exception ex)
            {
                throw new MyException(ex.Message, MyException.MSG_INIT_STAT_COUNTER_ERROR);
            }
        }

        public DataNode GetDisplayData()
        {
            DataNode dn = tempData.getLastPowerInfo();
            return dn;
        }

        public ArrayList GetDataList()
        { 
            ArrayList dl = tempData.getDataInfo();
            return dl;
        }

        public ArrayList GetRequired(long time)
        {
            ArrayList dl = tempData.GetDataRequired(time);
            return dl;
        }

        void closeSystemCounters()
        {
        }

        void getSystemInfo()
        {
            try
            {
                SYSTEM_INFO sys = new SYSTEM_INFO();
                WinApi.GetSystemInfo(ref sys);
                sysInfo.pageSize = ((int)sys.dwPageSize) / 1024; //kb
            }
            catch (System.Exception ex)
            {
                throw new MyException(ex.Message, MyException.MSG_INIT_SYSTEMINFO_ERROR);
            }
        }

        void LogResult()
        {
            if (SaveResult != 1)
                return;

            switch (tempData.getDataRecordMode())
            {
                case 1://save energy
                    tempData.logEnergy(log);
                    break;
                case 2://save raw data and energy
                    tempData.logEnergy(log);
                    tempData.logStatData(log);
                    break;
                default:
                    break;
            }
        }

        public void Log(String str)
        {
            if (debug)
            {
                log.WriteLine("log: " + str);
                log.Flush();
            }
        }

        public void SetWnicIndex(int index)
        {
            wnicIndex = index;
        }

        public int statCount()
        {
            return tempData.statCount();
        }

    }

    class ProcessCounters{
        public TimeSpan lastProcessorTime;
        IO_COUNTERS lastio;

        public ProcessCounters(String name){
            lastio.init();
        }

        public ProcessInfo sample(Process p)
        {
            TimeSpan timespan = p.TotalProcessorTime;
//            if (timespan.TotalMilliseconds == lastProcessorTime.TotalMilliseconds)
//                return null;

            ProcessInfo info = new ProcessInfo();
            info.no = p.Id;
            info.name = p.ProcessName;
            //stat cpu info

            info.cpuTime = timespan.TotalMilliseconds - lastProcessorTime.TotalMilliseconds;
            lastProcessorTime = timespan;
            
            //stat the io information
            IO_COUNTERS ioc;
            WinApi.GetProcessIoCounters(p.Handle, out ioc);
            info.dread = ioc.ReadTransferCount - lastio.ReadTransferCount;
            info.dwrite = ioc.WriteTransferCount - lastio.WriteTransferCount;
            lastio = ioc;

            return info;
        }
    }
}

