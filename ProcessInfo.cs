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
    public class ProcessInfo
    {
        public int no;

        public String name;
        public double cpuTime;
        public UInt64 dread;//bytes, disk read
        public UInt64 dwrite;//bytes  disk write
        public float nrecv;//bytes/sec   network received
        public float nsend;//bytes/sec   network send
        public bool networkActive;
        public float minput;//pages/sec  memory write
        public float moutput;//pages/sec memory read

	    public ProcessInfo()
	    {
		    no  = 0; 
		    cpuTime = 0;
		    dread = 0;
		    dwrite = 0;
		    nrecv = 0;//bytes/sec   network received
		    nsend = 0;//bytes/sec   network send
            networkActive = false;
		    minput = 0;//pages/sec  memory write
		    moutput = 0;//pages/sec memory read
	    }
    }
}
