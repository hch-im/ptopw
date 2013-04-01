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
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Threading;

namespace ptopw
{
    public partial class MainWindow : Form
    {
        private DataCollection dc = null;
        private static String DOUBLE_FORMAT = "{0:0.000}";
        private delegate void dataGridViewDelegate(DataNode dn, int number);
        public int RefreshInterval = 1200;
        public int ProcessNumber = 20;
        public int showType = 1;
        private ProcessEnergyCompare compare = new ProcessEnergyCompare();

        private DataNode dn = null;
//        private Object DataNodeLock = new Object();
        public Object order = new Object();

        private Thread refreshThread;
        private bool refreshThreadActive = false;
        public ArrayList list = new ArrayList();
        private Hashtable param = new Hashtable();

        private readonly String[] EnergyGroupBoxTitle = { "Component Energy Consumption (Joule)", 
                                                           "Process Energy Consumption (Joule)" };
        private readonly String[] PowerGroupBoxTitle = { "Component Power (Watt)", 
                                                          "Process Power (Watt)" };

        public MainWindow()
        {
            InitializeComponent();
            CenterToParent();
            //close cross-thread call alert
            Control.CheckForIllegalCrossThreadCalls = false;
            refreshThread = new Thread(RefreshData);
            refreshThread.IsBackground = true;
            refreshThreadActive = true;
            refreshThread.Start();
            //init views
            sortToolStripComboBox.SelectedIndex = 0;
            energyToolStripMenuItem.Checked = true;
            powerToolStripMenuItem.Checked = false;
        }

        public void SetDataCollection(DataCollection dc)
        {
            this.dc = dc;
        }

        private void updateDataView(DataNode dn, int number)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new dataGridViewDelegate(updateDataView), new object[] { dn, number });
            }
            else
            {
                ArrayList pe = dn.GetProcessEnergyList();

                if (pe == null || pe.Count == 0) return;
                pe.Sort(compare);
                
                processDataGridView.Rows.Clear();

                foreach (ProcessEnergy eng in pe)
                {
                    Object[] objs = new Object[8];
                    if (showType == 1)
                    {
//                        double total = eng.cpuEng + eng.memEng + eng.diskEng + eng.nicEng;
//                        objs[1] = NonEmptyString(string.Format(DOUBLE_FORMAT, total));
//                        objs[2] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.cpuEng));
//                        objs[3] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.memEng));
//                        objs[4] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.diskEng));
//                        objs[5] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.nicEng));
                    }
                    else
                    {
                        double total = eng.diskPower + eng.memPower + eng.cpuPower + eng.nicPower;
                        objs[1] = NonEmptyString(string.Format(DOUBLE_FORMAT, total));
                        objs[2] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.cpuPower));
                        objs[3] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.memPower));
                        objs[4] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.diskPower));
                        objs[5] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.nicPower));
                    }
                    objs[0] = NonEmptyString(eng.name);
                    processDataGridView.Rows.Add(objs);
               
                    if ((number--) == 0)
                        break;
                }

             }

        }

        private void processDataGridView_DataSourceChanged(Object sender, EventArgs e)
        {
            processDataGridView.Refresh();
        }
 

        public void UpdateDataNode(DataNode data)
        {
            if(data != null)
            {
//                lock (DataNodeLock)
//                {
                    dn = data;
//                }
            }
        }

        private void closeRefreshThread()
        {
            refreshThreadActive = false;
        }

        public bool isRefreshThreadActive()
        {
            return refreshThreadActive;
        }

        public void RefreshData()
        {
            while (refreshThreadActive)
            {
                if (dn == null)
                {
                    Thread.Sleep(100);
                    continue;
                }

                if (this.IsDisposed)
                {
                    refreshThreadActive = false;
                    return;
                }

                if(showType == 1)
                    continue;

                try
                {
//                    lock (DataNodeLock)
//                    {
                        //update energy labels
                        SystemEnergy se = dn.sEnergy;
                        if (se != null)
                        {

                            if(showType == 1)
                            {
//                                cpuEnergyLabel.Text = string.Format(DOUBLE_FORMAT, se.cpuEng);
//                                memEnergyLabel.Text = string.Format(DOUBLE_FORMAT, se.memEng);//4321.125
//                                diskEnergyLabel.Text = string.Format(DOUBLE_FORMAT, se.diskEng);
//                                wnicEnergyLabel.Text = string.Format(DOUBLE_FORMAT, se.nicEng);
                            }
                            else
                            {
                                cpuEnergyLabel.Text = string.Format(DOUBLE_FORMAT, se.cpuPower);
                                memEnergyLabel.Text = string.Format(DOUBLE_FORMAT, se.memPower);//4321.125
                                diskEnergyLabel.Text = string.Format(DOUBLE_FORMAT, se.diskPower);
                                wnicEnergyLabel.Text = string.Format(DOUBLE_FORMAT, se.nicPower);
                            }

//                        }

                        //update data grid view
                        updateDataView(dn, ProcessNumber);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.StackTrace); }

                Thread.Sleep(RefreshInterval);
            }
        }

        private String NonEmptyString(String str)
        {
            if(str == null)
                return "";
            return str;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setupWirelessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WNICSetupDialog dialog = new WNICSetupDialog(dc);
            dialog.ShowDialog();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(isRefreshThreadActive())
                closeRefreshThread();

            if(!IsDisposed)
                this.Dispose();
        }

        private void energyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showType = 1;
            clearView();

            energyToolStripMenuItem.Checked = true;
            powerToolStripMenuItem.Checked = false;
            timeTextBox.Visible = true;
            secLabel.Visible = true;
            calButton.Visible = true;
            componentGroupBox.Text = EnergyGroupBoxTitle[0];
            processGroupBox.Text = EnergyGroupBoxTitle[1];
        }

        private void powerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showType = 2;
            clearView();

            energyToolStripMenuItem.Checked = false;
            powerToolStripMenuItem.Checked = true;
            timeTextBox.Visible = false;
            secLabel.Visible = false;
            calButton.Visible = false;
            componentGroupBox.Text = PowerGroupBoxTitle[0];
            processGroupBox.Text = PowerGroupBoxTitle[1];
        }

        public void SetParams(Hashtable param)
        {
            this.param = param;
            RefreshInterval = Convert.ToInt32(param["RefreshInterval"]);
            ProcessNumber = Convert.ToInt32(param["ProcessNumber"]);
            showType = Convert.ToInt32(param["ShowType"]);
        }

        private void sortToolStripComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sortToolStripComboBox.SelectedIndex == -1)
                return;

            compare.setCompareMethod(sortToolStripComboBox.SelectedIndex + 1);
        }

        private void clearView()
        {
            cpuEnergyLabel.Text = "";
            memEnergyLabel.Text = "";
            diskEnergyLabel.Text = "";
            wnicEnergyLabel.Text = "";

            processDataGridView.Rows.Clear();
        }

        public void ProcessException(MyException mex)
        {
            MessageBox.Show(mex.s, "Alert : " + mex.error,
                   MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            switch(mex.error)
            {
                case MyException.MSG_PARSE_ERROR:
//                case MyException.MSG_CREATE_COUNTER_ERROR:
//                case MyException.MSG_UPDATE_COUNTER_ERROR:
//                case MyException.MSG_READ_MESSAGE_ERROR:
//                case MyException.MSG_WRITE_MESSAGE_ERROR:
                case MyException.MSG_CREATE_APISERVER_ERROR:
                case MyException.MSG_INIT_SYSTEMINFO_ERROR:
                case MyException.MSG_INIT_STAT_COUNTER_ERROR:
                case MyException.MSG_INIT_LOG_INUSE_ERROR:
                    Close();
                    break;
                default:
                    break;
            }
            
        }

        private void calButton_Click(object sender, EventArgs e)
        {
            if("".Equals(timeTextBox.Text.Trim()))
            {
                MessageBox.Show("Please input the time!", "Alert",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            long time = 0;

            try{
                time = Convert.ToInt32(timeTextBox.Text);
            }catch(Exception ex)
            {
                MessageBox.Show("Please input a correct time!","Alert",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
             }

            clearView();
            ArrayList al = dc.retrieveData(time * 1000);
            Hashtable engList = new Hashtable();
            SystemEnergy tse = new SystemEnergy();
            //calculate
            foreach(DataNode dn in al)
            {
                tse.cpuEng += dn.sEnergy.cpuEng;
                tse.diskEng += dn.sEnergy.diskEng;
                tse.memEng += dn.sEnergy.memEng;
                tse.nicEng += dn.sEnergy.nicEng;

                foreach(ProcessEnergy pe in dn.pEnergy.Values)
                {
                    ProcessEnergy tpe = null;
                    if(engList.ContainsKey(pe.GetID()))
                    {
                        tpe = (ProcessEnergy)engList[pe.GetID()];
                    }
                    else
                    {
                        tpe = new ProcessEnergy();
                        tpe.no = pe.no;
                        tpe.name = pe.name;
                        engList.Add(pe.no, tpe);
                    }

                    tpe.cpuEng += pe.cpuEng;
                    tpe.diskEng += pe.diskEng;
                    tpe.memEng += pe.memEng;
                    tpe.nicEng += pe.nicEng;
                }
            }
            //display
            cpuEnergyLabel.Text = string.Format(DOUBLE_FORMAT, tse.cpuEng);
            memEnergyLabel.Text = string.Format(DOUBLE_FORMAT, tse.memEng);
            diskEnergyLabel.Text = string.Format(DOUBLE_FORMAT, tse.diskEng);
            wnicEnergyLabel.Text = string.Format(DOUBLE_FORMAT, tse.nicEng);
            
            ArrayList list = new ArrayList(engList.Values);
            if (list.Count == 0) return;
            list.Sort(compare);

            foreach (ProcessEnergy eng in list)
            {
                Object[] objs = new Object[8];
                double total = eng.cpuEng + eng.memEng + eng.diskEng + eng.nicEng;
                objs[1] = NonEmptyString(string.Format(DOUBLE_FORMAT, total));
                objs[2] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.cpuEng));
                objs[3] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.memEng));
                objs[4] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.diskEng));
                objs[5] = NonEmptyString(string.Format(DOUBLE_FORMAT, eng.nicEng));
             
                objs[0] = NonEmptyString(eng.name);
                processDataGridView.Rows.Add(objs);               
             }
        }
    }
}
