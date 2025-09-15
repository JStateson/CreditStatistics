namespace CreditStatistics
{
    partial class ShowData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowData));
            tbInfo = new TextBox();
            tbHdrInfo = new TextBox();
            btnRead = new Button();
            timerDoSelected = new System.Windows.Forms.Timer(components);
            btnCancel = new Button();
            cbfilterSTD = new CheckBox();
            pbSeq = new ProgressBar();
            btnRunSprint = new Button();
            btnViewCol = new Button();
            tcShowData = new TabControl();
            tabResults = new TabPage();
            tabSettings = new TabPage();
            gbPJs = new GroupBox();
            btnInvertPC = new Button();
            btnClearPC = new Button();
            btnCheckPC = new Button();
            gbPCs = new GroupBox();
            btnSaveSystems = new Button();
            btnInvPJ = new Button();
            btnClearPJ = new Button();
            btnCheckPG = new Button();
            tabEachResult = new TabPage();
            tbEachResult = new TextBox();
            gbProj = new GroupBox();
            btnVisitPage = new Button();
            gbSys = new GroupBox();
            tabCurCol = new TabPage();
            tbCurCol = new TextBox();
            gbAnal = new GroupBox();
            btnReplaceStats = new Button();
            btnAddStats = new Button();
            btnShowAnal = new Button();
            btnLCalcGPU = new Button();
            btnApplyLimit = new Button();
            toolTip1 = new ToolTip(components);
            cbUseSecc = new CheckBox();
            label3 = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            lbBunkerDays = new Label();
            lbRemDays = new Label();
            btnUpdateUTC = new Button();
            tbUTCe = new TextBox();
            label2 = new Label();
            tbUTCs = new TextBox();
            label1 = new Label();
            TimerNoSeq = new System.Windows.Forms.Timer(components);
            groupBox3 = new GroupBox();
            rbAnyStudy = new RadioButton();
            rbUseStudy = new RadioButton();
            tcShowData.SuspendLayout();
            tabResults.SuspendLayout();
            tabSettings.SuspendLayout();
            gbPJs.SuspendLayout();
            gbPCs.SuspendLayout();
            tabEachResult.SuspendLayout();
            gbProj.SuspendLayout();
            tabCurCol.SuspendLayout();
            gbAnal.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // tbInfo
            // 
            tbInfo.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbInfo.Location = new Point(3, 6);
            tbInfo.Multiline = true;
            tbInfo.Name = "tbInfo";
            tbInfo.ScrollBars = ScrollBars.Vertical;
            tbInfo.Size = new Size(823, 680);
            tbInfo.TabIndex = 0;
            // 
            // tbHdrInfo
            // 
            tbHdrInfo.Location = new Point(173, 72);
            tbHdrInfo.Multiline = true;
            tbHdrInfo.Name = "tbHdrInfo";
            tbHdrInfo.ScrollBars = ScrollBars.Both;
            tbHdrInfo.Size = new Size(319, 225);
            tbHdrInfo.TabIndex = 1;
            tbHdrInfo.WordWrap = false;
            // 
            // btnRead
            // 
            btnRead.Location = new Point(12, 72);
            btnRead.Name = "btnRead";
            btnRead.Size = new Size(98, 23);
            btnRead.TabIndex = 2;
            btnRead.Text = "Fetch Again";
            btnRead.UseVisualStyleBackColor = true;
            btnRead.Click += btnRead_Click;
            // 
            // timerDoSelected
            // 
            timerDoSelected.Interval = 1000;
            timerDoSelected.Tick += timerDoSelected_Tick;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(22, 25);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // cbfilterSTD
            // 
            cbfilterSTD.AutoSize = true;
            cbfilterSTD.Enabled = false;
            cbfilterSTD.Location = new Point(940, 12);
            cbfilterSTD.Name = "cbfilterSTD";
            cbfilterSTD.Size = new Size(86, 19);
            cbfilterSTD.TabIndex = 5;
            cbfilterSTD.Text = "Apply Filter";
            cbfilterSTD.UseVisualStyleBackColor = true;
            cbfilterSTD.CheckedChanged += cbfilterSTD_CheckedChanged;
            // 
            // pbSeq
            // 
            pbSeq.Location = new Point(173, 25);
            pbSeq.Name = "pbSeq";
            pbSeq.Size = new Size(309, 23);
            pbSeq.TabIndex = 6;
            // 
            // btnRunSprint
            // 
            btnRunSprint.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRunSprint.ForeColor = SystemColors.Highlight;
            btnRunSprint.Location = new Point(22, 22);
            btnRunSprint.Name = "btnRunSprint";
            btnRunSprint.Size = new Size(85, 36);
            btnRunSprint.TabIndex = 7;
            btnRunSprint.Text = "Run sprint";
            btnRunSprint.UseVisualStyleBackColor = true;
            btnRunSprint.Click += btnRunSprint_Click;
            // 
            // btnViewCol
            // 
            btnViewCol.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnViewCol.ForeColor = SystemColors.Highlight;
            btnViewCol.Location = new Point(12, 119);
            btnViewCol.Name = "btnViewCol";
            btnViewCol.Size = new Size(111, 44);
            btnViewCol.TabIndex = 8;
            btnViewCol.Text = "View Current\r\ncollection";
            btnViewCol.UseVisualStyleBackColor = true;
            btnViewCol.Click += btnViewCol_Click;
            // 
            // tcShowData
            // 
            tcShowData.Controls.Add(tabResults);
            tcShowData.Controls.Add(tabSettings);
            tcShowData.Controls.Add(tabEachResult);
            tcShowData.Controls.Add(tabCurCol);
            tcShowData.Location = new Point(498, 37);
            tcShowData.Name = "tcShowData";
            tcShowData.SelectedIndex = 0;
            tcShowData.Size = new Size(840, 720);
            tcShowData.TabIndex = 9;
            // 
            // tabResults
            // 
            tabResults.Controls.Add(tbInfo);
            tabResults.Location = new Point(4, 24);
            tabResults.Name = "tabResults";
            tabResults.Padding = new Padding(3);
            tabResults.Size = new Size(832, 692);
            tabResults.TabIndex = 0;
            tabResults.Text = "Summery Results";
            tabResults.UseVisualStyleBackColor = true;
            // 
            // tabSettings
            // 
            tabSettings.Controls.Add(gbPJs);
            tabSettings.Controls.Add(gbPCs);
            tabSettings.Location = new Point(4, 24);
            tabSettings.Name = "tabSettings";
            tabSettings.Padding = new Padding(3);
            tabSettings.Size = new Size(832, 692);
            tabSettings.TabIndex = 1;
            tabSettings.Text = "Acquire Data";
            tabSettings.UseVisualStyleBackColor = true;
            // 
            // gbPJs
            // 
            gbPJs.Controls.Add(btnInvertPC);
            gbPJs.Controls.Add(btnClearPC);
            gbPJs.Controls.Add(btnCheckPC);
            gbPJs.Location = new Point(438, 34);
            gbPJs.Name = "gbPJs";
            gbPJs.Size = new Size(312, 409);
            gbPJs.TabIndex = 1;
            gbPJs.TabStop = false;
            gbPJs.Text = "Sprint Projects in blue, hover to get study";
            // 
            // btnInvertPC
            // 
            btnInvertPC.Location = new Point(220, 129);
            btnInvertPC.Name = "btnInvertPC";
            btnInvertPC.Size = new Size(75, 23);
            btnInvertPC.TabIndex = 8;
            btnInvertPC.Text = "Invert";
            btnInvertPC.UseVisualStyleBackColor = true;
            btnInvertPC.Click += ChangeCKbox;
            // 
            // btnClearPC
            // 
            btnClearPC.Location = new Point(220, 75);
            btnClearPC.Name = "btnClearPC";
            btnClearPC.Size = new Size(75, 23);
            btnClearPC.TabIndex = 7;
            btnClearPC.Text = "Clear all";
            btnClearPC.UseVisualStyleBackColor = true;
            btnClearPC.Click += ChangeCKbox;
            // 
            // btnCheckPC
            // 
            btnCheckPC.Location = new Point(220, 28);
            btnCheckPC.Name = "btnCheckPC";
            btnCheckPC.Size = new Size(75, 23);
            btnCheckPC.TabIndex = 6;
            btnCheckPC.Text = "Check all";
            btnCheckPC.UseVisualStyleBackColor = true;
            btnCheckPC.Click += ChangeCKbox;
            // 
            // gbPCs
            // 
            gbPCs.Controls.Add(btnSaveSystems);
            gbPCs.Controls.Add(btnInvPJ);
            gbPCs.Controls.Add(btnClearPJ);
            gbPCs.Controls.Add(btnCheckPG);
            gbPCs.Location = new Point(50, 34);
            gbPCs.Name = "gbPCs";
            gbPCs.Size = new Size(312, 409);
            gbPCs.TabIndex = 0;
            gbPCs.TabStop = false;
            gbPCs.Text = "Sprint PCs";
            // 
            // btnSaveSystems
            // 
            btnSaveSystems.Location = new Point(218, 347);
            btnSaveSystems.Name = "btnSaveSystems";
            btnSaveSystems.Size = new Size(75, 23);
            btnSaveSystems.TabIndex = 6;
            btnSaveSystems.Text = "Save";
            btnSaveSystems.UseVisualStyleBackColor = true;
            btnSaveSystems.Visible = false;
            btnSaveSystems.Click += btnSaveSystems_Click;
            // 
            // btnInvPJ
            // 
            btnInvPJ.Location = new Point(218, 129);
            btnInvPJ.Name = "btnInvPJ";
            btnInvPJ.Size = new Size(75, 23);
            btnInvPJ.TabIndex = 5;
            btnInvPJ.Text = "Invert";
            btnInvPJ.UseVisualStyleBackColor = true;
            btnInvPJ.Click += ChangeCKbox;
            // 
            // btnClearPJ
            // 
            btnClearPJ.Location = new Point(218, 75);
            btnClearPJ.Name = "btnClearPJ";
            btnClearPJ.Size = new Size(75, 23);
            btnClearPJ.TabIndex = 4;
            btnClearPJ.Text = "Clear all";
            btnClearPJ.UseVisualStyleBackColor = true;
            btnClearPJ.Click += ChangeCKbox;
            // 
            // btnCheckPG
            // 
            btnCheckPG.Location = new Point(218, 28);
            btnCheckPG.Name = "btnCheckPG";
            btnCheckPG.Size = new Size(75, 23);
            btnCheckPG.TabIndex = 3;
            btnCheckPG.Text = "Check all";
            btnCheckPG.UseVisualStyleBackColor = true;
            btnCheckPG.Click += ChangeCKbox;
            // 
            // tabEachResult
            // 
            tabEachResult.Controls.Add(tbEachResult);
            tabEachResult.Controls.Add(gbProj);
            tabEachResult.Controls.Add(gbSys);
            tabEachResult.Location = new Point(4, 24);
            tabEachResult.Name = "tabEachResult";
            tabEachResult.Size = new Size(832, 692);
            tabEachResult.TabIndex = 2;
            tabEachResult.Text = "Individual Results";
            tabEachResult.UseVisualStyleBackColor = true;
            // 
            // tbEachResult
            // 
            tbEachResult.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbEachResult.Location = new Point(5, 6);
            tbEachResult.Multiline = true;
            tbEachResult.Name = "tbEachResult";
            tbEachResult.ScrollBars = ScrollBars.Vertical;
            tbEachResult.Size = new Size(824, 387);
            tbEachResult.TabIndex = 1;
            // 
            // gbProj
            // 
            gbProj.Controls.Add(btnVisitPage);
            gbProj.Location = new Point(19, 543);
            gbProj.Name = "gbProj";
            gbProj.Size = new Size(810, 125);
            gbProj.TabIndex = 3;
            gbProj.TabStop = false;
            gbProj.Text = "Projects";
            // 
            // btnVisitPage
            // 
            btnVisitPage.Location = new Point(719, 51);
            btnVisitPage.Name = "btnVisitPage";
            btnVisitPage.Size = new Size(75, 23);
            btnVisitPage.TabIndex = 0;
            btnVisitPage.Text = "Visit Web";
            btnVisitPage.UseVisualStyleBackColor = true;
            btnVisitPage.Click += btnVisitPage_Click;
            // 
            // gbSys
            // 
            gbSys.Location = new Point(19, 399);
            gbSys.Name = "gbSys";
            gbSys.Size = new Size(810, 138);
            gbSys.TabIndex = 2;
            gbSys.TabStop = false;
            gbSys.Text = "System";
            // 
            // tabCurCol
            // 
            tabCurCol.Controls.Add(tbCurCol);
            tabCurCol.Location = new Point(4, 24);
            tabCurCol.Name = "tabCurCol";
            tabCurCol.Size = new Size(832, 692);
            tabCurCol.TabIndex = 3;
            tabCurCol.Text = "Current Limits";
            tabCurCol.UseVisualStyleBackColor = true;
            // 
            // tbCurCol
            // 
            tbCurCol.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            tbCurCol.Location = new Point(5, 4);
            tbCurCol.Multiline = true;
            tbCurCol.Name = "tbCurCol";
            tbCurCol.ScrollBars = ScrollBars.Vertical;
            tbCurCol.Size = new Size(823, 685);
            tbCurCol.TabIndex = 1;
            // 
            // gbAnal
            // 
            gbAnal.Controls.Add(btnReplaceStats);
            gbAnal.Controls.Add(btnAddStats);
            gbAnal.Controls.Add(btnShowAnal);
            gbAnal.Enabled = false;
            gbAnal.Location = new Point(12, 464);
            gbAnal.Name = "gbAnal";
            gbAnal.Size = new Size(139, 253);
            gbAnal.TabIndex = 10;
            gbAnal.TabStop = false;
            gbAnal.Text = "Update averages";
            // 
            // btnReplaceStats
            // 
            btnReplaceStats.Location = new Point(10, 192);
            btnReplaceStats.Name = "btnReplaceStats";
            btnReplaceStats.Size = new Size(88, 43);
            btnReplaceStats.TabIndex = 2;
            btnReplaceStats.Text = "2b - Replace\r\nStatistics";
            toolTip1.SetToolTip(btnReplaceStats, "Replace the old statistics with these new values\r\n");
            btnReplaceStats.UseVisualStyleBackColor = true;
            // 
            // btnAddStats
            // 
            btnAddStats.Location = new Point(10, 114);
            btnAddStats.Name = "btnAddStats";
            btnAddStats.Size = new Size(88, 43);
            btnAddStats.TabIndex = 1;
            btnAddStats.Text = "2a - Add\r\nStatistics";
            toolTip1.SetToolTip(btnAddStats, "This adds two samples together.  If there are no\r\nprevious samples then this is the same as the\r\nreplace samples.");
            btnAddStats.UseVisualStyleBackColor = true;
            // 
            // btnShowAnal
            // 
            btnShowAnal.Location = new Point(13, 35);
            btnShowAnal.Name = "btnShowAnal";
            btnShowAnal.Size = new Size(88, 43);
            btnShowAnal.TabIndex = 0;
            btnShowAnal.Text = "1 - Show\r\nStatistics";
            btnShowAnal.UseVisualStyleBackColor = true;
            btnShowAnal.Click += btnShowAnal_Click;
            // 
            // btnLCalcGPU
            // 
            btnLCalcGPU.Location = new Point(192, 22);
            btnLCalcGPU.Name = "btnLCalcGPU";
            btnLCalcGPU.Size = new Size(88, 43);
            btnLCalcGPU.TabIndex = 5;
            btnLCalcGPU.Text = "3 - Calculate\r\nall limits";
            btnLCalcGPU.UseVisualStyleBackColor = true;
            btnLCalcGPU.Click += btnLCalcGPU_Click;
            // 
            // btnApplyLimit
            // 
            btnApplyLimit.Location = new Point(192, 83);
            btnApplyLimit.Name = "btnApplyLimit";
            btnApplyLimit.Size = new Size(88, 43);
            btnApplyLimit.TabIndex = 4;
            btnApplyLimit.Text = "4 - Save\r\nnew Limit";
            btnApplyLimit.UseVisualStyleBackColor = true;
            btnApplyLimit.Click += btnApplyLimit_Click;
            // 
            // cbUseSecc
            // 
            cbUseSecc.AutoSize = true;
            cbUseSecc.Location = new Point(29, 96);
            cbUseSecc.Name = "cbUseSecc";
            cbUseSecc.Size = new Size(108, 19);
            cbUseSecc.TabIndex = 6;
            cbUseSecc.Text = "Include Job Cnt";
            toolTip1.SetToolTip(cbUseSecc, "subtract successfull jobs from the\r\nrequired job count when calculating\r\nnew limits for the remaining days.");
            cbUseSecc.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.Info;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(13, 155);
            label3.Name = "label3";
            label3.Size = new Size(107, 15);
            label3.TabIndex = 10;
            label3.Text = "Why  This Matters";
            toolTip1.SetToolTip(label3, resources.GetString("label3.ToolTip"));
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cbUseSecc);
            groupBox1.Controls.Add(btnLCalcGPU);
            groupBox1.Controls.Add(btnApplyLimit);
            groupBox1.Location = new Point(173, 346);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(319, 171);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Calculate new limits";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lbBunkerDays);
            groupBox2.Controls.Add(lbRemDays);
            groupBox2.Controls.Add(btnUpdateUTC);
            groupBox2.Controls.Add(tbUTCe);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(tbUTCs);
            groupBox2.Controls.Add(label1);
            groupBox2.Location = new Point(167, 546);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(325, 171);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "Bunker Times";
            // 
            // lbBunkerDays
            // 
            lbBunkerDays.AutoSize = true;
            lbBunkerDays.Location = new Point(6, 135);
            lbBunkerDays.Name = "lbBunkerDays";
            lbBunkerDays.Size = new Size(72, 15);
            lbBunkerDays.TabIndex = 10;
            lbBunkerDays.Text = "Bunker Days";
            // 
            // lbRemDays
            // 
            lbRemDays.AutoSize = true;
            lbRemDays.Location = new Point(6, 103);
            lbRemDays.Name = "lbRemDays";
            lbRemDays.Size = new Size(55, 15);
            lbRemDays.TabIndex = 9;
            lbRemDays.Text = "Days Left";
            // 
            // btnUpdateUTC
            // 
            btnUpdateUTC.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnUpdateUTC.ForeColor = SystemColors.Highlight;
            btnUpdateUTC.Location = new Point(219, 28);
            btnUpdateUTC.Name = "btnUpdateUTC";
            btnUpdateUTC.Size = new Size(96, 52);
            btnUpdateUTC.TabIndex = 8;
            btnUpdateUTC.Text = "Save new\r\nUTC times";
            btnUpdateUTC.UseVisualStyleBackColor = true;
            // 
            // tbUTCe
            // 
            tbUTCe.Location = new Point(68, 57);
            tbUTCe.Name = "tbUTCe";
            tbUTCe.Size = new Size(121, 23);
            tbUTCe.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 57);
            label2.Name = "label2";
            label2.Size = new Size(52, 15);
            label2.TabIndex = 2;
            label2.Text = "UTC End";
            // 
            // tbUTCs
            // 
            tbUTCs.Location = new Point(68, 26);
            tbUTCs.Name = "tbUTCs";
            tbUTCs.Size = new Size(121, 23);
            tbUTCs.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 26);
            label1.Name = "label1";
            label1.Size = new Size(56, 15);
            label1.TabIndex = 0;
            label1.Text = "UTC Start";
            // 
            // TimerNoSeq
            // 
            TimerNoSeq.Interval = 1000;
            TimerNoSeq.Tick += TimerNoSeq_Tick;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label3);
            groupBox3.Controls.Add(rbAnyStudy);
            groupBox3.Controls.Add(rbUseStudy);
            groupBox3.Controls.Add(btnRunSprint);
            groupBox3.Location = new Point(16, 241);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(135, 201);
            groupBox3.TabIndex = 13;
            groupBox3.TabStop = false;
            groupBox3.Text = "Sprint data collection";
            // 
            // rbAnyStudy
            // 
            rbAnyStudy.AutoSize = true;
            rbAnyStudy.Checked = true;
            rbAnyStudy.ForeColor = SystemColors.Highlight;
            rbAnyStudy.Location = new Point(18, 80);
            rbAnyStudy.Name = "rbAnyStudy";
            rbAnyStudy.Size = new Size(92, 19);
            rbAnyStudy.TabIndex = 9;
            rbAnyStudy.TabStop = true;
            rbAnyStudy.Tag = "none";
            rbAnyStudy.Text = "Use any data";
            rbAnyStudy.UseVisualStyleBackColor = true;
            rbAnyStudy.CheckedChanged += rbStudyOption_CheckedChanged;
            // 
            // rbUseStudy
            // 
            rbUseStudy.AutoSize = true;
            rbUseStudy.ForeColor = SystemColors.Highlight;
            rbUseStudy.Location = new Point(18, 115);
            rbUseStudy.Name = "rbUseStudy";
            rbUseStudy.Size = new Size(97, 19);
            rbUseStudy.TabIndex = 8;
            rbUseStudy.Tag = "one";
            rbUseStudy.Text = "Require study";
            rbUseStudy.UseVisualStyleBackColor = true;
            rbUseStudy.CheckedChanged += rbStudyOption_CheckedChanged;
            // 
            // ShowData
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1350, 765);
            Controls.Add(groupBox3);
            Controls.Add(tcShowData);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(gbAnal);
            Controls.Add(btnViewCol);
            Controls.Add(pbSeq);
            Controls.Add(tbHdrInfo);
            Controls.Add(cbfilterSTD);
            Controls.Add(btnCancel);
            Controls.Add(btnRead);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ShowData";
            Text = "ShowData";
            tcShowData.ResumeLayout(false);
            tabResults.ResumeLayout(false);
            tabResults.PerformLayout();
            tabSettings.ResumeLayout(false);
            gbPJs.ResumeLayout(false);
            gbPCs.ResumeLayout(false);
            tabEachResult.ResumeLayout(false);
            tabEachResult.PerformLayout();
            gbProj.ResumeLayout(false);
            tabCurCol.ResumeLayout(false);
            tabCurCol.PerformLayout();
            gbAnal.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox tbInfo;
        private TextBox tbHdrInfo;
        private Button btnRead;
        private System.Windows.Forms.Timer timerDoSelected;
        private Button btnCancel;
        private CheckBox cbfilterSTD;
        private ProgressBar pbSeq;
        private Button btnRunSprint;
        private Button btnViewCol;
        private TabControl tcShowData;
        private TabPage tabResults;
        private TabPage tabSettings;
        private GroupBox gbPJs;
        private GroupBox gbPCs;
        private Button btnInvPJ;
        private Button btnClearPJ;
        private Button btnCheckPG;
        private Button btnInvertPC;
        private Button btnClearPC;
        private Button btnCheckPC;
        private TabPage tabEachResult;
        private TextBox tbEachResult;
        private GroupBox gbProj;
        private GroupBox gbSys;
        private Button btnVisitPage;
        private GroupBox gbAnal;
        private Button btnReplaceStats;
        private Button btnAddStats;
        private Button btnShowAnal;
        private Button btnApplyLimit;
        private ToolTip toolTip1;
        private Button btnLCalcGPU;
        private Button btnSaveSystems;
        private TabPage tabCurCol;
        private TextBox tbCurCol;
        private GroupBox groupBox1;
        private CheckBox cbUseSecc;
        private GroupBox groupBox2;
        private TextBox tbUTCs;
        private Label label1;
        private Label lbBunkerDays;
        private Label lbRemDays;
        private Button btnUpdateUTC;
        private TextBox tbUTCe;
        private Label label2;
        private System.Windows.Forms.Timer TimerNoSeq;
        private GroupBox groupBox3;
        private RadioButton rbUseStudy;
        private Label label3;
        private RadioButton rbAnyStudy;
    }
}