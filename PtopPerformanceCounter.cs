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
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ptopw
{
    public class PtopPerformanceCounter
    {
        public static String PTOPW_CATEGORY = "wsu_ptopw";
        public static int SYSTEM_COUNTER_NUM = 5;
        private ArrayList counterNames = new ArrayList();
        private ArrayList counterType = new ArrayList();
        private Hashtable pCounters = new Hashtable();
        private PerformanceCounter[] sysCounters = null;

        public PtopPerformanceCounter()
        {
            counterNames.Add("Total Power");
            counterType.Add(PerformanceCounterType.NumberOfItems64);
            counterNames.Add("CPU Power");
            counterType.Add(PerformanceCounterType.NumberOfItems64);
            counterNames.Add("Memory Power");
            counterType.Add(PerformanceCounterType.NumberOfItems64);
            counterNames.Add("Disk Power");
            counterType.Add(PerformanceCounterType.NumberOfItems64);
            counterNames.Add("Wireless Network Card Power");
            counterType.Add(PerformanceCounterType.NumberOfItems64);
        }

        public bool deletePerformanceCounter()
        {
            if (PerformanceCounterCategory.Exists(PTOPW_CATEGORY))
            {
                PerformanceCounterCategory.Delete(PTOPW_CATEGORY);
                try
                {
                    PerformanceCounterCategory.Delete(PTOPW_CATEGORY);
                }catch(Exception ex)
                {
                    return true;
                }
                return false;
            }

            return true;
        }

        public void CreateCategory()
        {
            try
            {
                if (!deletePerformanceCounter())
                {
                    return;
                }

                CounterCreationDataCollection CounterDatas = new CounterCreationDataCollection();
                for (int i = 0; i < counterNames.Count; i++ )
                {
                    CounterCreationData counter = new CounterCreationData();
                    counter.CounterName = (String)counterNames[i];
                    counter.CounterHelp = "";
                    counter.CounterType = (PerformanceCounterType)counterType[i];
                    CounterDatas.Add(counter);
                }

                PerformanceCounterCategory cat = PerformanceCounterCategory.Create(PTOPW_CATEGORY, "", 
                                        PerformanceCounterCategoryType.MultiInstance, CounterDatas);
                cat = PerformanceCounterCategory.Create(PTOPW_CATEGORY, "", 
                                        PerformanceCounterCategoryType.MultiInstance, CounterDatas);
            }
            catch (Exception ex)
            {
                //throw new MyException(ex.Message, MyException.MSG_CREATE_COUNTER_ERROR);
            }
        }

        public void addOrUpdatePerformanceCounter(DataNode dn)
        {
            if(dn != null)
            {
                try
                {
                    addOrUpdateSystemCounters(dn.sEnergy);

                    if (dn.pEnergy != null)
                        foreach (ProcessEnergy pe in dn.pEnergy.Values)
                        {
                            addOrUpdateProcessCounters(pe);
                        }
                }
                catch (MyException ex)
                {
                    throw ex;
                }
            }
        }

        private void addOrUpdateSystemCounters(SystemEnergy se)
        {
            try
            {
                int i;
                if (sysCounters == null)
                {
                    sysCounters = new PerformanceCounter[SYSTEM_COUNTER_NUM];

                    for (i = 0; i < SYSTEM_COUNTER_NUM; i++)
                    {
                        sysCounters[i] = new PerformanceCounter(PTOPW_CATEGORY, (String)counterNames[i], "_Total", false);
                        sysCounters[i].RawValue = 0;
                    }
                }

                i = 0;
                sysCounters[i++].RawValue = (long)(se.totalActivePower() * 1000);//uw
                sysCounters[i++].RawValue = (long)(se.cpuPower * 1000);//uw
                sysCounters[i++].RawValue = (long)(se.memPower * 1000);//uw
                sysCounters[i++].RawValue = (long)(se.diskPower * 1000);//uw
                sysCounters[i++].RawValue = (long)(se.nicPower * 1000);//uw
            }
            catch (System.Exception ex)
            {
                throw new MyException(ex.Message, MyException.MSG_UPDATE_COUNTER_ERROR);	
            }

        }

        private void addOrUpdateProcessCounters(ProcessEnergy pe)
        {
            try{
                PerformanceCounter[] counters = null;
                int i;

                if(!pCounters.Contains(pe.no))
                {
                    counters = new PerformanceCounter[SYSTEM_COUNTER_NUM];
                    for (i = 0; i < SYSTEM_COUNTER_NUM; i++)
                    {
                        counters[i] = new PerformanceCounter(PTOPW_CATEGORY
                                        ,(String)counterNames[i]
                                        , pe.no + "_" + pe.name ,false);
                        counters[i].RawValue = 0;
                    }

                    pCounters.Add(pe.no, counters);
                }
                else
                {
                    counters = (PerformanceCounter[])pCounters[pe.no];
                }

                i = 0;
                counters[i++].RawValue = (long)(pe.totalActivePower() * 1000);//uw
                counters[i++].RawValue = (long)(pe.cpuPower * 1000);//uw
                counters[i++].RawValue = (long)(pe.memPower * 1000);//uw
                counters[i++].RawValue = (long)(pe.diskPower * 1000);//uw
                counters[i++].RawValue = (long)(pe.nicPower * 1000);//uw
            }
            catch (System.Exception ex)
            {
                throw new MyException(ex.Message, MyException.MSG_UPDATE_COUNTER_ERROR);	
            }
        }
    }
}
