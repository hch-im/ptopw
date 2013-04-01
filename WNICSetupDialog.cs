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
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace ptopw
{
    struct wnic_names
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2560)]
        public byte[] dst_addr;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
        public int[] index;
        public int number;
    }

    partial class WNICSetupDialog : Form
    {
        [DllImport("mydll.dll", EntryPoint = "WirelessNetInterfaceNames", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool WirelessNetInterfaceNames(ref wnic_names names);

        DataCollection dc;
        private wnic_names names;
        public WNICSetupDialog(DataCollection dc)
        {
            InitializeComponent();
            this.dc = dc;
        }

        private void initList()
        {
            names.dst_addr = new byte[2560];
            names.index = new int[10];
            bool res = WirelessNetInterfaceNames(ref names);
            for (int i = 0; i < names.number; i++){
                string   str   =   System.Text.Encoding.Default.GetString(names.dst_addr, 0 + i * 256, 256);
                wnicCheckList.Items.Add(str);
            }
        }

        private void WNICSetupDialog_Load(object sender, EventArgs e)
        {
            initList();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (wnicCheckList.SelectedIndex < 0)
            {
                MessageBox.Show("Please select a wireless network interface!");
            }
            else
            {
                dc.setNetworkInterface(names.index[wnicCheckList.SelectedIndex]);
                Close();
            }
        }
    }
}
