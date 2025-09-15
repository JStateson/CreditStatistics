namespace CreditStatistics
{
    partial class EditAllAppConfigs
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
            components = new System.ComponentModel.Container();
            radioBoxGroup1 = new RadioBoxGroup();
            rtbLocalHostsBT = new RichTextBox();
            cmsConfigOptions = new ContextMenuStrip(components);
            tsmFetch1cc = new ToolStripMenuItem();
            tsmAllac = new ToolStripMenuItem();
            tsm1ac = new ToolStripMenuItem();
            tsmForceRead = new ToolStripMenuItem();
            tsmAllRead = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            cutToolStripMenuItem = new ToolStripMenuItem();
            copyToolStripMenuItem = new ToolStripMenuItem();
            pasteToolStripMenuItem = new ToolStripMenuItem();
            deleteToolStripMenuItem = new ToolStripMenuItem();
            btnCountAC = new Button();
            btnSendAppConfig = new Button();
            btnFetchCCconfig = new Button();
            tbCCconfig = new TextBox();
            btnSendCcConfig = new Button();
            label3 = new Label();
            cmsConfigOptions.SuspendLayout();
            SuspendLayout();
            // 
            // radioBoxGroup1
            // 
            radioBoxGroup1.Location = new Point(278, 115);
            radioBoxGroup1.Name = "radioBoxGroup1";
            radioBoxGroup1.ProjectStats = null;
            radioBoxGroup1.Size = new Size(264, 473);
            radioBoxGroup1.TabIndex = 5;
            // 
            // rtbLocalHostsBT
            // 
            rtbLocalHostsBT.ContextMenuStrip = cmsConfigOptions;
            rtbLocalHostsBT.Location = new Point(578, 123);
            rtbLocalHostsBT.Name = "rtbLocalHostsBT";
            rtbLocalHostsBT.Size = new Size(356, 465);
            rtbLocalHostsBT.TabIndex = 8;
            rtbLocalHostsBT.Text = "";
            rtbLocalHostsBT.WordWrap = false;
            rtbLocalHostsBT.MouseDown += rtbLocalHostsBT_MouseDown;
            // 
            // cmsConfigOptions
            // 
            cmsConfigOptions.Items.AddRange(new ToolStripItem[] { tsmFetch1cc, tsmAllac, tsm1ac, tsmForceRead, tsmAllRead, toolStripSeparator1, cutToolStripMenuItem, copyToolStripMenuItem, pasteToolStripMenuItem, deleteToolStripMenuItem });
            cmsConfigOptions.Name = "cmsConfigOptions";
            cmsConfigOptions.Size = new Size(254, 208);
            cmsConfigOptions.ItemClicked += cmsConfigOptions_ItemClicked;
            // 
            // tsmFetch1cc
            // 
            tsmFetch1cc.Name = "tsmFetch1cc";
            tsmFetch1cc.Size = new Size(253, 22);
            tsmFetch1cc.Tag = "fetch1cc";
            tsmFetch1cc.Text = "SELECTED PC: get cc_config";
            // 
            // tsmAllac
            // 
            tsmAllac.Name = "tsmAllac";
            tsmAllac.Size = new Size(253, 22);
            tsmAllac.Tag = "fetchALLac";
            tsmAllac.Text = "SELECTED_PC: Get all app_configs";
            // 
            // tsm1ac
            // 
            tsm1ac.Name = "tsm1ac";
            tsm1ac.Size = new Size(253, 22);
            tsm1ac.Tag = "fetch1ac";
            tsm1ac.Text = "SELECTED_PC: get () app_config";
            // 
            // tsmForceRead
            // 
            tsmForceRead.Name = "tsmForceRead";
            tsmForceRead.Size = new Size(253, 22);
            tsmForceRead.Tag = "read1c";
            tsmForceRead.Text = "SELECTED PC: re-read configs";
            // 
            // tsmAllRead
            // 
            tsmAllRead.Name = "tsmAllRead";
            tsmAllRead.Size = new Size(253, 22);
            tsmAllRead.Tag = "readALLc";
            tsmAllRead.Text = "Make all PCs read configs";
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(250, 6);
            // 
            // cutToolStripMenuItem
            // 
            cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            cutToolStripMenuItem.Size = new Size(253, 22);
            cutToolStripMenuItem.Text = "Cut";
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(253, 22);
            copyToolStripMenuItem.Text = "Copy";
            // 
            // pasteToolStripMenuItem
            // 
            pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            pasteToolStripMenuItem.Size = new Size(253, 22);
            pasteToolStripMenuItem.Text = "Paste";
            // 
            // deleteToolStripMenuItem
            // 
            deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            deleteToolStripMenuItem.Size = new Size(253, 22);
            deleteToolStripMenuItem.Text = "Delete";
            // 
            // btnCountAC
            // 
            btnCountAC.ForeColor = SystemColors.Highlight;
            btnCountAC.Location = new Point(432, 22);
            btnCountAC.Name = "btnCountAC";
            btnCountAC.Size = new Size(110, 64);
            btnCountAC.TabIndex = 7;
            btnCountAC.Text = "Fetch app_config\r\nfrom all remotes\r\nfor all projects";
            btnCountAC.UseVisualStyleBackColor = true;
            btnCountAC.Click += btnCountAC_Click;
            // 
            // btnSendAppConfig
            // 
            btnSendAppConfig.ForeColor = SystemColors.Highlight;
            btnSendAppConfig.Location = new Point(685, 45);
            btnSendAppConfig.Name = "btnSendAppConfig";
            btnSendAppConfig.Size = new Size(117, 42);
            btnSendAppConfig.TabIndex = 9;
            btnSendAppConfig.Text = "Update app_config";
            btnSendAppConfig.UseVisualStyleBackColor = true;
            btnSendAppConfig.Click += btnSendAppConfig_Click;
            // 
            // btnFetchCCconfig
            // 
            btnFetchCCconfig.ForeColor = SystemColors.Highlight;
            btnFetchCCconfig.Location = new Point(278, 22);
            btnFetchCCconfig.Name = "btnFetchCCconfig";
            btnFetchCCconfig.Size = new Size(110, 64);
            btnFetchCCconfig.TabIndex = 10;
            btnFetchCCconfig.Text = "Fetch cc_configs\r\nfrom all remotes";
            btnFetchCCconfig.UseVisualStyleBackColor = true;
            btnFetchCCconfig.Click += btnFetchCCconfig_Click;
            // 
            // tbCCconfig
            // 
            tbCCconfig.ContextMenuStrip = cmsConfigOptions;
            tbCCconfig.Location = new Point(957, 123);
            tbCCconfig.Multiline = true;
            tbCCconfig.Name = "tbCCconfig";
            tbCCconfig.ScrollBars = ScrollBars.Both;
            tbCCconfig.Size = new Size(367, 465);
            tbCCconfig.TabIndex = 11;
            tbCCconfig.MouseDown += tbCCconfig_MouseDown;
            // 
            // btnSendCcConfig
            // 
            btnSendCcConfig.ForeColor = SystemColors.Highlight;
            btnSendCcConfig.Location = new Point(1073, 45);
            btnSendCcConfig.Name = "btnSendCcConfig";
            btnSendCcConfig.Size = new Size(115, 42);
            btnSendCcConfig.TabIndex = 12;
            btnSendCcConfig.Text = "Update cc_config";
            btnSendCcConfig.UseVisualStyleBackColor = true;
            btnSendCcConfig.Click += btnSendCcConfig_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.Info;
            label3.Font = new Font("Segoe UI", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(868, 83);
            label3.Name = "label3";
            label3.Size = new Size(146, 26);
            label3.TabIndex = 13;
            label3.Text = "right click mouse in either\r\nbox for additional features";
            // 
            // EditAllAppConfigs
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1336, 616);
            Controls.Add(label3);
            Controls.Add(btnSendCcConfig);
            Controls.Add(tbCCconfig);
            Controls.Add(btnFetchCCconfig);
            Controls.Add(btnSendAppConfig);
            Controls.Add(rtbLocalHostsBT);
            Controls.Add(btnCountAC);
            Controls.Add(radioBoxGroup1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "EditAllAppConfigs";
            Text = "EditAllAppConfigs";
            Controls.SetChildIndex(radioBoxGroup1, 0);
            Controls.SetChildIndex(btnCountAC, 0);
            Controls.SetChildIndex(rtbLocalHostsBT, 0);
            Controls.SetChildIndex(pbUSE, 0);
            Controls.SetChildIndex(btnSendAppConfig, 0);
            Controls.SetChildIndex(btnFetchCCconfig, 0);
            Controls.SetChildIndex(tbCCconfig, 0);
            Controls.SetChildIndex(btnSendCcConfig, 0);
            Controls.SetChildIndex(label3, 0);
            cmsConfigOptions.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private RadioBoxGroup radioBoxGroup1;
        private RichTextBox rtbLocalHostsBT;
        private Button btnCountAC;
        private Button btnSendAppConfig;
        private Button btnFetchCCconfig;
        private TextBox tbCCconfig;
        private Button btnSendCcConfig;
        private Label label3;
        private ContextMenuStrip cmsConfigOptions;
        private ToolStripMenuItem tsmFetch1cc;
        private ToolStripMenuItem tsmForceRead;
        private ToolStripMenuItem tsm1ac;
        private ToolStripMenuItem tsmAllac;
        private ToolStripMenuItem tsmAllRead;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem cutToolStripMenuItem;
        private ToolStripMenuItem copyToolStripMenuItem;
        private ToolStripMenuItem pasteToolStripMenuItem;
        private ToolStripMenuItem deleteToolStripMenuItem;
    }
}