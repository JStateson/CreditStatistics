namespace CreditStatistics
{
    partial class ShowPandora
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowPandora));
            btnSave = new Button();
            gbPCs = new GroupBox();
            tbInfo = new TextBox();
            btnSendApp = new Button();
            label1 = new Label();
            btnRemove = new Button();
            toolTip1 = new ToolTip(components);
            btnPCtoAll = new Button();
            btnSuspAll = new Button();
            btnResAll = new Button();
            btnAllowWork = new Button();
            btnNoWork = new Button();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // btnSave
            // 
            btnSave.ForeColor = SystemColors.Highlight;
            btnSave.Location = new Point(875, 259);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(75, 42);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save Edits";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click;
            // 
            // gbPCs
            // 
            gbPCs.Location = new Point(12, 36);
            gbPCs.Name = "gbPCs";
            gbPCs.Size = new Size(160, 372);
            gbPCs.TabIndex = 6;
            gbPCs.TabStop = false;
            gbPCs.Text = "Sprint PCs";
            // 
            // tbInfo
            // 
            tbInfo.Location = new Point(343, 36);
            tbInfo.Multiline = true;
            tbInfo.Name = "tbInfo";
            tbInfo.ScrollBars = ScrollBars.Vertical;
            tbInfo.Size = new Size(511, 507);
            tbInfo.TabIndex = 8;
            // 
            // btnSendApp
            // 
            btnSendApp.ForeColor = SystemColors.Highlight;
            btnSendApp.Location = new Point(875, 102);
            btnSendApp.Name = "btnSendApp";
            btnSendApp.Size = new Size(75, 42);
            btnSendApp.TabIndex = 9;
            btnSendApp.Text = "Send\r\nTo PC";
            btnSendApp.UseVisualStyleBackColor = true;
            btnSendApp.Click += btnSendApp_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Info;
            label1.Location = new Point(12, 435);
            label1.Name = "label1";
            label1.Size = new Size(284, 135);
            label1.TabIndex = 10;
            label1.Text = resources.GetString("label1.Text");
            // 
            // btnRemove
            // 
            btnRemove.ForeColor = SystemColors.Highlight;
            btnRemove.Location = new Point(875, 180);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(101, 50);
            btnRemove.TabIndex = 11;
            btnRemove.Text = "Remove remote\r\npandora_config";
            toolTip1.SetToolTip(btnRemove, "This only removes the\r\nfile from the remote pc.\r\nIt is not deleted.");
            btnRemove.UseVisualStyleBackColor = true;
            btnRemove.Click += btnRemove_Click;
            // 
            // btnPCtoAll
            // 
            btnPCtoAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnPCtoAll.ForeColor = SystemColors.Highlight;
            btnPCtoAll.Location = new Point(209, 118);
            btnPCtoAll.Name = "btnPCtoAll";
            btnPCtoAll.Size = new Size(116, 42);
            btnPCtoAll.TabIndex = 12;
            btnPCtoAll.Text = "Send pandora\r\nto All PCs";
            btnPCtoAll.UseVisualStyleBackColor = true;
            btnPCtoAll.Click += btnPCtoAll_Click;
            // 
            // btnSuspAll
            // 
            btnSuspAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSuspAll.ForeColor = SystemColors.Highlight;
            btnSuspAll.Location = new Point(209, 183);
            btnSuspAll.Name = "btnSuspAll";
            btnSuspAll.Size = new Size(116, 42);
            btnSuspAll.TabIndex = 13;
            btnSuspAll.Text = "Suspend all\r\nsprint projects";
            btnSuspAll.UseVisualStyleBackColor = true;
            btnSuspAll.Click += btnSuspAll_Click;
            // 
            // btnResAll
            // 
            btnResAll.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnResAll.ForeColor = SystemColors.Highlight;
            btnResAll.Location = new Point(209, 240);
            btnResAll.Name = "btnResAll";
            btnResAll.Size = new Size(116, 42);
            btnResAll.TabIndex = 14;
            btnResAll.Text = "Resume all\r\nsprint projects";
            btnResAll.UseVisualStyleBackColor = true;
            btnResAll.Click += btnResAll_Click;
            // 
            // btnAllowWork
            // 
            btnAllowWork.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAllowWork.ForeColor = SystemColors.Highlight;
            btnAllowWork.Location = new Point(209, 372);
            btnAllowWork.Name = "btnAllowWork";
            btnAllowWork.Size = new Size(116, 42);
            btnAllowWork.TabIndex = 16;
            btnAllowWork.Text = "Allow new work\r\non sprint projects";
            btnAllowWork.UseVisualStyleBackColor = true;
            btnAllowWork.Click += btnAllowWork_Click;
            // 
            // btnNoWork
            // 
            btnNoWork.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNoWork.ForeColor = SystemColors.Highlight;
            btnNoWork.Location = new Point(209, 315);
            btnNoWork.Name = "btnNoWork";
            btnNoWork.Size = new Size(116, 42);
            btnNoWork.TabIndex = 15;
            btnNoWork.Text = "no new work\r\nto all sprints";
            btnNoWork.UseVisualStyleBackColor = true;
            btnNoWork.Click += btnNoWork_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = SystemColors.Info;
            label2.Location = new Point(913, 48);
            label2.Name = "label2";
            label2.Size = new Size(128, 30);
            label2.TabIndex = 17;
            label2.Text = "buttons below act only\r\non the PC selected";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = SystemColors.Info;
            label3.Location = new Point(197, 33);
            label3.Name = "label3";
            label3.Size = new Size(125, 60);
            label3.TabIndex = 18;
            label3.Text = "The buttons below act\r\non ALL ENABLED PCs\r\nto the left, not just the\r\nselected PC.";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = SystemColors.Info;
            label4.Location = new Point(889, 411);
            label4.Name = "label4";
            label4.Size = new Size(152, 90);
            label4.TabIndex = 19;
            label4.Text = "If the pandora_config file\r\nis transfered, the command\r\nto read cc_config is issued.\r\nIf the pandora_config file is\r\ndeleted, the command to\r\nrestart boinc is issued.";
            // 
            // ShowPandora
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1107, 579);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(btnAllowWork);
            Controls.Add(btnNoWork);
            Controls.Add(btnResAll);
            Controls.Add(btnSuspAll);
            Controls.Add(btnPCtoAll);
            Controls.Add(tbInfo);
            Controls.Add(btnRemove);
            Controls.Add(label1);
            Controls.Add(btnSave);
            Controls.Add(btnSendApp);
            Controls.Add(gbPCs);
            Name = "ShowPandora";
            Text = "AppProjPan";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl tcAppPan;
        private TabPage tabApp;
        private TextBox tbApp;
        private TabPage tabPandora;
        private TextBox textBox1;
        private Button btnSendPC;
        private Button btnSave;
        private GroupBox gbPCs;
        private TextBox tbInfo;
        private Button btnSendApp;
        private Label label1;
        private Button btnRemove;
        private ToolTip toolTip1;
        private Button btnPCtoAll;
        private Button btnSuspAll;
        private Button btnResAll;
        private Button btnAllowWork;
        private Button btnNoWork;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}