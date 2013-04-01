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
namespace ptopw
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.componentGroupBox = new System.Windows.Forms.GroupBox();
            this.calButton = new System.Windows.Forms.Button();
            this.secLabel = new System.Windows.Forms.Label();
            this.timeTextBox = new System.Windows.Forms.TextBox();
            this.wnicEnergyLabel = new System.Windows.Forms.Label();
            this.wnicLabel = new System.Windows.Forms.Label();
            this.diskEnergyLabel = new System.Windows.Forms.Label();
            this.diskLabel = new System.Windows.Forms.Label();
            this.memEnergyLabel = new System.Windows.Forms.Label();
            this.memLabel = new System.Windows.Forms.Label();
            this.cpuEnergyLabel = new System.Windows.Forms.Label();
            this.cpuLabel = new System.Windows.Forms.Label();
            this.processGroupBox = new System.Windows.Forms.GroupBox();
            this.processDataGridView = new System.Windows.Forms.DataGridView();
            this.ProcessNameColum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalEnergy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CPUEnergy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MEMEnergy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiskEnergy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WNICEnergy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setupWirelessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.energyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.powerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sortToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutPTopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.componentGroupBox.SuspendLayout();
            this.processGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processDataGridView)).BeginInit();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // componentGroupBox
            // 
            this.componentGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.componentGroupBox.AutoSize = true;
            this.componentGroupBox.Controls.Add(this.calButton);
            this.componentGroupBox.Controls.Add(this.secLabel);
            this.componentGroupBox.Controls.Add(this.timeTextBox);
            this.componentGroupBox.Controls.Add(this.wnicEnergyLabel);
            this.componentGroupBox.Controls.Add(this.wnicLabel);
            this.componentGroupBox.Controls.Add(this.diskEnergyLabel);
            this.componentGroupBox.Controls.Add(this.diskLabel);
            this.componentGroupBox.Controls.Add(this.memEnergyLabel);
            this.componentGroupBox.Controls.Add(this.memLabel);
            this.componentGroupBox.Controls.Add(this.cpuEnergyLabel);
            this.componentGroupBox.Controls.Add(this.cpuLabel);
            this.componentGroupBox.Location = new System.Drawing.Point(12, 27);
            this.componentGroupBox.Name = "componentGroupBox";
            this.componentGroupBox.Size = new System.Drawing.Size(660, 56);
            this.componentGroupBox.TabIndex = 0;
            this.componentGroupBox.TabStop = false;
            this.componentGroupBox.Text = "Component Energy Consumption (Joule)";
            // 
            // calButton
            // 
            this.calButton.Location = new System.Drawing.Point(565, 14);
            this.calButton.Name = "calButton";
            this.calButton.Size = new System.Drawing.Size(75, 23);
            this.calButton.TabIndex = 10;
            this.calButton.Text = "Calculate";
            this.calButton.UseVisualStyleBackColor = true;
            this.calButton.Click += new System.EventHandler(this.calButton_Click);
            // 
            // secLabel
            // 
            this.secLabel.AutoSize = true;
            this.secLabel.Location = new System.Drawing.Point(546, 20);
            this.secLabel.Name = "secLabel";
            this.secLabel.Size = new System.Drawing.Size(20, 13);
            this.secLabel.TabIndex = 9;
            this.secLabel.Text = "(S)";
            // 
            // timeTextBox
            // 
            this.timeTextBox.Location = new System.Drawing.Point(472, 17);
            this.timeTextBox.Name = "timeTextBox";
            this.timeTextBox.Size = new System.Drawing.Size(71, 20);
            this.timeTextBox.TabIndex = 8;
            // 
            // wnicEnergyLabel
            // 
            this.wnicEnergyLabel.Location = new System.Drawing.Point(396, 20);
            this.wnicEnergyLabel.Name = "wnicEnergyLabel";
            this.wnicEnergyLabel.Size = new System.Drawing.Size(40, 13);
            this.wnicEnergyLabel.TabIndex = 7;
            this.wnicEnergyLabel.Text = "0.0";
            // 
            // wnicLabel
            // 
            this.wnicLabel.AutoSize = true;
            this.wnicLabel.Location = new System.Drawing.Point(350, 20);
            this.wnicLabel.Name = "wnicLabel";
            this.wnicLabel.Size = new System.Drawing.Size(39, 13);
            this.wnicLabel.TabIndex = 6;
            this.wnicLabel.Text = "WNIC:";
            // 
            // diskEnergyLabel
            // 
            this.diskEnergyLabel.Location = new System.Drawing.Point(279, 20);
            this.diskEnergyLabel.Name = "diskEnergyLabel";
            this.diskEnergyLabel.Size = new System.Drawing.Size(40, 13);
            this.diskEnergyLabel.TabIndex = 5;
            this.diskEnergyLabel.Text = "0.0";
            // 
            // diskLabel
            // 
            this.diskLabel.AutoSize = true;
            this.diskLabel.Location = new System.Drawing.Point(241, 20);
            this.diskLabel.Name = "diskLabel";
            this.diskLabel.Size = new System.Drawing.Size(31, 13);
            this.diskLabel.TabIndex = 4;
            this.diskLabel.Text = "Disk:";
            // 
            // memEnergyLabel
            // 
            this.memEnergyLabel.Location = new System.Drawing.Point(171, 20);
            this.memEnergyLabel.Name = "memEnergyLabel";
            this.memEnergyLabel.Size = new System.Drawing.Size(40, 13);
            this.memEnergyLabel.TabIndex = 3;
            this.memEnergyLabel.Text = "0.0";
            // 
            // memLabel
            // 
            this.memLabel.AutoSize = true;
            this.memLabel.Location = new System.Drawing.Point(118, 20);
            this.memLabel.Name = "memLabel";
            this.memLabel.Size = new System.Drawing.Size(47, 13);
            this.memLabel.TabIndex = 2;
            this.memLabel.Text = "Memory:";
            // 
            // cpuEnergyLabel
            // 
            this.cpuEnergyLabel.Location = new System.Drawing.Point(45, 20);
            this.cpuEnergyLabel.Name = "cpuEnergyLabel";
            this.cpuEnergyLabel.Size = new System.Drawing.Size(40, 13);
            this.cpuEnergyLabel.TabIndex = 1;
            this.cpuEnergyLabel.Text = "0.0";
            // 
            // cpuLabel
            // 
            this.cpuLabel.AutoSize = true;
            this.cpuLabel.Location = new System.Drawing.Point(7, 20);
            this.cpuLabel.Name = "cpuLabel";
            this.cpuLabel.Size = new System.Drawing.Size(32, 13);
            this.cpuLabel.TabIndex = 0;
            this.cpuLabel.Text = "CPU:";
            // 
            // processGroupBox
            // 
            this.processGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.processGroupBox.AutoSize = true;
            this.processGroupBox.Controls.Add(this.processDataGridView);
            this.processGroupBox.Location = new System.Drawing.Point(12, 82);
            this.processGroupBox.Name = "processGroupBox";
            this.processGroupBox.Size = new System.Drawing.Size(660, 470);
            this.processGroupBox.TabIndex = 1;
            this.processGroupBox.TabStop = false;
            this.processGroupBox.Text = "Process Energy Consumption (Joule)";
            // 
            // processDataGridView
            // 
            this.processDataGridView.AllowUserToAddRows = false;
            this.processDataGridView.AllowUserToDeleteRows = false;
            this.processDataGridView.AllowUserToOrderColumns = true;
            this.processDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.processDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.processDataGridView.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.processDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.processDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProcessNameColum,
            this.TotalEnergy,
            this.CPUEnergy,
            this.MEMEnergy,
            this.DiskEnergy,
            this.WNICEnergy});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.processDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.processDataGridView.Location = new System.Drawing.Point(0, 19);
            this.processDataGridView.Name = "processDataGridView";
            this.processDataGridView.ReadOnly = true;
            this.processDataGridView.RightToLeft = System.Windows.Forms.RightToLeft.No;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.BottomLeft;
            this.processDataGridView.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.processDataGridView.RowTemplate.ReadOnly = true;
            this.processDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.processDataGridView.ShowCellErrors = false;
            this.processDataGridView.ShowRowErrors = false;
            this.processDataGridView.Size = new System.Drawing.Size(660, 451);
            this.processDataGridView.TabIndex = 0;
            // 
            // ProcessNameColum
            // 
            this.ProcessNameColum.HeaderText = "Process Name";
            this.ProcessNameColum.Name = "ProcessNameColum";
            this.ProcessNameColum.ReadOnly = true;
            this.ProcessNameColum.Width = 135;
            // 
            // TotalEnergy
            // 
            this.TotalEnergy.HeaderText = "Total";
            this.TotalEnergy.Name = "TotalEnergy";
            this.TotalEnergy.ReadOnly = true;
            this.TotalEnergy.Width = 69;
            // 
            // CPUEnergy
            // 
            this.CPUEnergy.HeaderText = "CPU";
            this.CPUEnergy.Name = "CPUEnergy";
            this.CPUEnergy.ReadOnly = true;
            this.CPUEnergy.Width = 64;
            // 
            // MEMEnergy
            // 
            this.MEMEnergy.HeaderText = "Memory";
            this.MEMEnergy.Name = "MEMEnergy";
            this.MEMEnergy.ReadOnly = true;
            this.MEMEnergy.Width = 88;
            // 
            // DiskEnergy
            // 
            this.DiskEnergy.HeaderText = "Disk";
            this.DiskEnergy.Name = "DiskEnergy";
            this.DiskEnergy.ReadOnly = true;
            this.DiskEnergy.Width = 64;
            // 
            // WNICEnergy
            // 
            this.WNICEnergy.HeaderText = "Wireless Network Card";
            this.WNICEnergy.Name = "WNICEnergy";
            this.WNICEnergy.ReadOnly = true;
            this.WNICEnergy.Width = 191;
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.viewToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(684, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "Menu";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.closeToolStripMenuItem.Text = "Exit";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupWirelessToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.editToolStripMenuItem.Text = "Setup";
            // 
            // setupWirelessToolStripMenuItem
            // 
            this.setupWirelessToolStripMenuItem.Name = "setupWirelessToolStripMenuItem";
            this.setupWirelessToolStripMenuItem.Size = new System.Drawing.Size(214, 22);
            this.setupWirelessToolStripMenuItem.Text = "Wireless Network Interface";
            this.setupWirelessToolStripMenuItem.Click += new System.EventHandler(this.setupWirelessToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.sortToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.energyToolStripMenuItem,
            this.powerToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(103, 22);
            this.toolStripMenuItem1.Text = "Show";
            // 
            // energyToolStripMenuItem
            // 
            this.energyToolStripMenuItem.Name = "energyToolStripMenuItem";
            this.energyToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.energyToolStripMenuItem.Text = "Energy";
            this.energyToolStripMenuItem.Click += new System.EventHandler(this.energyToolStripMenuItem_Click);
            // 
            // powerToolStripMenuItem
            // 
            this.powerToolStripMenuItem.Name = "powerToolStripMenuItem";
            this.powerToolStripMenuItem.Size = new System.Drawing.Size(110, 22);
            this.powerToolStripMenuItem.Text = "Power";
            this.powerToolStripMenuItem.Click += new System.EventHandler(this.powerToolStripMenuItem_Click);
            // 
            // sortToolStripMenuItem
            // 
            this.sortToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sortToolStripComboBox});
            this.sortToolStripMenuItem.Name = "sortToolStripMenuItem";
            this.sortToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
            this.sortToolStripMenuItem.Text = "Sort";
            // 
            // sortToolStripComboBox
            // 
            this.sortToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sortToolStripComboBox.Items.AddRange(new object[] {
            "Total",
            "Processor",
            "Memory",
            "Disk",
            "Wireless Network Card"});
            this.sortToolStripComboBox.Name = "sortToolStripComboBox";
            this.sortToolStripComboBox.Size = new System.Drawing.Size(121, 23);
            this.sortToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.sortToolStripComboBox_SelectedIndexChanged);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutPTopToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.aboutToolStripMenuItem.Text = "Help";
            // 
            // aboutPTopToolStripMenuItem
            // 
            this.aboutPTopToolStripMenuItem.Name = "aboutPTopToolStripMenuItem";
            this.aboutPTopToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.aboutPTopToolStripMenuItem.Text = "About pTopW";
            // 
            // statusStrip
            // 
            this.statusStrip.Location = new System.Drawing.Point(0, 555);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(684, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(684, 577);
            this.Controls.Add(this.processGroupBox);
            this.Controls.Add(this.componentGroupBox);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.Name = "MainWindow";
            this.Text = "pTopW";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWindow_FormClosing);
            this.componentGroupBox.ResumeLayout(false);
            this.componentGroupBox.PerformLayout();
            this.processGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.processDataGridView)).EndInit();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutPTopToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox componentGroupBox;
        private System.Windows.Forms.GroupBox processGroupBox;
        private System.Windows.Forms.Label cpuLabel;
        private System.Windows.Forms.Label memLabel;
        private System.Windows.Forms.Label cpuEnergyLabel;
        private System.Windows.Forms.Label wnicEnergyLabel;
        private System.Windows.Forms.Label wnicLabel;
        private System.Windows.Forms.Label diskEnergyLabel;
        private System.Windows.Forms.Label diskLabel;
        private System.Windows.Forms.Label memEnergyLabel;
        private System.Windows.Forms.DataGridView processDataGridView;
        private System.Windows.Forms.ToolStripMenuItem setupWirelessToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem energyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem powerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sortToolStripMenuItem;
        private System.Windows.Forms.ToolStripComboBox sortToolStripComboBox;
        private System.Windows.Forms.Button calButton;
        private System.Windows.Forms.Label secLabel;
        private System.Windows.Forms.TextBox timeTextBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcessNameColum;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalEnergy;
        private System.Windows.Forms.DataGridViewTextBoxColumn CPUEnergy;
        private System.Windows.Forms.DataGridViewTextBoxColumn MEMEnergy;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiskEnergy;
        private System.Windows.Forms.DataGridViewTextBoxColumn WNICEnergy;
    }
}

