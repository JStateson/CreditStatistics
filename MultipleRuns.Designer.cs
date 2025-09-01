namespace CreditStatistics
{
    partial class MultipleRuns
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
            gbProj = new GroupBox();
            btnInvPJ = new Button();
            btnClearPJ = new Button();
            btnCheckPG = new Button();
            gbPCs = new GroupBox();
            btnInvertPC = new Button();
            btnClearPC = new Button();
            btnCheckPC = new Button();
            btnAssignProj = new Button();
            btnAssignStudy = new Button();
            label1 = new Label();
            tbInfo = new TextBox();
            btnSetBGsprint = new Button();
            btnSelSprint = new Button();
            gbProj.SuspendLayout();
            gbPCs.SuspendLayout();
            SuspendLayout();
            // 
            // gbProj
            // 
            gbProj.Controls.Add(btnSelSprint);
            gbProj.Controls.Add(btnInvPJ);
            gbProj.Controls.Add(btnClearPJ);
            gbProj.Controls.Add(btnCheckPG);
            gbProj.Location = new Point(12, 12);
            gbProj.Name = "gbProj";
            gbProj.Size = new Size(374, 499);
            gbProj.TabIndex = 0;
            gbProj.TabStop = false;
            gbProj.Text = "Projects to use (Sprint in blue)";
            // 
            // btnInvPJ
            // 
            btnInvPJ.Location = new Point(274, 135);
            btnInvPJ.Name = "btnInvPJ";
            btnInvPJ.Size = new Size(75, 23);
            btnInvPJ.TabIndex = 2;
            btnInvPJ.Text = "Invert";
            btnInvPJ.UseVisualStyleBackColor = true;
            btnInvPJ.Click += ChangeCKbox;
            // 
            // btnClearPJ
            // 
            btnClearPJ.Location = new Point(274, 81);
            btnClearPJ.Name = "btnClearPJ";
            btnClearPJ.Size = new Size(75, 23);
            btnClearPJ.TabIndex = 1;
            btnClearPJ.Text = "Clear all";
            btnClearPJ.UseVisualStyleBackColor = true;
            btnClearPJ.Click += ChangeCKbox;
            // 
            // btnCheckPG
            // 
            btnCheckPG.Location = new Point(274, 34);
            btnCheckPG.Name = "btnCheckPG";
            btnCheckPG.Size = new Size(75, 23);
            btnCheckPG.TabIndex = 0;
            btnCheckPG.Text = "Check all";
            btnCheckPG.UseVisualStyleBackColor = true;
            btnCheckPG.Click += ChangeCKbox;
            // 
            // gbPCs
            // 
            gbPCs.Controls.Add(btnInvertPC);
            gbPCs.Controls.Add(btnClearPC);
            gbPCs.Controls.Add(btnCheckPC);
            gbPCs.Location = new Point(441, 12);
            gbPCs.Name = "gbPCs";
            gbPCs.Size = new Size(374, 499);
            gbPCs.TabIndex = 1;
            gbPCs.TabStop = false;
            gbPCs.Text = "Systems to use";
            // 
            // btnInvertPC
            // 
            btnInvertPC.Location = new Point(277, 135);
            btnInvertPC.Name = "btnInvertPC";
            btnInvertPC.Size = new Size(75, 23);
            btnInvertPC.TabIndex = 5;
            btnInvertPC.Text = "Invert";
            btnInvertPC.UseVisualStyleBackColor = true;
            btnInvertPC.Click += ChangeCKbox;
            // 
            // btnClearPC
            // 
            btnClearPC.Location = new Point(277, 81);
            btnClearPC.Name = "btnClearPC";
            btnClearPC.Size = new Size(75, 23);
            btnClearPC.TabIndex = 4;
            btnClearPC.Text = "Clear all";
            btnClearPC.UseVisualStyleBackColor = true;
            btnClearPC.Click += ChangeCKbox;
            // 
            // btnCheckPC
            // 
            btnCheckPC.Location = new Point(277, 34);
            btnCheckPC.Name = "btnCheckPC";
            btnCheckPC.Size = new Size(75, 23);
            btnCheckPC.TabIndex = 3;
            btnCheckPC.Text = "Check all";
            btnCheckPC.UseVisualStyleBackColor = true;
            btnCheckPC.Click += ChangeCKbox;
            // 
            // btnAssignProj
            // 
            btnAssignProj.ForeColor = SystemColors.Highlight;
            btnAssignProj.Location = new Point(878, 12);
            btnAssignProj.Name = "btnAssignProj";
            btnAssignProj.Size = new Size(123, 61);
            btnAssignProj.TabIndex = 4;
            btnAssignProj.Text = "Assign Projects for\r\nBoincGames sprint";
            btnAssignProj.UseVisualStyleBackColor = true;
            btnAssignProj.Click += btnAssignProj_Click;
            // 
            // btnAssignStudy
            // 
            btnAssignStudy.ForeColor = SystemColors.Highlight;
            btnAssignStudy.Location = new Point(878, 93);
            btnAssignStudy.Name = "btnAssignStudy";
            btnAssignStudy.Size = new Size(123, 61);
            btnAssignStudy.TabIndex = 5;
            btnAssignStudy.Text = "Assign Studies for\r\nBoincGames sprint";
            btnAssignStudy.UseVisualStyleBackColor = true;
            btnAssignStudy.Click += btnAssignStudy_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.Location = new Point(850, 274);
            label1.Name = "label1";
            label1.Size = new Size(185, 60);
            label1.TabIndex = 6;
            label1.Text = "If all PCs use the same projects,\r\nthen select (check) those projects\r\nand also your PCs and then click\r\nthe above 'Assign default' button.";
            // 
            // tbInfo
            // 
            tbInfo.Location = new Point(835, 399);
            tbInfo.Multiline = true;
            tbInfo.Name = "tbInfo";
            tbInfo.Size = new Size(245, 112);
            tbInfo.TabIndex = 7;
            // 
            // btnSetBGsprint
            // 
            btnSetBGsprint.ForeColor = SystemColors.Highlight;
            btnSetBGsprint.Location = new Point(878, 179);
            btnSetBGsprint.Name = "btnSetBGsprint";
            btnSetBGsprint.Size = new Size(123, 61);
            btnSetBGsprint.TabIndex = 3;
            btnSetBGsprint.Text = "Assign default\r\nSprint settings";
            btnSetBGsprint.UseVisualStyleBackColor = true;
            btnSetBGsprint.Click += btnSetBGsprint_Click;
            // 
            // btnSelSprint
            // 
            btnSelSprint.Location = new Point(274, 433);
            btnSelSprint.Name = "btnSelSprint";
            btnSelSprint.Size = new Size(75, 44);
            btnSelSprint.TabIndex = 3;
            btnSelSprint.Text = "Check all\r\nthe Sprints";
            btnSelSprint.UseVisualStyleBackColor = true;
            btnSelSprint.Click += btnSelSprint_Click;
            // 
            // MultipleRuns
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1092, 535);
            Controls.Add(tbInfo);
            Controls.Add(label1);
            Controls.Add(btnSetBGsprint);
            Controls.Add(btnAssignStudy);
            Controls.Add(btnAssignProj);
            Controls.Add(gbPCs);
            Controls.Add(gbProj);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "MultipleRuns";
            Text = "MultipleRuns";
            gbProj.ResumeLayout(false);
            gbPCs.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox gbProj;
        private GroupBox gbPCs;
        private Button btnCheckPG;
        private Button btnInvPJ;
        private Button btnClearPJ;
        private Button btnInvertPC;
        private Button btnClearPC;
        private Button btnCheckPC;
        private Button btnAssignProj;
        private Button btnAssignStudy;
        private Label label1;
        private TextBox tbInfo;
        private Button btnSetBGsprint;
        private Button btnSelSprint;
    }
}