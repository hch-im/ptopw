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
    public interface PtopwAPIs
    {
        /*Return the component Information
         * flag : 0  both energy and power
         * flag : 1 only energy
         * flag : 2 only power
         */
         double[] ComponentInfo(int flag);
        /*Return the process Information
         * flag : 0  both energy and power
         * flag : 1 only energy
         * flag : 2 only power
         */
         double[] ProcessInfo(int flag, int pid);

         double systemEnergy(long time);
         double cpuEnergy(long time);
         double memoryEnergy(long time);
         double diskEnergy(long time);
         double nicEnergy(long time);

         double totalProcessEnergy(long time, int pid);
         double cpuProcessEnergy(long time, int pid);
         double memoryProcessEnergy(long time, int pid);
         double diskProcessEnergy(long time, int pid);
         double nicProcessEnergy(long time, int pid);

         Hashtable processEnergy();
         Hashtable processAvgPower();
         double systemAvgPower();
    }
}
