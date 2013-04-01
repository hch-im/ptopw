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
using System.Management;

namespace ptopw
{
    public class Processor
    {

        public UInt32 NumberOfCores;
        public UInt32 MaxClockSpeed;
        public UInt32 CurrentClockSpeed;
        public UInt32 L2CacheSize;
        public UInt32 L3CacheSize;
        public UInt16 CurrentVoltage;

        public void update(ManagementObject obj)
        {
            NumberOfCores = (UInt32)obj["NumberOfCores"];
            MaxClockSpeed = (UInt32)obj["MaxClockSpeed"];//MHz
            CurrentClockSpeed = (UInt32)obj["CurrentClockSpeed"];//MHz
            L2CacheSize = (UInt32)obj["L2CacheSize"];
            L3CacheSize = (UInt32)obj["L3CacheSize"];
            CurrentVoltage = (UInt16)obj["CurrentVoltage"];//divide 10 is the real
        }
    }

}
