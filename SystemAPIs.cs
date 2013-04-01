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
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ptopw
{
    public class WinApi
    {
        [DllImport("kernel32.dll")]
        public static extern void GetSystemInfo([MarshalAs(UnmanagedType.Struct)] ref SYSTEM_INFO lpSystemInfo);
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool GetSystemPowerStatus([In, Out] ref PowerInfo systemPowerStatus);
        [DllImport("kernel32.dll")]
        public static extern bool GetProcessIoCounters(IntPtr ProcessHandle, out IO_COUNTERS IoCounters);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SYSTEM_INFO
    {
        internal _PROCESSOR_INFO_UNION uProcessorInfo;
        public uint dwPageSize;
        public IntPtr lpMinimumApplicationAddress;
        public IntPtr lpMaximumApplicationAddress;
        public IntPtr dwActiveProcessorMask;
        public uint dwNumberOfProcessors;
        public uint dwProcessorType;
        public uint dwAllocationGranularity;
        public ushort dwProcessorLevel;
        public ushort dwProcessorRevision;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct _PROCESSOR_INFO_UNION
    {
        [FieldOffset(0)]
        internal uint dwOemId;
        [FieldOffset(0)]
        internal ushort wProcessorArchitecture;
        [FieldOffset(2)]
        internal ushort wReserved;
    }

    public struct WNICInfo
    {
        public int index;
        public UInt32 In; //octets
        public UInt32 Out;//octets
        public UInt32 speed;//bits per second
    }

    public enum BatteryFlag
    {
        High = 1,        //the battery capacity is at more than 66 percent
        Low = 2,         //the battery capacity is at less than 33 percent
        Critical = 4,    //the battery capacity is at less than five percent
        Charging = 8,    //Charging
        NOBattery = 128, //no system battery
        UnKnown = 255    //unable to read the battery flag information
    }

    public struct PowerInfo
    {
        public byte ACLineStatus;//0:offline 1:online
        public byte BatteryFlag;
        public byte BatteryLifePercent;//The percentage of full battery charge remaining. 255:unknown
        public byte Reserved1;//must be zero
        public UInt32 BatteryLifeTime;//The number of seconds of battery life remaining -1:unknown
        public UInt32 BatteryFullLifeTime;//The number of seconds of battery life when at full charge
    }

    public struct IO_COUNTERS
    {
        public ulong ReadOperationCount;
        public ulong WriteOperationCount;
        public ulong OtherOperationCount;
        public ulong ReadTransferCount;
        public ulong WriteTransferCount;
        public ulong OtherTransferCount;

        public void init(){
            ReadOperationCount = 0;
            WriteOperationCount = 0;
            OtherOperationCount = 0;
            ReadTransferCount = 0;
            WriteTransferCount = 0;
            OtherTransferCount = 0;
        }
    }
}
