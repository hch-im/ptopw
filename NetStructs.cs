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

namespace ptopw
{
    public class AFType
    {
        public static int AF_INET = 2;
        public static int AF_INET6 = 23;
    }
    public class IP
    {
        public static UInt32 localhost = 16777343;//127.0.0.1
        public static UInt32 systemmsg = 0;//0.0.0.0
        public static bool specialIP(UInt32 ip)
        {
            return ip == localhost || ip == systemmsg;
        }
    }
#region(UDP Structure)
    [StructLayout(LayoutKind.Sequential)]
    public struct MIB_UDPSTATS
    {
        public int dwInDatagrams;
        public int dwNoPorts;
        public int dwInErrors;
        public int dwOutDatagrams;
        public int dwNumAddrs;
    }

    public struct MIB_UDPTABLE
    {
        public int dwNumEntries;
        public MIB_UDPROW[] table;
    }

    public struct MIB_UDPROW
    {
        public IPEndPoint Local;
    }

    public struct MIB_UDPTABLE_OWNER_PID
    {
        public UInt32 dwNumEntries;
        public MIB_UDPROW_OWNER_PID[] table;
    }

    public struct MIB_UDPROW_OWNER_PID
    {
        public UInt32 dwLocalAddr;
        public UInt32 dwLocalPort;
        public UInt32 dwOwningPid;
    }

    public enum  UDP_TABLE_CLASS{
        UDP_TABLE_BASIC,
        UDP_TABLE_OWNER_PID,
        UDP_TABLE_OWNER_MODULE 
    }
#endregion
#region(TCP Structure)
    [StructLayout(LayoutKind.Sequential)]
    public struct MIB_TCPSTATS
    {
        public int dwRtoAlgorithm;
        public int dwRtoMin;
        public int dwRtoMax;
        public int dwMaxConn;
        public int dwActiveOpens;
        public int dwPassiveOpens;
        public int dwAttemptFails;
        public int dwEstabResets;
        public int dwCurrEstab;
        public int dwInSegs;
        public int dwOutSegs;
        public int dwRetransSegs;
        public int dwInErrs;
        public int dwOutRsts;
        public int dwNumConns;
    }


    public struct MIB_TCPTABLE
    {
        public int dwNumEntries;
        public MIB_TCPROW[] table;
    }

    public struct MIB_TCPROW
    {
        public string StrgState;
        public int iState;
        public IPEndPoint Local;
        public IPEndPoint Remote;
    }

    public struct MIB_TCPTABLE_OWNER_PID 
    {
        public UInt32 dwNumEntries;
        public MIB_TCPROW_OWNER_PID[] table;
    }

    public struct MIB_TCPROW_OWNER_PID 
    {
        public UInt32 dwState;
        public UInt32 dwLocalAddr;
        public UInt32 dwLocalPort;
        public UInt32 dwRemoteAddr;
        public UInt32 dwRemotePort;
        public UInt32 dwOwningPid;
    }
    public enum TCP_TABLE_CLASS
    {
      TCP_TABLE_BASIC_LISTENER,
      TCP_TABLE_BASIC_CONNECTIONS,
      TCP_TABLE_BASIC_ALL,
      TCP_TABLE_OWNER_PID_LISTENER,
      TCP_TABLE_OWNER_PID_CONNECTIONS,
      TCP_TABLE_OWNER_PID_ALL,
      TCP_TABLE_OWNER_MODULE_LISTENER,
      TCP_TABLE_OWNER_MODULE_CONNECTIONS,
      TCP_TABLE_OWNER_MODULE_ALL 
    }
#endregion

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct MIB_IFROW
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x100)]
        public string wszName;
        public uint dwIndex;
        public uint dwType;
        public uint dwMtu;
        public uint dwSpeed;
        public uint dwPhysAddrLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] bPhysAddr;
        public uint dwAdminStatus;
        public uint dwOperStatus;
        public uint dwLastChange;
        public uint dwInOctets;
        public uint dwInUcastPkts;
        public uint dwInNUcastPkts;
        public uint dwInDiscards;
        public uint dwInErrors;
        public uint dwInUnknownProtos;
        public uint dwOutOctets;
        public uint dwOutUcastPkts;
        public uint dwOutNUcastPkts;
        public uint dwOutDiscards;
        public uint dwOutErrors;
        public uint dwOutQLen;
        public uint dwDescrLen;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100)]
        public byte[] bDescr;
    }
}
