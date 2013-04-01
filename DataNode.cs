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
    public class DataNode
    {

	    public long time;
        public ArrayList  pData;
        public SystemInfo sData;
        public Hashtable pEnergy;
        public SystemEnergy sEnergy;

        public DataNode()
	    {
		    time = 0;
		    pData = new ArrayList();
		    sData = null;
		    pEnergy = new Hashtable();
		    sEnergy = null;
	    }

        public ProcessEnergy GetProcessEnergy(int id)
        {
            if (pEnergy.ContainsKey(id))
                return (ProcessEnergy)pEnergy[id];

            return null;
        }

        public ArrayList GetProcessEnergyList()
        {
            ArrayList list;
            if (pEnergy != null)
                list = new ArrayList(pEnergy.Values);
            else
                list = new ArrayList();

            return list;
        }         
    }
}
