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
using System.Threading;
using System.IO.Pipes;
using System.IO;

namespace ptopw
{
    class PtopwAPIImpl : PtopwAPIs
    {

        Thread thread = null;
        DataStatistics stat;
        NamedPipeServerStream pipeServer = null;
        public PtopwAPIImpl(DataStatistics ds)
        {
            stat = ds;
            initServerThread();
        }

        public double[] ComponentInfo(int flag)
        {
            double[] data = new double[flag == 0?8:4];
            DataNode dn = stat.GetDisplayData();
            if (dn == null) return null;

            SystemEnergy se = dn.sEnergy;
            int i = 0;

            switch(flag)
            {
                case 1:
                    data[i++] = se.cpuEng;
                    data[i++] = se.memEng;
                    data[i++] = se.diskEng;
                    data[i++] = se.nicEng;
                    break;
                case 2:
                    data[i++] = se.cpuPower;
                    data[i++] = se.memPower;
                    data[i++] = se.diskPower;
                    data[i++] = se.nicPower;
                    break;
                case 0:
                default:
                    data[i++] = se.cpuEng;
                    data[i++] = se.memEng;
                    data[i++] = se.diskEng;
                    data[i++] = se.nicEng;
                    data[i++] = se.cpuPower;
                    data[i++] = se.memPower;
                    data[i++] = se.diskPower;
                    data[i++] = se.nicPower;
                    break;
            }
            return data;
        }

        public double[] ProcessInfo(int flag, int pid)
        {
            DataNode dn = stat.GetDisplayData();
            if (dn == null|| dn.pEnergy.Contains(pid) == false)
                return null;

            ProcessEnergy se = (ProcessEnergy)dn.pEnergy[pid];
            double[] data = new double[flag == 0 ? 8 : 4];
            int i = 0;

            switch (flag)
            {
                case 1:
                    data[i++] = se.cpuEng;
                    data[i++] = se.memEng;
                    data[i++] = se.diskEng;
                    data[i++] = se.nicEng;
                    break;
                case 2:
                    data[i++] = se.cpuPower;
                    data[i++] = se.memPower;
                    data[i++] = se.diskPower;
                    data[i++] = se.nicPower;
                    break;
                case 0:
                default:
                    data[i++] = se.cpuEng;
                    data[i++] = se.memEng;
                    data[i++] = se.diskEng;
                    data[i++] = se.nicEng;
                    data[i++] = se.cpuPower;
                    data[i++] = se.memPower;
                    data[i++] = se.diskPower;
                    data[i++] = se.nicPower;
                    break;
            }
            return data;
        }

        public double systemEnergy(long time)
        {
            double total = 0;
            ArrayList datalist;
            datalist = stat.GetRequired(time);

            foreach (DataNode dk in datalist)
            {
                total += dk.sEnergy.totalSystemEnergy();
            }


            return total;
        }
        
        public double cpuEnergy(long time)
        {
            double total = 0;

            ArrayList datalist;
            datalist = stat.GetRequired(time);

            foreach (DataNode dk in datalist)
            {
                total += dk.sEnergy.cpuEng;
            }

            return total;
        }

        public double memoryEnergy(long time)
        {
            double total = 0;

            ArrayList datalist;
            datalist = stat.GetRequired(time);

            foreach (DataNode dk in datalist)
            {
                total += dk.sEnergy.memEng;
            }

            return total;
        }

        public double diskEnergy(long time)
        {
            double total = 0;

            ArrayList datalist;
            datalist = stat.GetRequired(time);

            foreach (DataNode dk in datalist)
            {
                total += dk.sEnergy.diskEng;
            }

            return total;
        }

        public double nicEnergy(long time)
        {
            double total = 0;

            ArrayList datalist;
            datalist = stat.GetRequired(time);

            foreach (DataNode dk in datalist)
            {
                total += dk.sEnergy.nicEng;
            }

            return total;
        }

        public double totalProcessEnergy(long time, int pid)
        {
            double total = 0;

            ArrayList datalist;
            datalist = stat.GetRequired(time);

            foreach (DataNode dk in datalist)
            {
                if (dk.pEnergy.Contains(pid) == false)
                    continue;

                ProcessEnergy pe = dk.GetProcessEnergy(pid);
                total += pe.GetTotalEnergy();
            }

            return total;
        }

        public double cpuProcessEnergy(long time, int pid)
        {
            double total = 0;

            ArrayList datalist;
            datalist = stat.GetRequired(time);

            foreach (DataNode dk in datalist)
            {
                if (dk.pEnergy.Contains(pid) == false)
                    continue;

                ProcessEnergy pe = dk.GetProcessEnergy(pid);
                total += pe.cpuEng;
            }

            return total;
        }

        public double memoryProcessEnergy(long time, int pid)
        {
            double total = 0;

            ArrayList datalist;
            datalist = stat.GetRequired(time);

            foreach (DataNode dk in datalist)
            {
                if (dk.pEnergy.Contains(pid) == false)
                    continue;

                ProcessEnergy pe = dk.GetProcessEnergy(pid);
                total += pe.memEng;
            }

            return total;
        }

        public double diskProcessEnergy(long time, int pid)
        {
            double total = 0;

            ArrayList datalist;
            datalist = stat.GetRequired(time);

            foreach (DataNode dk in datalist)
            {
                if (dk.pEnergy.Contains(pid) == false)
                    continue;

                ProcessEnergy pe = dk.GetProcessEnergy(pid);
                total += pe.diskEng;
            }

            return total;
        }

        public double nicProcessEnergy(long time, int pid)
        {
            double total = 0;

            ArrayList datalist;
            datalist = stat.GetRequired(time);

            foreach (DataNode dk in datalist)
            {
                if (dk.pEnergy.Contains(pid) == false)
                    continue;

                ProcessEnergy pe = dk.GetProcessEnergy(pid);
                total += pe.nicEng;
            }

            return total;
        }

       public Hashtable processEnergy()
        {
            DataNode temp = stat.GetDisplayData();
            if (temp == null) return null;
            
            return temp.pEnergy;
        }

       public Hashtable processAvgPower()
       {
           DataNode temp = stat.GetDisplayData();
           if (temp == null) return null;

           return temp.pEnergy;
       }

       public double systemAvgPower()
       {
           DataNode temp = stat.GetDisplayData();
           if (temp == null) return 0;

           return temp.sEnergy.weightedPower;
       }

        private void initServerThread()
        {
            thread = new Thread(ServerThread);
            thread.IsBackground = true;
            thread.Start();
        }

        private void ServerThread()
        {
            try{
                if(pipeServer == null)
                    pipeServer = new NamedPipeServerStream("ptopw", PipeDirection.InOut, 1);
            }catch(Exception ex)
            {
                throw new MyException(ex.Message, MyException.MSG_CREATE_APISERVER_ERROR);
            }

            while(true)
            {
                try
                {
                    // Wait for a client to connect
                    pipeServer.WaitForConnection();
                    PipeProtocol pp = new PipeProtocol(pipeServer);
                    Message msg = pp.ReadMessage();
                    
                    Message resp = msg.opossiteMessage();
                    processRequest(msg, resp);
                    pp.WriteMessage(resp);
                    pipeServer.Flush();
                }
                catch(Exception e)
                {
                    Console.WriteLine("ERROR: {0}", e.Message);
                }
                finally{
                    pipeServer.Disconnect();
                }
            }
        }

        private void processRequest(Message req, Message resp)
        {
            switch(req.type)
            {
                case Message.MSG_REQ_COMPONENT:
                    resp.data= ComponentInfo(req.flag);
                    break;
                case Message.MSG_REQ_PROCESS:
                    resp.data = ProcessInfo(req.flag, req.pid);
                    break;
                case Message.MSG_REQ_SYSTEM_ENG:
                    resp.energy = systemEnergy(req.time);
                    break;
                case Message.MSG_REQ_CPU_ENG:
                    resp.energy = cpuEnergy(req.time);
                    break;
                case Message.MSG_REQ_MEM_ENG:
                    resp.energy = memoryEnergy(req.time);
                    break;
                case Message.MSG_REQ_DISK_ENG:
                    resp.energy = diskEnergy(req.time);
                    break;
                case Message.MSG_REQ_NIC_ENG:
                    resp.energy = nicEnergy(req.time);
                    break;
                case Message.MSG_REQ_PRO_ENG:
                    resp.energy = totalProcessEnergy(req.time, req.pid);
                    break;
                case Message.MSG_REQ_PRO_CPU_ENG:
                    resp.energy = cpuProcessEnergy(req.time, req.pid);
                    break;
                case Message.MSG_REQ_PRO_MEM_ENG:
                    resp.energy = memoryProcessEnergy(req.time, req.pid);
                    break;
                case Message.MSG_REQ_PRO_DISK_ENG:
                    resp.energy = diskProcessEnergy(req.time, req.pid);
                    break;
                case Message.MSG_REQ_PRO_NIC_ENG:
                    resp.energy = nicProcessEnergy(req.time, req.pid);
                    break;
                case Message.MSG_REQ_PROINFO:
                    resp.info = processEnergy();
                    break;
                case Message.MSG_REQ_PRO_AVGPOWER:
                    resp.info = processAvgPower();
                    break;
                case Message.MSG_REQ_SYS_AVGPOWER:
                    resp.energy = systemAvgPower();
                    break;
                default:
                    break;
            }
        }
    }
}
