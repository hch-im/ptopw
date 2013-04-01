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
    public class ProcessEnergy
    {
        public int no;
        public String name;
        public double diskEng;
        public double memEng;
        public double nicEng;
        public double cpuEng;
        public double maxEng;
        public double diskPower;
        public double memPower;
        public double nicPower;
        public double cpuPower;
        public double weightedPower;
        public double timeSave;

	    public ProcessEnergy()
	    {
		    no  = 0; 
		    diskEng = 0;
		    memEng = 0;
		    nicEng = 0;
		    cpuEng = 0;
            maxEng = 0;
            diskPower = 0;
            memPower = 0;
            nicPower = 0;
            cpuPower = 0;
            weightedPower = 0;
            timeSave = 0;
	    }

        public int GetID()
        {
            return no;
        }

        public void SetMaxvalue(double k)
        {
            maxEng = k;
        }

        public double GetTotalEnergy()
        {
            return (diskEng+cpuEng+nicEng+memEng);
        }

        public double totalActivePower()
        {
            return diskPower + memPower + nicPower + cpuPower;
        }

        public void computeWeightedPower(double last, double weight)
        {
            weightedPower = totalActivePower() * weight + last * (1 - weight);
        }
    }

    public class ProcessEnergyCompare:System.Collections.IComparer
    {
        public const int COMPARE_TOTAL = 1;
        public const int COMPARE_CPU = 2;
        public const int COMPARE_MEM = 3;
        public const int COMPARE_DISK = 4;
        public const int COMPARE_WNIC = 5;

        private int compareMethod = COMPARE_TOTAL;
        public void setCompareMethod(int m)
        {
            compareMethod = m;
        }

        public int Compare(object pe1, object pe2)
        {
            int value = 0;
            switch (compareMethod)
            {
                case COMPARE_CPU:
                    value = (int)(((ProcessEnergy)pe2).cpuEng * 100 - ((ProcessEnergy)pe1).cpuEng * 100);
                    break;
                case COMPARE_MEM:
                    value = (int)(((ProcessEnergy)pe2).memEng * 100 - ((ProcessEnergy)pe1).memEng * 100);
                    break;
                case COMPARE_DISK:
                    value = (int)(((ProcessEnergy)pe2).diskEng * 100 - ((ProcessEnergy)pe1).diskEng * 100);
                    break;
                case COMPARE_WNIC:
                    value = (int)(((ProcessEnergy)pe2).nicEng * 100 - ((ProcessEnergy)pe1).nicEng * 100);
                    break;
                case COMPARE_TOTAL:
                    value = (int)(((ProcessEnergy)pe2).GetTotalEnergy() * 100 - ((ProcessEnergy)pe1).GetTotalEnergy() * 100);
                    break;
                default:
                    value = (int)(((ProcessEnergy)pe2).GetTotalEnergy() * 100 - ((ProcessEnergy)pe1).GetTotalEnergy() * 100);
                    break;
            }

            return value;
        }
    }

}
