namespace CreditStatistics
{
    partial class CreditStatistics
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            menuStrip1 = new MenuStrip();
            settingsToolStripMenuItem = new ToolStripMenuItem();
            findOtherPCsToolStripMenuItem = new ToolStripMenuItem();
            getStudyInfoToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripMenuItem();
            getAllConfigFilesToolStripMenuItem = new ToolStripMenuItem();
            passwordInfoToolStripMenuItem = new ToolStripMenuItem();
            runOptionsToolStripMenuItem = new ToolStripMenuItem();
            ContactProject = new ToolStripMenuItem();
            bunkerControlsToolStripMenuItem = new ToolStripMenuItem();
            seeWhatsBunkeredToolStripMenuItem = new ToolStripMenuItem();
            seeRemoteConfigFilesToolStripMenuItem = new ToolStripMenuItem();
            communicateWithPCsToolStripMenuItem = new ToolStripMenuItem();
            slprintPreperationToolStripMenuItem = new ToolStripMenuItem();
            tsmAssignCpuGpu = new ToolStripMenuItem();
            tsmAssignStudy = new ToolStripMenuItem();
            tsmMinWUsNeeded = new ToolStripMenuItem();
            tsmSendSC = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            collectDataToolStripMenuItem = new ToolStripMenuItem();
            ShowPandoraMenuItem = new ToolStripMenuItem();
            showEditAppConfigToolStripMenuItem = new ToolStripMenuItem();
            whatToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            tsmWhatIs = new ToolStripMenuItem();
            tsmInitSetup = new ToolStripMenuItem();
            tsmGetStats = new ToolStripMenuItem();
            tsmSaveData = new ToolStripMenuItem();
            tasmSprintCfg = new ToolStripMenuItem();
            tsmCreateLimits = new ToolStripMenuItem();
            tsmRunSprint = new ToolStripMenuItem();
            groupBox1 = new GroupBox();
            btnRun = new Button();
            btnViewPage = new Button();
            btnPaste = new Button();
            tbInfo = new TextBox();
            groupBox2 = new GroupBox();
            cbStudyAvail = new ComboBox();
            btnGetData = new Button();
            tbProjID = new TextBox();
            tbStudyID = new TextBox();
            tbPCname = new TextBox();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            cbPCavail = new ComboBox();
            tbPage = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            gbSamURL = new GroupBox();
            tbProjUrl = new TextBox();
            label1 = new Label();
            menuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { settingsToolStripMenuItem, runOptionsToolStripMenuItem, bunkerControlsToolStripMenuItem, slprintPreperationToolStripMenuItem, whatToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(934, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // settingsToolStripMenuItem
            // 
            settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { findOtherPCsToolStripMenuItem, getStudyInfoToolStripMenuItem, toolStripMenuItem1, getAllConfigFilesToolStripMenuItem, passwordInfoToolStripMenuItem });
            settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            settingsToolStripMenuItem.Size = new Size(49, 20);
            settingsToolStripMenuItem.Text = "Setup";
            // 
            // findOtherPCsToolStripMenuItem
            // 
            findOtherPCsToolStripMenuItem.Name = "findOtherPCsToolStripMenuItem";
            findOtherPCsToolStripMenuItem.Size = new Size(204, 22);
            findOtherPCsToolStripMenuItem.Text = "Find Projects and PCs";
            findOtherPCsToolStripMenuItem.Click += findOtherPCsToolStripMenuItem_Click;
            // 
            // getStudyInfoToolStripMenuItem
            // 
            getStudyInfoToolStripMenuItem.Name = "getStudyInfoToolStripMenuItem";
            getStudyInfoToolStripMenuItem.Size = new Size(204, 22);
            getStudyInfoToolStripMenuItem.Text = "Get Study Info";
            getStudyInfoToolStripMenuItem.Click += getStudyInfoToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(204, 22);
            toolStripMenuItem1.Text = "View/Edit all app configs";
            toolStripMenuItem1.Click += toolStripMenuItem1_Click;
            // 
            // getAllConfigFilesToolStripMenuItem
            // 
            getAllConfigFilesToolStripMenuItem.Name = "getAllConfigFilesToolStripMenuItem";
            getAllConfigFilesToolStripMenuItem.Size = new Size(204, 22);
            getAllConfigFilesToolStripMenuItem.Text = " View app versions";
            getAllConfigFilesToolStripMenuItem.Click += getAllConfigFilesToolStripMenuItem_Click;
            // 
            // passwordInfoToolStripMenuItem
            // 
            passwordInfoToolStripMenuItem.Name = "passwordInfoToolStripMenuItem";
            passwordInfoToolStripMenuItem.Size = new Size(204, 22);
            passwordInfoToolStripMenuItem.Text = "Password Info";
            passwordInfoToolStripMenuItem.Click += passwordInfoToolStripMenuItem_Click;
            // 
            // runOptionsToolStripMenuItem
            // 
            runOptionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { ContactProject });
            runOptionsToolStripMenuItem.Name = "runOptionsToolStripMenuItem";
            runOptionsToolStripMenuItem.Size = new Size(63, 20);
            runOptionsToolStripMenuItem.Text = "Pandora";
            // 
            // ContactProject
            // 
            ContactProject.Name = "ContactProject";
            ContactProject.Size = new Size(180, 22);
            ContactProject.Text = "Contact Projects";
            ContactProject.Click += ContactProject_Click;
            // 
            // bunkerControlsToolStripMenuItem
            // 
            bunkerControlsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { seeWhatsBunkeredToolStripMenuItem, seeRemoteConfigFilesToolStripMenuItem, communicateWithPCsToolStripMenuItem });
            bunkerControlsToolStripMenuItem.Name = "bunkerControlsToolStripMenuItem";
            bunkerControlsToolStripMenuItem.Size = new Size(104, 20);
            bunkerControlsToolStripMenuItem.Text = "Bunker Controls";
            // 
            // seeWhatsBunkeredToolStripMenuItem
            // 
            seeWhatsBunkeredToolStripMenuItem.Name = "seeWhatsBunkeredToolStripMenuItem";
            seeWhatsBunkeredToolStripMenuItem.Size = new Size(199, 22);
            seeWhatsBunkeredToolStripMenuItem.Text = "See what's bunkered";
            seeWhatsBunkeredToolStripMenuItem.Click += seeWhatsBunkeredToolStripMenuItem_Click;
            // 
            // seeRemoteConfigFilesToolStripMenuItem
            // 
            seeRemoteConfigFilesToolStripMenuItem.Name = "seeRemoteConfigFilesToolStripMenuItem";
            seeRemoteConfigFilesToolStripMenuItem.Size = new Size(199, 22);
            seeRemoteConfigFilesToolStripMenuItem.Text = "Edit all config files";
            seeRemoteConfigFilesToolStripMenuItem.Click += seeRemoteConfigFilesToolStripMenuItem_Click;
            // 
            // communicateWithPCsToolStripMenuItem
            // 
            communicateWithPCsToolStripMenuItem.Name = "communicateWithPCsToolStripMenuItem";
            communicateWithPCsToolStripMenuItem.Size = new Size(199, 22);
            communicateWithPCsToolStripMenuItem.Text = "Communicate with PCs";
            communicateWithPCsToolStripMenuItem.Click += communicateWithPCsToolStripMenuItem_Click;
            // 
            // slprintPreperationToolStripMenuItem
            // 
            slprintPreperationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tsmAssignCpuGpu, tsmAssignStudy, tsmMinWUsNeeded, tsmSendSC, toolStripSeparator1, collectDataToolStripMenuItem, ShowPandoraMenuItem, showEditAppConfigToolStripMenuItem });
            slprintPreperationToolStripMenuItem.Name = "slprintPreperationToolStripMenuItem";
            slprintPreperationToolStripMenuItem.Size = new Size(77, 20);
            slprintPreperationToolStripMenuItem.Text = "Sprint Prep";
            // 
            // tsmAssignCpuGpu
            // 
            tsmAssignCpuGpu.Name = "tsmAssignCpuGpu";
            tsmAssignCpuGpu.Size = new Size(234, 22);
            tsmAssignCpuGpu.Text = "Assign Cpu or Gpu to projects";
            tsmAssignCpuGpu.Click += tsmAssignCpuGpu_Click;
            // 
            // tsmAssignStudy
            // 
            tsmAssignStudy.Name = "tsmAssignStudy";
            tsmAssignStudy.Size = new Size(234, 22);
            tsmAssignStudy.Text = "Assign Study to be used";
            tsmAssignStudy.Click += tsmAssignStudy_Click;
            // 
            // tsmMinWUsNeeded
            // 
            tsmMinWUsNeeded.Name = "tsmMinWUsNeeded";
            tsmMinWUsNeeded.Size = new Size(234, 22);
            tsmMinWUsNeeded.Text = "Assign minimum WUs needed";
            tsmMinWUsNeeded.Click += tsmMinWUsNeeded_Click;
            // 
            // tsmSendSC
            // 
            tsmSendSC.Name = "tsmSendSC";
            tsmSendSC.Size = new Size(234, 22);
            tsmSendSC.Text = "Start minimum WU collection";
            tsmSendSC.Click += tsmSendSC_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(231, 6);
            // 
            // collectDataToolStripMenuItem
            // 
            collectDataToolStripMenuItem.Name = "collectDataToolStripMenuItem";
            collectDataToolStripMenuItem.Size = new Size(234, 22);
            collectDataToolStripMenuItem.Text = "Collect Data make limits";
            collectDataToolStripMenuItem.Click += collectDataToolStripMenuItem_Click;
            // 
            // ShowPandoraMenuItem
            // 
            ShowPandoraMenuItem.Name = "ShowPandoraMenuItem";
            ShowPandoraMenuItem.Size = new Size(234, 22);
            ShowPandoraMenuItem.Text = "Show Pandora";
            ShowPandoraMenuItem.Click += sendPandoraViewAppsToolStripMenuItem_Click;
            // 
            // showEditAppConfigToolStripMenuItem
            // 
            showEditAppConfigToolStripMenuItem.Name = "showEditAppConfigToolStripMenuItem";
            showEditAppConfigToolStripMenuItem.Size = new Size(234, 22);
            showEditAppConfigToolStripMenuItem.Text = "Show/Edit App Config";
            showEditAppConfigToolStripMenuItem.Click += showEditAppConfigToolStripMenuItem_Click;
            // 
            // whatToolStripMenuItem
            // 
            whatToolStripMenuItem.Name = "whatToolStripMenuItem";
            whatToolStripMenuItem.Size = new Size(127, 20);
            whatToolStripMenuItem.Text = "Check OnLine status";
            whatToolStripMenuItem.Click += whatToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { tsmWhatIs, tsmInitSetup, tsmGetStats, tsmSaveData, tasmSprintCfg, tsmCreateLimits, tsmRunSprint });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(44, 20);
            helpToolStripMenuItem.Text = "Help";
            // 
            // tsmWhatIs
            // 
            tsmWhatIs.Name = "tsmWhatIs";
            tsmWhatIs.Size = new Size(214, 22);
            tsmWhatIs.Text = "What this program is";
            tsmWhatIs.Click += tsmHelp_Click;
            // 
            // tsmInitSetup
            // 
            tsmInitSetup.Name = "tsmInitSetup";
            tsmInitSetup.Size = new Size(214, 22);
            tsmInitSetup.Text = "Initial setup";
            tsmInitSetup.Click += tsmHelp_Click;
            // 
            // tsmGetStats
            // 
            tsmGetStats.Name = "tsmGetStats";
            tsmGetStats.Size = new Size(214, 22);
            tsmGetStats.Text = "How to get credit statistics";
            tsmGetStats.Click += tsmHelp_Click;
            // 
            // tsmSaveData
            // 
            tsmSaveData.Name = "tsmSaveData";
            tsmSaveData.Size = new Size(214, 22);
            tsmSaveData.Text = "How to save credit data";
            tsmSaveData.Click += tsmHelp_Click;
            // 
            // tasmSprintCfg
            // 
            tasmSprintCfg.Name = "tasmSprintCfg";
            tasmSprintCfg.Size = new Size(214, 22);
            tasmSprintCfg.Text = "Configuring for Sprint";
            tasmSprintCfg.Click += tsmHelp_Click;
            // 
            // tsmCreateLimits
            // 
            tsmCreateLimits.Name = "tsmCreateLimits";
            tsmCreateLimits.Size = new Size(214, 22);
            tsmCreateLimits.Text = "Create optimum Sprint";
            tsmCreateLimits.Click += tsmHelp_Click;
            // 
            // tsmRunSprint
            // 
            tsmRunSprint.Name = "tsmRunSprint";
            tsmRunSprint.Size = new Size(214, 22);
            tsmRunSprint.Text = "Running the Sprint";
            tsmRunSprint.Click += tsmHelp_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnRun);
            groupBox1.Controls.Add(btnViewPage);
            groupBox1.Controls.Add(btnPaste);
            groupBox1.Controls.Add(tbInfo);
            groupBox1.Controls.Add(groupBox2);
            groupBox1.Controls.Add(gbSamURL);
            groupBox1.Controls.Add(tbProjUrl);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(12, 56);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(877, 635);
            groupBox1.TabIndex = 3;
            groupBox1.TabStop = false;
            groupBox1.Text = "System and Project selections";
            // 
            // btnRun
            // 
            btnRun.ForeColor = SystemColors.Highlight;
            btnRun.Location = new Point(116, 168);
            btnRun.Name = "btnRun";
            btnRun.Size = new Size(75, 23);
            btnRun.TabIndex = 6;
            btnRun.Text = "RUN";
            btnRun.UseVisualStyleBackColor = true;
            btnRun.Click += btnRun_Click;
            // 
            // btnViewPage
            // 
            btnViewPage.Location = new Point(278, 168);
            btnViewPage.Name = "btnViewPage";
            btnViewPage.Size = new Size(75, 23);
            btnViewPage.TabIndex = 12;
            btnViewPage.Text = "View page";
            btnViewPage.UseVisualStyleBackColor = true;
            btnViewPage.Click += btnViewPage_Click;
            // 
            // btnPaste
            // 
            btnPaste.Location = new Point(24, 168);
            btnPaste.Name = "btnPaste";
            btnPaste.Size = new Size(75, 23);
            btnPaste.TabIndex = 5;
            btnPaste.Text = "Paste";
            btnPaste.UseVisualStyleBackColor = true;
            btnPaste.Click += btnPaste_Click;
            // 
            // tbInfo
            // 
            tbInfo.Location = new Point(603, 35);
            tbInfo.Multiline = true;
            tbInfo.Name = "tbInfo";
            tbInfo.ScrollBars = ScrollBars.Horizontal;
            tbInfo.Size = new Size(255, 193);
            tbInfo.TabIndex = 4;
            tbInfo.WordWrap = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(cbStudyAvail);
            groupBox2.Controls.Add(btnGetData);
            groupBox2.Controls.Add(tbProjID);
            groupBox2.Controls.Add(tbStudyID);
            groupBox2.Controls.Add(tbPCname);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(cbPCavail);
            groupBox2.Controls.Add(tbPage);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(label2);
            groupBox2.Location = new Point(438, 261);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(420, 352);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Selected Options";
            // 
            // cbStudyAvail
            // 
            cbStudyAvail.FormattingEnabled = true;
            cbStudyAvail.Location = new Point(154, 196);
            cbStudyAvail.MaxDropDownItems = 4;
            cbStudyAvail.Name = "cbStudyAvail";
            cbStudyAvail.Size = new Size(175, 23);
            cbStudyAvail.TabIndex = 5;
            cbStudyAvail.SelectedIndexChanged += cbStudyAvail_SelectedIndexChanged;
            // 
            // btnGetData
            // 
            btnGetData.ForeColor = SystemColors.Highlight;
            btnGetData.Location = new Point(275, 21);
            btnGetData.Name = "btnGetData";
            btnGetData.Size = new Size(87, 23);
            btnGetData.TabIndex = 11;
            btnGetData.Text = "Get Statistics";
            btnGetData.UseVisualStyleBackColor = true;
            btnGetData.Click += btnGetData_Click;
            // 
            // tbProjID
            // 
            tbProjID.Location = new Point(123, 126);
            tbProjID.Name = "tbProjID";
            tbProjID.ReadOnly = true;
            tbProjID.Size = new Size(100, 23);
            tbProjID.TabIndex = 10;
            // 
            // tbStudyID
            // 
            tbStudyID.Location = new Point(123, 88);
            tbStudyID.Name = "tbStudyID";
            tbStudyID.ReadOnly = true;
            tbStudyID.Size = new Size(100, 23);
            tbStudyID.TabIndex = 9;
            // 
            // tbPCname
            // 
            tbPCname.Location = new Point(123, 54);
            tbPCname.Name = "tbPCname";
            tbPCname.ReadOnly = true;
            tbPCname.Size = new Size(100, 23);
            tbPCname.TabIndex = 8;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(8, 129);
            label7.Name = "label7";
            label7.Size = new Size(85, 15);
            label7.TabIndex = 7;
            label7.Text = "Current Proj ID";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 88);
            label6.Name = "label6";
            label6.Size = new Size(80, 15);
            label6.TabIndex = 6;
            label6.Text = "Current Study";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(6, 54);
            label5.Name = "label5";
            label5.Size = new Size(65, 15);
            label5.TabIndex = 1;
            label5.Text = "Current PC";
            // 
            // cbPCavail
            // 
            cbPCavail.FormattingEnabled = true;
            cbPCavail.Location = new Point(6, 196);
            cbPCavail.MaxDropDownItems = 4;
            cbPCavail.Name = "cbPCavail";
            cbPCavail.Size = new Size(114, 23);
            cbPCavail.TabIndex = 4;
            cbPCavail.SelectedIndexChanged += cbPCavail_SelectedIndexChanged;
            cbPCavail.TextChanged += cbPCavail_TextChanged;
            // 
            // tbPage
            // 
            tbPage.Location = new Point(123, 22);
            tbPage.Name = "tbPage";
            tbPage.ReadOnly = true;
            tbPage.Size = new Size(44, 23);
            tbPage.TabIndex = 3;
            tbPage.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 25);
            label4.Name = "label4";
            label4.Size = new Size(76, 15);
            label4.TabIndex = 2;
            label4.Text = "Current Page";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(154, 178);
            label3.Name = "label3";
            label3.Size = new Size(96, 15);
            label3.TabIndex = 1;
            label3.Text = "Available Studies";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(8, 178);
            label2.Name = "label2";
            label2.Size = new Size(78, 15);
            label2.TabIndex = 0;
            label2.Text = "Available PCs";
            // 
            // gbSamURL
            // 
            gbSamURL.Location = new Point(24, 261);
            gbSamURL.Name = "gbSamURL";
            gbSamURL.Size = new Size(364, 352);
            gbSamURL.TabIndex = 2;
            gbSamURL.TabStop = false;
            gbSamURL.Text = "Select Project";
            // 
            // tbProjUrl
            // 
            tbProjUrl.Location = new Point(24, 107);
            tbProjUrl.Multiline = true;
            tbProjUrl.Name = "tbProjUrl";
            tbProjUrl.ReadOnly = true;
            tbProjUrl.Size = new Size(547, 43);
            tbProjUrl.TabIndex = 1;
            tbProjUrl.Text = "https://milkyway.cs.rpi.edu/milkyway/results.php?hostid=1046337&offset=0&show_names=0&state=4&appid=";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.Location = new Point(24, 35);
            label1.Name = "label1";
            label1.Size = new Size(270, 45);
            label1.TabIndex = 0;
            label1.Text = "Paste your URL in below box and click RUN to get\r\nstatistics.  The URL must contain the phrase HOST\r\nAlternately select a project and it's study.\r\n";
            // 
            // CreditStatistics
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(934, 703);
            Controls.Add(groupBox1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "CreditStatistics";
            Text = "Credit Statistics";
            FormClosing += CreditStatistics_FormClosing;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ContextMenuStrip contextMenuStrip1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem settingsToolStripMenuItem;
        private ToolStripMenuItem getStudyInfoToolStripMenuItem;
        private GroupBox groupBox1;
        private TextBox tbProjUrl;
        private Label label1;
        private GroupBox gbSamURL;
        private GroupBox groupBox2;
        private Label label4;
        private Label label2;
        private ComboBox cbStudyAvail;
        private ComboBox cbPCavail;
        private TextBox tbPage;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label3;
        private TextBox tbProjID;
        private TextBox tbStudyID;
        private TextBox tbPCname;
        private Button btnGetData;
        private ToolStripMenuItem findOtherPCsToolStripMenuItem;
        private ToolStripMenuItem runOptionsToolStripMenuItem;
        private ToolStripMenuItem ContactProject;
        private Button btnViewPage;
        private TextBox tbInfo;
        private Button btnPaste;
        private Button btnRun;
        private ToolStripMenuItem bunkerControlsToolStripMenuItem;
        private ToolStripMenuItem seeWhatsBunkeredToolStripMenuItem;
        private ToolStripMenuItem seeRemoteConfigFilesToolStripMenuItem;
        private ToolStripMenuItem communicateWithPCsToolStripMenuItem;
        private ToolStripMenuItem slprintPreperationToolStripMenuItem;
        private ToolStripMenuItem collectDataToolStripMenuItem;
        private ToolStripMenuItem tsmAssignCpuGpu;
        private ToolStripMenuItem ShowPandoraMenuItem;
        private ToolStripMenuItem showEditAppConfigToolStripMenuItem;
        private ToolStripMenuItem whatToolStripMenuItem;
        private ToolStripMenuItem passwordInfoToolStripMenuItem;
        private ToolStripMenuItem getAllConfigFilesToolStripMenuItem;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripMenuItem tsmWhatIs;
        private ToolStripMenuItem tsmInitSetup;
        private ToolStripMenuItem tsmGetStats;
        private ToolStripMenuItem tsmSaveData;
        private ToolStripMenuItem tasmSprintCfg;
        private ToolStripMenuItem tsmCreateLimits;
        private ToolStripMenuItem tsmRunSprint;
        private ToolStripMenuItem tsmAssignStudy;
        private ToolStripMenuItem tsmMinWUsNeeded;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem tsmSendSC;
    }
}
