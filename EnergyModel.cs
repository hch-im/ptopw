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
using System.Collections;

namespace ptopw
{
    class EnergyModel
    {
	    //Energy Computation Module
	    double MaxCPUPower;    //Watts, the maximum power of CPU
	    double MinCPUPower;    //6.0    #Watts, the minimum power of CPu
	    //Currently we do not consider data write operation of memory
	    double MemReadPower; //  2.016   #Watts, the power of memory during read operations
	    double MemWritePower ;// 2.016   #Watts, the power of memory may be different during read and write operations
        double MemIdlePower;
        double MemReadSpeed; 
        double MemWriteSpeed;
        double MemCopySpeed;

        double DiskReadPower; //  2.78     #Watts, the power of disk during read operations
	    double DiskWritePower;//  2.19     #Watts, the power of disk during write operations
        double DiskIdlePower;     //1.43     #Watts, the power of disk during write operations
	    double WirelessTransmitPower; //1.8    #Watts, the power of wireless network card when transmit signal
	    double WirelessReceivePower;   //1.4   #Watts, the power of wireless network card when receive signal
        double WirelessNICIdlePower;

	    double MaxFrequency; //the maximum frequency of the processor
        double alpha;

    	public EnergyModel(Hashtable param)
        {
            alpha = PublicFuns.str2Double((String)param["ALPHA"], 0.8);
            MaxCPUPower = PublicFuns.str2Double((String)param["MaxCPUPower"], 24.5);
            MinCPUPower = PublicFuns.str2Double((String)param["MinCPUPower"], 7.0);

            MemReadPower = PublicFuns.str2Double((String)param["MemReadPower"], 2.016);
            MemWritePower = PublicFuns.str2Double((String)param["MemWritePower"], 2.016);
            MemIdlePower = PublicFuns.str2Double((String)param["MemIdlePower"], 1.2);
            MemReadSpeed = PublicFuns.str2Double((String)param["MemReadSpeed"], 1015);
            MemWriteSpeed = PublicFuns.str2Double((String)param["MemWriteSpeed"], 1332);
            MemCopySpeed = PublicFuns.str2Double((String)param["MemCopySpeed"], 1440);

            DiskReadPower = PublicFuns.str2Double((String)param["DiskReadPower"], 2.78);
            DiskWritePower = PublicFuns.str2Double((String)param["DiskWritePower"], 2.19);
            DiskIdlePower = PublicFuns.str2Double((String)param["DiskIdlePower"], 1.43);

            WirelessTransmitPower = PublicFuns.str2Double((String)param["WirelessTransmitPower"], 2.268);
            WirelessReceivePower = PublicFuns.str2Double((String)param["WirelessReceivePower"], 2.289);
            WirelessNICIdlePower = PublicFuns.str2Double((String)param["WirelessNICIdlePower"], 0.43);
	        //init parameters
	        init();
        }

	    void init()
        {
	        MaxFrequency = 0;
        }

        void CPUEnergy(double CurrentFrequency, double cpuTime, double Interval, uint coreNum, SystemEnergy se)
        {
            se.cpuEng = (MinCPUPower*Interval/1000) +
                        alpha * ( 
                            (MaxCPUPower - MinCPUPower) * (CurrentFrequency / MaxFrequency) * cpuTime / coreNum / 1000
                            );

            se.cpuPower = se.cpuEng * 1000 / Interval;
        }

        void DiskEnergy(UInt64 idle, UInt64 read, UInt64 write, double interval, SystemEnergy se)//ms
        {

            se.diskEng = (((read*DiskReadPower) 
                           + (write*DiskWritePower) + (idle*DiskIdlePower)) * 1.0 /(idle + read + write)) * interval/1000;
            se.diskPower = se.diskEng * 1000 / interval;
        }

        void NICEnergy(double drecv, double dsend, double interval, SystemEnergy se)//ms
        {
            if((drecv + dsend) == 0)
            {
                se.nicEng = WirelessNICIdlePower * interval / 1000;
            }
            else
            {
                se.nicEng = ((drecv / (drecv + dsend) * WirelessReceivePower) 
                            + (dsend / (drecv + dsend) * WirelessTransmitPower)) 
                            * interval / 1000;
            }

            se.nicPower = se.nicEng / (interval / 1000);

        }

        void MemoryEnergy(double memread, double memwrite, double memcopy,double interval, int pagesize, SystemEnergy se)
        {
            double indata = ((memread * interval/1000) * pagesize)/1024;//MB, memory write
	        double outdata = ((memwrite * interval/1000) * pagesize)/1024;//MB, memory read
            double copydata = ((memcopy * interval / 1000) * pagesize);//MB, memory read
            double t = (indata / MemReadSpeed + outdata / MemWriteSpeed + copydata / MemCopySpeed)*1000;//ms 
            double idlet = interval - t;
            if (t > interval)//may happen
            {
                t = interval;
                idlet = 0;
            }

	        se.memEng = ((t * MemWritePower) + (idlet * MemIdlePower)) / 1000;//joule
            se.memPower = se.memEng / (interval/1000);//watt
        }

    	public void systemEnergy(SystemInfo info, SystemEnergy se)
        {
            MaxFrequency = info.pro.MaxClockSpeed;
            DiskEnergy(info.percentDiskIdleTime, info.percentDiskReadTime,
                info.percentDiskWriteTime, info.timeInterval, se);
            NICEnergy(info.nrecv, info.nsend, info.timeInterval, se);
            MemoryEnergy(info.memRead, info.memWrite, info.memCopy, info.timeInterval,info.pageSize, se);
            CPUEnergy(info.cpuFrequency, info.cpuTime, info.timeInterval, info.pro.NumberOfCores, se);	
        }

        void pCPUEnergy (ProcessInfo info, ProcessEnergy pe, SystemInfo sinfo, SystemEnergy se)
        {
            pe.cpuEng = se.cpuEng * info.cpuTime / (sinfo.cpuTime);
            pe.cpuPower = pe.cpuEng * 1000 / sinfo.timeInterval;
        }

        void pNICEnergy(ProcessInfo pinfo, ProcessEnergy peng, SystemEnergy seng, SystemInfo sinfo)
        {
            if(pinfo.networkActive && sinfo.NetActiveProcessNum > 0)
            {
                peng.nicEng = seng.nicEng / sinfo.NetActiveProcessNum; 
            }
            else{
                peng.nicEng = 0;
            }

            peng.nicPower = peng.nicEng * 1000 / sinfo.timeInterval; 
        }
        
        void pDiskEnergy(ProcessInfo pinfo, ProcessEnergy peng, SystemEnergy seng, SystemInfo sinfo)
        {
            peng.diskEng = seng.diskEng * 
                ((pinfo.dwrite + pinfo.dread) * 1.0 / (sinfo.diskRead + sinfo.diskWrite));
            peng.diskPower = peng.diskEng * 1000 / sinfo.timeInterval;
        }

        void pMemoryEnergy(ProcessInfo pinfo, ProcessEnergy peng, SystemEnergy seng, SystemInfo sinfo)
        {
            peng.memEng = seng.memEng *
                    ((pinfo.dwrite + pinfo.dread) * 1.0 / (sinfo.diskRead + sinfo.diskWrite));
            peng.memPower = peng.memEng * 1000 / sinfo.timeInterval;
        }
       
        public void processEnergy(ProcessInfo info, SystemEnergy se, SystemInfo sinfo,ProcessEnergy pe)
        {
            pCPUEnergy(info, pe, sinfo, se);
            pNICEnergy(info, pe, se, sinfo);
            pDiskEnergy(info, pe, se, sinfo);
            pMemoryEnergy(info, pe, se, sinfo);
        }
    }
}
