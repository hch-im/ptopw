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
using System.Net;
using System.Runtime.InteropServices;
using System.Collections;
using System.Diagnostics;

namespace ptopw
{
    static unsafe class NetInfoWrapper
    {
        private const int NO_ERROR = 0;
        private const int MIB_TCP_STATE_CLOSED = 1;
        private const int MIB_TCP_STATE_LISTEN = 2;
        private const int MIB_TCP_STATE_SYN_SENT = 3;
        private const int MIB_TCP_STATE_SYN_RCVD = 4;
        private const int MIB_TCP_STATE_ESTAB = 5;
        private const int MIB_TCP_STATE_FIN_WAIT1 = 6;
        private const int MIB_TCP_STATE_FIN_WAIT2 = 7;
        private const int MIB_TCP_STATE_CLOSE_WAIT = 8;
        private const int MIB_TCP_STATE_CLOSING = 9;
        private const int MIB_TCP_STATE_LAST_ACK = 10;
        private const int MIB_TCP_STATE_TIME_WAIT = 11;
        private const int MIB_TCP_STATE_DELETE_TCB = 12;

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private extern static int GetUdpStatistics(ref MIB_UDPSTATS pStats);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern int GetUdpTable(byte[] UcpTable, out int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private extern static int GetTcpStatistics(ref MIB_TCPSTATS pStats);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern int GetTcpTable(byte[] pTcpTable, out int pdwSize, bool bOrder);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private extern static int GetExtendedTcpTable(IntPtr pTable, ref UInt32 DwOutBufLen, bool sort, int ipVersion, TCP_TABLE_CLASS tblClass, int reserved);

        [DllImport("iphlpapi.dll", SetLastError = true)]
        private static extern int GetExtendedUdpTable(IntPtr pTable, ref UInt32 dwOutBufLen, bool sort, int ipVersion, UDP_TABLE_CLASS tblClass, int reserved);

        [DllImport("IpHlpApi.dll")]
        extern static public uint GetIfTable(byte[] pIfTable, ref uint pdwSize, bool bOrder);

        public static MIB_TCPSTATS GetTcpStats()
        {
            MIB_TCPSTATS tcpStats = new MIB_TCPSTATS();
            GetTcpStatistics(ref tcpStats);
            return tcpStats;
        }

        public static MIB_UDPSTATS GetUdpStats()
        {
            MIB_UDPSTATS udpStats = new MIB_UDPSTATS();
            GetUdpStatistics(ref udpStats);
            return udpStats;
        }

        public static bool GetProcessTCPConns(ref MIB_TCPTABLE_OWNER_PID ExConns)
        {
            UInt32* ptable = (UInt32*)IntPtr.Zero;
            UInt32 dwSize = 0;
            GetExtendedTcpTable((IntPtr)ptable, ref dwSize, true, AFType.AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0);

            char* tmp = stackalloc char[(int)dwSize];
            ptable = (UInt32*)tmp;

            if (GetExtendedTcpTable((IntPtr)ptable, ref dwSize, true, AFType.AF_INET, TCP_TABLE_CLASS.TCP_TABLE_OWNER_PID_ALL, 0) != NO_ERROR)
                return false;

            ExConns.dwNumEntries = (UInt32)ptable[0];
            ExConns.table = new MIB_TCPROW_OWNER_PID[ExConns.dwNumEntries];
            MIB_TCPROW_OWNER_PID * row = (MIB_TCPROW_OWNER_PID*)&ptable[1];
            UInt32 j = 0;
            for (int i = 0; i < ExConns.dwNumEntries; i++)
            {
                if (!IP.specialIP(row[i].dwLocalAddr) && !IP.specialIP(row[i].dwRemoteAddr))
                    ExConns.table[j++] = row[i];
            }
            
            ExConns.dwNumEntries = j;

            return true;
        }

        private static string convert_state(int state)
        {
            string strg_state = "";
            switch (state)
            {
                case MIB_TCP_STATE_CLOSED: strg_state = "CLOSED"; break;
                case MIB_TCP_STATE_LISTEN: strg_state = "LISTEN"; break;
                case MIB_TCP_STATE_SYN_SENT: strg_state = "SYN_SENT"; break;
                case MIB_TCP_STATE_SYN_RCVD: strg_state = "SYN_RCVD"; break;
                case MIB_TCP_STATE_ESTAB: strg_state = "ESTAB"; break;
                case MIB_TCP_STATE_FIN_WAIT1: strg_state = "FIN_WAIT1"; break;
                case MIB_TCP_STATE_FIN_WAIT2: strg_state = "FIN_WAIT2"; break;
                case MIB_TCP_STATE_CLOSE_WAIT: strg_state = "CLOSE_WAIT"; break;
                case MIB_TCP_STATE_CLOSING: strg_state = "CLOSING"; break;
                case MIB_TCP_STATE_LAST_ACK: strg_state = "LAST_ACK"; break;
                case MIB_TCP_STATE_TIME_WAIT: strg_state = "TIME_WAIT"; break;
                case MIB_TCP_STATE_DELETE_TCB: strg_state = "DELETE_TCB"; break;
            }
            return strg_state;
        }

        public static bool GetProcessUDPConns(ref MIB_UDPTABLE_OWNER_PID UdpConns)
        {
            UInt32* ptable = (UInt32*)IntPtr.Zero;
            UInt32 dwSize = 0;
            GetExtendedUdpTable((IntPtr)ptable, ref dwSize, true, AFType.AF_INET, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0);
            char* tmp = stackalloc char[(int)dwSize];
            ptable = (UInt32*)tmp;

            if (GetExtendedUdpTable((IntPtr)ptable, ref dwSize, true, AFType.AF_INET, UDP_TABLE_CLASS.UDP_TABLE_OWNER_PID, 0) != NO_ERROR)
                return false;

            UdpConns.dwNumEntries = ptable[0];
            UdpConns.table = new MIB_UDPROW_OWNER_PID[UdpConns.dwNumEntries];
            MIB_UDPROW_OWNER_PID * row = (MIB_UDPROW_OWNER_PID*)&ptable[1];
            UInt32 j = 0;
            for (int i = 0; i < UdpConns.dwNumEntries; i++)
            {
                if(!IP.specialIP(row[i].dwLocalAddr))
                    UdpConns.table[j++] = row[i];
            }

            UdpConns.dwNumEntries = j;

            return true;
        }

        private static UInt16 convert_Port(UInt32 dwPort)
        {
            byte[] b = new Byte[2];
            // high weight byte
            b[0] = byte.Parse((dwPort >> 8).ToString());
            // low weight byte
            b[1] = byte.Parse((dwPort & 0xFF).ToString());
            return BitConverter.ToUInt16(b, 0);
        }

        public static bool tcpActive(MIB_TCPROW_OWNER_PID trow)
        {
            bool res = false;

            switch(trow.dwState)
            {
                case MIB_TCP_STATE_CLOSED:
                case MIB_TCP_STATE_LISTEN:
                    res = false;
                    break;
                case MIB_TCP_STATE_SYN_SENT:
                case MIB_TCP_STATE_SYN_RCVD:
                case MIB_TCP_STATE_ESTAB:
                    res = true;
                    break;
                case MIB_TCP_STATE_FIN_WAIT1:
                case MIB_TCP_STATE_FIN_WAIT2:
                case MIB_TCP_STATE_CLOSE_WAIT:
                case MIB_TCP_STATE_CLOSING:
                case MIB_TCP_STATE_LAST_ACK:
                case MIB_TCP_STATE_TIME_WAIT:
                case MIB_TCP_STATE_DELETE_TCB:
                    res = false;
                    break;
            }

            return res & (trow.dwRemotePort != 0);
        }

        public static HashSet<UInt32> netActiveProcesses()
        {
            HashSet<UInt32> processes = new HashSet<UInt32>();
            MIB_UDPTABLE_OWNER_PID utable = new MIB_UDPTABLE_OWNER_PID();
            bool res = GetProcessUDPConns(ref utable);
            if(res)
            {
                for(int i = 0; i < utable.dwNumEntries; i++)
                {
                    MIB_UDPROW_OWNER_PID urow = utable.table[i];
                    if (!processes.Contains(urow.dwOwningPid))
                        processes.Add(urow.dwOwningPid);
                }
            }

            MIB_TCPTABLE_OWNER_PID ttable = new MIB_TCPTABLE_OWNER_PID();
            res = GetProcessTCPConns(ref ttable);
            if(res)
            {
                for(int i = 0; i < ttable.dwNumEntries; i++)
                {
                    MIB_TCPROW_OWNER_PID trow = ttable.table[i];
                    if (!processes.Contains(trow.dwOwningPid) && tcpActive(trow))
                        processes.Add(trow.dwOwningPid);
                }
            }

            return processes;
        }
    }
}
