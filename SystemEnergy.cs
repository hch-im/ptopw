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
    public class SystemEnergy
    {
        public double diskEng;
        public double memEng;
        public double nicEng;
        public double cpuEng;
        public double diskPower;
        public double memPower;
        public double nicPower;
        public double cpuPower;
        public double interval;
        public double weightedPower;

        public SystemEnergy()
	    {
		    diskEng = 0;
		    memEng = 0;
		    nicEng = 0;
		    cpuEng = 0;

            diskPower = 0;
            memPower = 0;
            nicPower = 0;
            cpuPower = 0;
            weightedPower = 0;
            interval = 0;
	    }

        public double totalActivePower()
        {
            return diskPower + memPower + nicPower + cpuPower;
        }

        public double totalSystemEnergy()
        {
            return cpuEng + memEng + nicEng + diskEng;
        }

        public void computeWeightedPower(double last, double weight)
        {
            weightedPower = totalActivePower() * weight + last * (1 - weight);
        }
    }
}
