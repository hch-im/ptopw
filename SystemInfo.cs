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

namespace ptopw
{
    public class SystemInfo
    {
        public double time;
        public WNICInfo winfo;

        public float nrecv;//bytes/sec   network received
        public float nsend;//bytes/sec   network send
        public int NetActiveProcessNum;

        public UInt32 memRead;//pages/sec
        public UInt32 memWrite;//pages/sec
        public UInt32 memCopy;
        public int pageSize;

        public long cpuFrequency;
        public double cpuTime;
        public UInt16 voltage;
        public double timeInterval;

        public UInt64 percentDiskReadTime;
        public UInt64 percentDiskWriteTime;
        public UInt64 percentDiskIdleTime;
        public UInt64 diskRead;
        public UInt64 diskWrite;

        public Processor pro;

	    public SystemInfo()
	    {
            percentDiskReadTime = 0;
            percentDiskWriteTime = 0;
            percentDiskIdleTime = 0;
            diskRead = diskWrite = 0;

            pageSize = 0;
		    nrecv = 0;//bytes/sec   network received
		    nsend = 0;//bytes/sec   network send

            memRead = memWrite = memCopy = 0;//pages/sec  memory write

            cpuFrequency = 0;
            cpuTime = 0;
            voltage = 0;
            NetActiveProcessNum = 0;
        }
    }
}
